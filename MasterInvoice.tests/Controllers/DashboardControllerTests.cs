using Moq;
using Xunit;
using Domain.Interfaces;
using MasterInvoice.Controllers;
using Microsoft.AspNetCore.Mvc;
using Entities;

namespace MasterInvoice.Tests.Controllers
{
    public class DashboardControllerTests
    {
        private readonly DashboardController _dashboardController;
        private readonly Mock<IInvoice> _mockInvoiceService;

        public DashboardControllerTests()
        {
            // Inicializar o Mock do serviço IInvoice
            _mockInvoiceService = new Mock<IInvoice>();

            // Inicializar o controlador com o Mock
            _dashboardController = new DashboardController(_mockInvoiceService.Object);
        }

        [Fact]
        public async Task GetDashboard_ReturnsCorrectData()
        {
            // Arrange
            DateTime dateInit = new DateTime(2023, 1, 1);
            DateTime finalDate = new DateTime(2023, 12, 31);
            var mockgraficModelView = new List<GraficModelView>{
                    new GraficModelView
                    {
                        Value = 0.0M,
                        Month = "01/2023",
                    },
            };

            var mockDashBoardModelView = new DashBoardModelView{
                    Issued = "0.0",
                    NoCharge = "0.0",
                    LatePayment = "0.0",
                    DuePayment = "0.0",
                    PaymentMade = "0.0",
                    ValuesDelinquency = mockgraficModelView,
                    ValuesPastDue = mockgraficModelView
            };

            // Configurar o Mock para retornar os dados mockados
            _mockInvoiceService
                .Setup(service => service.GetDashBoard(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(mockDashBoardModelView);

            // Act
            var result = await _dashboardController.Index(dateInit, finalDate);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<DashBoardModelView>(viewResult.Model);
            Assert.NotEmpty(model.Issued);
            Assert.NotEmpty(model.NoCharge);
            // Adicione mais assertivas conforme necessário para validar os dados
        }
        [Fact]
        public async Task GetDashboard_ReturnsNoData()
        {
            // Arrange
            DateTime dateInit = new DateTime(2023, 1, 1);
            DateTime finalDate = new DateTime(2023, 12, 31);

            // Act
            var result = await _dashboardController.Index(dateInit, finalDate) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Error", result.ViewName); // Verifique se está retornando a view de erro
            Assert.Equal("Error retrieving dashboard data: No data found", result.ViewData["ErrorMessage"]); // Verifique se a mensagem de erro está correta

        }
    }
}
