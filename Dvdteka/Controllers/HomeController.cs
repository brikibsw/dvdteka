using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dvdteka.Models;
using Dvdteka.Data;
using Microsoft.EntityFrameworkCore;

namespace Dvdteka.Controllers
{
    public class HomeController : Controller
    {
        private readonly DvdtekaContext context;

        public HomeController(DvdtekaContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Seed()
        {
            for (int i = 1; i < 11; i++)
            {
                var genre = new Genre
                {
                    Name = "Žanr " + i
                };

                await context.Genres.AddAsync(genre);
            }

            await context.SaveChangesAsync();

            for (int i = 1; i < 51; i++)
            {
                var director = new Director
                {
                    Name = "Neki" + i + " Režiser" + i
                };

                await context.Directors.AddAsync(director);
            }

            await context.SaveChangesAsync();

            var genres = await context.Genres.ToListAsync();
            var directors = await context.Directors.ToListAsync();

            var rnd = new Random();

            for (int i = 1; i < 201; i++)
            {
                var dvd = new Dvd
                {
                    Name = "Dvd " + i,
                    Quantity = 5,
                    AvailableQuantity = 5,
                    Price = rnd.Next(16, 49),
                    Year = i%2 == 0 ? DateTime.Now.AddYears(-1).Year : DateTime.Now.AddYears(-2).Year,
                    GenreId = genres[rnd.Next(0, genres.Count - 1)].Id,
                    DirectorId = directors[rnd.Next(0, directors.Count - 1)].Id
                };

                await context.Dvds.AddAsync(dvd);
            }

            await context.SaveChangesAsync();

            return Ok("Seed uspio");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
