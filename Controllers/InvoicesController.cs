using MasterInvoice.Interfaces;
using MasterInvoice.Models.invoice;
using MasterInvoice.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MasterInvoice.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly IInvoice _invoiceService;

        public InvoicesController(IInvoice invoiceService)
        {
            _invoiceService = invoiceService;
        }

        // GET: InvoicesController
        public async Task<ActionResult> Index(DateTime? filtroMesEmissao, DateTime? filtroMesCobranca, DateTime? filtroMesPagamento, string filtroStatusNota)
        {
            try
            {
                // Aplicar filtros se houver valores nos parâmetros
                
                var invoices = await _invoiceService.GetAllInvoices(filtroMesEmissao, filtroMesCobranca, filtroMesPagamento, filtroStatusNota);

                ViewBag.FiltroMesEmissao = filtroMesEmissao;
                ViewBag.FiltroMesCobranca = filtroMesCobranca;
                ViewBag.FiltroMesPagamento = filtroMesPagamento;
                ViewBag.FiltroStatusNota = filtroStatusNota;

                return View(invoices);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro ao carregar faturas: {ex.Message}";
                return View();
            }
        }

        // GET: InvoicesController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var invoice = await _invoiceService.GetInvoiceById(id);
                if (invoice == null)
                {
                    return NotFound();
                }
                return View(invoice);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro ao obter detalhes da fatura: {ex.Message}";
                return View();
            }
        }

        // GET: InvoicesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InvoicesController/Create
        [HttpPost]
        public async Task<ActionResult> Create(Invoices invoice)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _invoiceService.AddInvoice(invoice);
                    return RedirectToAction(nameof(Index));
                }
                return View(invoice);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro ao criar fatura: {ex.Message}";
                return View(invoice);
            }
        }

        // GET: InvoicesController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var invoice = await _invoiceService.GetInvoiceById(id);
                if (invoice == null)
                {
                    return NotFound();
                }
                return View(invoice);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro ao editar fatura: {ex.Message}";
                return View();
            }
        }

        // POST: InvoicesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Invoices invoice)
        {
            try
            {
                if (id != invoice.Id)
                {
                    return BadRequest();
                }

                if (ModelState.IsValid)
                {
                    await _invoiceService.UpdateInvoice(invoice);
                    return RedirectToAction(nameof(Index));
                }
                return View(invoice);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro ao editar fatura: {ex.Message}";
                return View(invoice);
            }
        }

        // GET: InvoicesController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var invoice = await _invoiceService.GetInvoiceById(id);
                if (invoice == null)
                {
                    return NotFound();
                }
                return View(invoice);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro ao obter fatura para exclusão: {ex.Message}";
                return View();
            }
        }

        // POST: InvoicesController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _invoiceService.DeleteInvoice(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro ao excluir fatura: {ex.Message}";
                return View();
            }
        }
    }
}
