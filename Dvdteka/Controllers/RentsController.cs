﻿using System;
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
    public class RentsController : Controller
    {
        private readonly DvdtekaContext _context;

        public RentsController(DvdtekaContext context)
        {
            _context = context;
        }

        // GET: Rents
        public async Task<IActionResult> Index(string memberName, string dvdName)
        {
            ViewData["memberName"] = memberName;
            ViewData["dvdName"] = dvdName;
            var rents = _context.Rents.Include(r => r.Dvd).Include(r => r.Member).AsQueryable();

            if( !string.IsNullOrEmpty(memberName))
            {
                rents = rents.Where(a => a.Member.Name.ToUpper().Contains(memberName.ToUpper()));
            }

            if (!string.IsNullOrEmpty(dvdName))
            {
                rents = rents.Where(a => a.Dvd.Name.ToUpper().Contains(dvdName.ToUpper()));
            }

            rents = rents.Where(a => a.ReturnTime == null);

            var rentViewModels = rents.Select(a => new RentViewModel
            {
                Id = a.Id,
                DvdName = a.Dvd.Name,
                MemberName = a.Member.Name,
                MemberId = a.MemberId,
                RentTime = a.RentTime,
                ReturnTime = a.ReturnTime,
                Price = a.Price,
                Returning = false
            });

            return View(await rentViewModels.ToListAsync());
        }

        public async Task<IActionResult> ClosedRents(string memberName, string dvdName, string sortBy, string sort = "asc", int page = 1, int pageSize = 10)
        {
            ViewData["memberName"] = memberName;
            ViewData["dvdName"] = dvdName;

            ViewData["page"] = page;
            ViewData["pageSize"] = pageSize;

            ViewData["sortBy"] = sortBy;
            ViewData["sort"] = sort;

            var rents = _context.Rents.Include(r => r.Dvd).Include(r => r.Member).AsQueryable();

            if (!string.IsNullOrEmpty(memberName))
            {
                rents = rents.Where(a => a.Member.Name.ToUpper().Contains(memberName.ToUpper()));
            }

            if (!string.IsNullOrEmpty(dvdName))
            {
                rents = rents.Where(a => a.Dvd.Name.ToUpper().Contains(dvdName.ToUpper()));
            }

            rents = rents.Where(a => a.ReturnTime != null);

            var dataCount = await rents.CountAsync();

            ViewData["pages"] = (int)Math.Ceiling(decimal.Divide(dataCount, pageSize));

            var skip = page == 1 ? 0 : pageSize * (page - 1);

            switch (sortBy)
            {
                case "MemberName":
                    rents = sort == "asc" ? rents.OrderBy(a => a.Member.Name) : rents.OrderByDescending(a => a.Member.Name);
                    break;
                case "DvdName":
                    rents = sort == "asc" ? rents.OrderBy(a => a.Dvd.Name) : rents.OrderByDescending(a => a.Dvd.Name);
                    break;
                case "RentTime":
                    rents = sort == "asc" ? rents.OrderBy(a => a.RentTime) : rents.OrderByDescending(a => a.RentTime);
                    break;
                case "ReturnTime":
                    rents = sort == "asc" ? rents.OrderBy(a => a.ReturnTime) : rents.OrderByDescending(a => a.ReturnTime);
                    break;
                case "Price":
                    rents = sort == "asc" ? rents.OrderBy(a => a.Price) : rents.OrderByDescending(a => a.Price);
                    break;
                default:
                    rents = sort == "asc" ? rents.OrderBy(a => a.Id) : rents.OrderByDescending(a => a.Id);
                    break;
            }

            rents = rents.Skip(skip).Take(pageSize);

            var rentViewModels = rents.Select(a => new RentViewModel
            {
                Id = a.Id,
                DvdName = a.Dvd.Name,
                MemberName = a.Member.Name,
                MemberId = a.MemberId,
                RentTime = a.RentTime,
                ReturnTime = a.ReturnTime,
                Price = a.Price,
                Returning = false
            });

            return View(await rentViewModels.ToListAsync());
        }

        // GET: Rents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rents
                .Include(r => r.Dvd)
                .Include(r => r.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }

        // GET: Rents/Create
        public async Task<IActionResult> Create(int? id)
        {
            var dvd = await _context.Dvds.FindAsync(id);
            var rent = new Rent
            {
                Dvd = dvd,
                DvdId = (int)id,
                RentTime = DateTime.Now
            };
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Name");
            return View(rent);
        }

        public IActionResult ReturnMany(IEnumerable<RentViewModel> rents)
        {
            ViewData["error"] = false;
            var group = rents.Where(a => a.Returning).GroupBy(a => a.MemberName);

            if (group.Count() > 1)
            {
                ModelState.AddModelError("MemberId", "Povrat može biti samo za istog člana!");
                ViewData["error"] = true;
            }

            var returnManyViewModel = new ReturnManyViewModel
            {
                ReturnTime = DateTime.Now,
                Rents = rents.Where(a => a.Returning).ToList()
            };

            return View(returnManyViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveReturnMany(ReturnManyViewModel returnManyViewModel)
        {
            var ids = returnManyViewModel.Rents.Where(a => a.Returning).Select(a => a.Id);
            var rents = await _context.Rents.Include(a => a.Dvd).Include(a => a.Member).Where(a => ids.Contains(a.Id)).ToListAsync();

            foreach (var rent in rents)
            {
                rent.ReturnTime = returnManyViewModel.ReturnTime;
                var time = rent.ReturnTime - rent.RentTime;
                rent.Price = (decimal)(time.Value.Days * (double)rent.Dvd.Price);

                rent.Dvd.AvailableQuantity = rent.Dvd.AvailableQuantity + 1;
                _context.Dvds.Update(rent.Dvd);

                _context.Update(rent);
            }

            await _context.SaveChangesAsync();

            var invoice = new Invoice
            {
                ReturnTime = returnManyViewModel.ReturnTime,
                MemberId = rents.FirstOrDefault().MemberId,
                MemberName = rents.FirstOrDefault().Member.Name,
                InvoiceItems = rents.Select(a => new InvoiceItem
                {
                    DvdId = a.DvdId,
                    DvdName = a.Dvd.Name,
                    RentTime = a.RentTime,
                    ReturnTime = returnManyViewModel.ReturnTime,
                    Price = (decimal)a.Price
                }).ToList()
            };

            _context.Invoices.Add(invoice);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Invoices", new { id = invoice.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DvdId,MemberId,RentTime,ReturnTime,Price")] Rent rent)
        {
            if (ModelState.IsValid)
            {
                var dvd = await _context.Dvds.FindAsync(rent.DvdId);
                dvd.AvailableQuantity = dvd.AvailableQuantity - 1;
                _context.Dvds.Update(dvd);

                _context.Add(rent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Name", rent.MemberId);
            return View(rent);
        }

        // GET: Rents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rents
                                        .Include(a => a.Dvd)
                                        .Include(a => a.Member)
                                        .FirstOrDefaultAsync( a => a.Id == id);
            if (rent == null)
            {
                return NotFound();
            }

            rent.ReturnTime = DateTime.Now;

            return View(rent);
        }

        // POST: Rents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DvdId,MemberId,RentTime,ReturnTime,Price")] Rent rent)
        {
            if (id != rent.Id)
            {
                return NotFound();
            }

            rent.Dvd = await _context.Dvds.FindAsync(rent.DvdId);

            var time = rent.ReturnTime - rent.RentTime;
            rent.Price = (decimal)(time.Value.Days * (double)rent.Dvd.Price);

            if (ModelState.IsValid)
            {
                try
                {
                    rent.Dvd.AvailableQuantity = rent.Dvd.AvailableQuantity + 1;
                    _context.Dvds.Update(rent.Dvd);

                    _context.Update(rent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentExists(rent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = rent.Id });
            }
            return View(rent);
        }

        // GET: Rents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rents
                .Include(r => r.Dvd)
                .Include(r => r.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }

        // POST: Rents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rent = await _context.Rents.FindAsync(id);
            _context.Rents.Remove(rent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentExists(int id)
        {
            return _context.Rents.Any(e => e.Id == id);
        }
    }
}
