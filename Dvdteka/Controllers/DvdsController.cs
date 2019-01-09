using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dvdteka.Data;

namespace Dvdteka.Controllers
{
    public class DvdsController : Controller
    {
        private readonly DvdtekaContext _context;

        public DvdsController(DvdtekaContext context)
        {
            _context = context;
        }

        // GET: Dvds
        public async Task<IActionResult> Index(string searchByName, string searchByDirector, string searchByGenre)
        {
            ViewData["searchByName"] = searchByName;
            ViewData["searchByDirector"] = searchByDirector;
            ViewData["searchByGenre"] = searchByGenre;

            var dvdtekaContext = _context.Dvds.Include(d => d.Director).Include(d => d.Genre).AsQueryable();

            if (!string.IsNullOrEmpty(searchByName))
            {
                dvdtekaContext = dvdtekaContext.Where(a => a.Name.ToUpper().Contains(searchByName.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchByDirector))
            {
                dvdtekaContext = dvdtekaContext.Where(a => a.Director.Name.ToUpper().Contains(searchByDirector.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchByGenre))
            {
                dvdtekaContext = dvdtekaContext.Where(a => a.Genre.Name.ToUpper().Contains(searchByGenre.ToUpper()));
            }

            return View(await dvdtekaContext.ToListAsync());
        }

        // GET: Dvds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dvd = await _context.Dvds
                .Include(d => d.Director)
                .Include(d => d.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dvd == null)
            {
                return NotFound();
            }

            return View(dvd);
        }

        // GET: Dvds/Create
        public IActionResult Create()
        {
            ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name");
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            return View();
        }

        // POST: Dvds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,Year,Quantity,DirectorId,GenreId")] Dvd dvd)
        {
            if (ModelState.IsValid)
            {
                dvd.AvailableQuantity = dvd.Quantity;
                _context.Add(dvd);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name", dvd.DirectorId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", dvd.GenreId);
            return View(dvd);
        }

        // GET: Dvds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dvd = await _context.Dvds.FindAsync(id);
            if (dvd == null)
            {
                return NotFound();
            }
            ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name", dvd.DirectorId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", dvd.GenreId);
            return View(dvd);
        }

        // POST: Dvds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Year,Quantity,AvailableQuantity,DirectorId,GenreId")] Dvd dvd)
        {
            if (id != dvd.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dvd);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DvdExists(dvd.Id))
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
            ViewData["DirectorId"] = new SelectList(_context.Directors, "Id", "Name", dvd.DirectorId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", dvd.GenreId);
            return View(dvd);
        }

        // GET: Dvds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dvd = await _context.Dvds
                .Include(d => d.Director)
                .Include(d => d.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dvd == null)
            {
                return NotFound();
            }

            return View(dvd);
        }

        // POST: Dvds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dvd = await _context.Dvds.FindAsync(id);
            _context.Dvds.Remove(dvd);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DvdExists(int id)
        {
            return _context.Dvds.Any(e => e.Id == id);
        }
    }
}
