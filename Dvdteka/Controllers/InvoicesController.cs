using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dvdteka.Data;
using Dvdteka.Models;

namespace Dvdteka.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly DvdtekaContext _context;

        public InvoicesController(DvdtekaContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index(string memberName)
        {
            ViewData["memberName"] = memberName;

            var invoices = _context.Invoices.AsQueryable();
            
            if(!string.IsNullOrEmpty(memberName))
            {
                invoices = invoices.Where(a => a.MemberName.ToUpper().Contains(memberName.ToUpper()));
            }

            var list = await invoices.Select(a => new InvoiceViewModel
            {
                Id = a.Id,
                MemberId = a.MemberId,
                MemberName = a.MemberName,
                ReturnTime = a.ReturnTime,
                PriceSum = a.InvoiceItems.Sum(b => b.Price),
                InvoiceItems = a.InvoiceItems.Select(i => new InvoiceItemViewModel
                {
                    Id = i.Id,
                    InvoiceId = i.InvoiceId,
                    DvdName = i.DvdName,
                    Price = i.Price,
                    RentTime = i.RentTime,
                    ReturnTime = i.ReturnTime
                }).ToList()
            }).ToListAsync();

            return View(list);
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.Include(a => a.InvoiceItems).FirstOrDefaultAsync(a => a.Id == id);

            var model = new ReturnRecapitulationViewModel
            {
                Invoice = invoice,
                Sum = invoice.InvoiceItems.Sum(a => a.Price)
            };

            return View(model);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }
    }
}
