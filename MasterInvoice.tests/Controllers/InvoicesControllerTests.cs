using Domain.Interfaces;
using Entities;
using Entities.Enums;
using MasterInvoice.Controllers;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace MasterInvoice.tests.Controllers
{
        public class InvoicesControllerTests
        {
            private readonly InvoicesController _invoicesController;
            private readonly Mock<IInvoice> _mockInvoiceService;

            public InvoicesControllerTests()
            {
                // Inicializar o Mock do serviço IInvoice
                _mockInvoiceService = new Mock<IInvoice>();

                // Inicializar o controlador com o Mock
                _invoicesController = new InvoicesController(_mockInvoiceService.Object);
            }

            [Fact]
            public async Task Index_ReturnsAllInvoices()
            {
                // Arrange
                var mockInvoices = new List<Invoice>{
                    new Invoice { Id = 1, PayerName = "John Doe", IdentificationNumber = "123456789", InvoiceDoc= "xxx-xxx", BillDoc= "yyty-yy",  IssueDate = new DateTime(2023, 1, 1), Amount = 100, StatusId = StatusType.LatePayment },
                    new Invoice { Id = 2, PayerName = "Jane Smith", IdentificationNumber = "987654321", InvoiceDoc= "xxx-xxx", BillDoc= "yyty-yy", IssueDate = new DateTime(2023, 2, 1), Amount = 200, StatusId = StatusType.PaymentMade },
                    new Invoice { Id = 3, PayerName = "Alice Johnson", IdentificationNumber = "112233445",InvoiceDoc= "xxx-xxx", BillDoc= "yyty-yy",  IssueDate = new DateTime(2023, 3, 1), Amount = 300, StatusId = StatusType.LatePayment }
                    // Add more mock invoices as needed
                }; 
                _mockInvoiceService.Setup(service => service.GetFiltered(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string>()))
                                   .ReturnsAsync(mockInvoices);

                // Act
                var result = await _invoicesController.Index(null, null, null, null) as ViewResult;

                // Assert
                Assert.NotNull(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Invoice>>(result.ViewData.Model);
                Assert.Equal(model.Count(), mockInvoices.Count);
        }

            [Fact]
            public async Task Details_ReturnsViewWithInvoice()
            {
                // Arrange
                int invoiceId = 1;
                var mockInvoice = new Invoice { Id = 1, PayerName = "John Doe", IdentificationNumber = "123456789", InvoiceDoc = "xxx-xxx", BillDoc = "yyty-yy", IssueDate = new DateTime(2023, 1, 1), Amount = 100, StatusId = StatusType.LatePayment };
                _mockInvoiceService.Setup(service => service.GetByID(invoiceId))
                                   .ReturnsAsync(mockInvoice);

                // Act
                var result = await _invoicesController.Details(invoiceId) as ViewResult;

                // Assert
                Assert.NotNull(result);
                var model = Assert.IsType<Invoice>(result.Model);
                Assert.Equal(invoiceId, model.Id); // Verifica se o modelo retornado é a fatura correta
            }
        }
    
}
