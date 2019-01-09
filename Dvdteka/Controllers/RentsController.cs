﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dvdteka.Data;

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
        public async Task<IActionResult> Index()
        {
            var dvdtekaContext = _context.Rents.Include(r => r.Dvd).Include(r => r.Member);
            return View(await dvdtekaContext.ToListAsync());
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

        // POST: Rents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                return RedirectToAction(nameof(Index));
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
