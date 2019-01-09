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
    public class MemberContactsController : Controller
    {
        private readonly DvdtekaContext _context;

        public MemberContactsController(DvdtekaContext context)
        {
            _context = context;
        }

        // GET: MemberContacts/Create
        public IActionResult Create(int id)
        {
            var member = _context.Members.Find(id);
            var contact = new MemberContact { Member = member, MemberId = member.Id };
            ViewData["InfoTypes"] = ContactInfos();
            return View(contact);
        }

        // POST: MemberContacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Type,Value,MemberId")] MemberContact memberContact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(memberContact);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Members");
            }
            ViewData["InfoTypes"] = ContactInfos(memberContact.Type);
            return View(memberContact);
        }

        // GET: MemberContacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberContact = await _context.MemberContacts.Include(a => a.Member).FirstOrDefaultAsync(a => a.Id == id);
            if (memberContact == null)
            {
                return NotFound();
            }
            ViewData["InfoTypes"] = ContactInfos(memberContact.Type);
            return View(memberContact);
        }

        // POST: MemberContacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Value,MemberId")] MemberContact memberContact)
        {
            if (id != memberContact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(memberContact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberContactExists(memberContact.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Members");
            }
            ViewData["InfoTypes"] = ContactInfos(memberContact.Type);
            return View(memberContact);
        }

        // GET: MemberContacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberContact = await _context.MemberContacts
                .Include(m => m.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (memberContact == null)
            {
                return NotFound();
            }

            return View(memberContact);
        }

        // POST: MemberContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var memberContact = await _context.MemberContacts.FindAsync(id);
            _context.MemberContacts.Remove(memberContact);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Members");
        }

        private bool MemberContactExists(int id)
        {
            return _context.MemberContacts.Any(e => e.Id == id);
        }

        private SelectList ContactInfos(string selected = null)
        {
            var list = new List<InfoType>
            {
                new InfoType { Name = "Telefon" },
                new InfoType { Name = "Mobitel" },
                new InfoType { Name = "Email"}
            };

            if (string.IsNullOrEmpty(selected))
            {
                return new SelectList(list, "Name", "Name");
            }

            return new SelectList(list, "Name", "Name", selected);
        }
    }
}
