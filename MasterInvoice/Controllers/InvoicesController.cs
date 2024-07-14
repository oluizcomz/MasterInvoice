using Domain.Interfaces;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace MasterInvoice.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly IInvoice iInvoice;
        public InvoicesController(IInvoice iExpense)
        {
            this.iInvoice = iExpense;
        }

        // GET: InvoicesController
        public async Task<ActionResult> Index(DateTime? filtroMesEmissao, DateTime? filtroMesCobranca, DateTime? filtroMesPagamento, string filtroStatusNota)
        {
            try
            {
                ViewBag.FiltroMesEmissao = filtroMesEmissao;
                ViewBag.FiltroMesCobranca = filtroMesCobranca;
                ViewBag.FiltroMesPagamento = filtroMesPagamento;
                ViewBag.FiltroStatusNota = filtroStatusNota;
                var invoices = await iInvoice.GetFiltered(filtroMesEmissao, filtroMesCobranca, filtroMesPagamento, filtroStatusNota);

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
                var invoice = await iInvoice.GetByID(id);
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

      
        // POST: InvoicesController/Create
        [HttpPost]
        public async Task<ActionResult> Create(Invoice invoice)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await iInvoice.Insert(invoice);
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
                var invoice = await iInvoice.GetByID(id);
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
        public async Task<ActionResult> Edit(int id, Invoice invoice)
        {
            try
            {
                if (id != invoice.Id)
                {
                    return BadRequest();
                }

                if (ModelState.IsValid)
                {
                    await iInvoice.Update(invoice);
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
                var invoice = await iInvoice.GetByID(id);
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
                var invoice = await iInvoice.GetByID(id);
                await iInvoice.Delete(invoice);
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
