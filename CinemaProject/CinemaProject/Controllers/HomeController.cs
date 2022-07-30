using CinemaProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CenimaDBContext _context;

        public HomeController(ILogger<HomeController> logger, CenimaDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        //public IActionResult Index()
        //{
        //    var list = _context.Movies.Join(_context.Genres, m => m.GenreId, g => g.GenreId, (m, g) => new
        //    {
        //        MovieId = m.MovieId,
        //        Title = m.Title,
        //        Year = m.Year,
        //        Image = m.Image,
        //        Des = g.Description,
        //    }).Join(_context.Rates, x => x.MovieId, r => r.MovieId, (x, r) => new { x, r }).
        //    GroupBy(t => new
        //    {
        //        t.x.MovieId,
        //        t.x.Title,
        //        t.x.Year,
        //        t.x.Image,
        //        t.x.Des
        //    }).Select(z => new
        //    {
        //        MovieId = z.Key.MovieId,
        //        Title = z.Key.Title,
        //        Year = z.Key.Year,
        //        Img = z.Key.Image,
        //        Des = z.Key.Des,
        //        RatingAverage = z.Average( k => k.r.NumericRating)
        //    }).ToList();
        //    ViewData["listGenre"] = _context.Genres.ToList();
        //    return Json(list);
        //}
        //public IActionResult Index()
        //{

        //    return View();
        //}
        public IActionResult Index(string search)
        {
            dynamic listMovie;
            var list1 = _context.Movies.Join(_context.Genres, m => m.GenreId, g => g.GenreId, (m, g) => new
            {
                MovieId = m.MovieId,
                Title = m.Title,
                Year = m.Year,
                Image = m.Image,
                Des = g.Description
            }).ToList();
            var list2 = list1.Select(x => new
            {
                MovieId = x.MovieId,
                Title = x.Title,
                Year = x.Year,
                Image = x.Image,
                Des = x.Des,
                Average = getAverageRating(x.MovieId)
            }).ToList();
            ViewData["listGenre"] = _context.Genres.ToList();

            if(search == null)
            {
                listMovie = list2;
            }
            else
            {
                listMovie = list2.Where(x => x.Title.Contains(search));
            }
            ViewData["search"] = search;
            return View(listMovie);
        }
        public double? getAverageRating(int movieId) 
        {
             double? a = _context.Rates.Where(c => c.MovieId == movieId).Average(r => r.NumericRating);
            return a;
            
        }

        public IActionResult Genres(int genreId)
        {
            var list1 = _context.Movies.Join(_context.Genres, m => m.GenreId, g => g.GenreId, (m, g) => new
            {
                MovieId = m.MovieId,
                GenreId = m.GenreId,
                Title = m.Title,
                Year = m.Year,
                Image = m.Image,
                Des = g.Description
            }).Where(a => a.GenreId == genreId).ToList();
            dynamic list2 = list1.Select(x => new
            {
                MovieId = x.MovieId,
                Title = x.Title,
                Year = x.Year,
                Image = x.Image,
                Des = x.Des,
                Average = getAverageRating(x.MovieId)
            }).ToList();
            ViewData["GenreId"] = genreId;
            ViewData["listGenre"] = _context.Genres.ToList();
            return View("Index", list2);
        }
        public ActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            HttpContext.Session.Clear();
            return Redirect("/Home");
        }

    }


}
