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

        public async Task<IActionResult> Index()
        {
            var openedRents = await context.Rents.Where(a => a.ReturnTime == null)
                                        .Select(a => new RentViewModel
                                        {
                                            Id = a.Id,
                                            DvdName = a.Dvd.Name,
                                            DvdId = a.DvdId,
                                            MemberName = a.Member.Name,
                                            MemberId = a.MemberId,
                                            RentTime = a.RentTime
                                        }).ToListAsync();

            foreach (var openedRent in openedRents)
            {
                var time = DateTime.Now - openedRent.RentTime;
                openedRent.DaysRented = time.Days;
            }

            var topMembers = await context.Invoices
                                    .GroupBy(a => new { MemberId = a.MemberId, Member = a.MemberName })
                                    .OrderByDescending(a => a.Sum(b => b.InvoiceItems.Sum(c => c.Price))).Take(5)
                                    .Select(a => new DashboardMemberModel { MemberId = a.Key.MemberId, MemberName = a.Key.Member, Sum = a.Sum(b => b.InvoiceItems.Sum(c => c.Price)) })
                                    .ToListAsync();

            var topDvds = await context.Rents.GroupBy(a => new { DvdId = a.DvdId, DvdName = a.Dvd.Name })
                                        .OrderByDescending(a => a.Count()).Take(5)
                                        .Select(a => new DashboardDvdModel { DvdId = a.Key.DvdId, DvdName = a.Key.DvdName, TimesRented = a.Count() })
                                        .ToListAsync();

            var dashboardModel = new DashboardModel
            {
                OpenedRents = openedRents.OrderByDescending(a => a.DaysRented).ToList(),
                TopDvds = topDvds,
                TopMembers = topMembers
            };

            return View(dashboardModel);
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
