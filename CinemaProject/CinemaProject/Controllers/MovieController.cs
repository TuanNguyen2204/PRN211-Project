using CinemaProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaProject.Controllers
{
    public class MovieController : Controller
    {
        private readonly CenimaDBContext _context;

        public MovieController(CenimaDBContext context)
        {
            _context = context;
        }
        public IActionResult Detail(int id)
        {
            Movie movie = _context.Movies.FirstOrDefault(m => m.MovieId == id);
            var list1 = _context.Movies.Where(m=> m.MovieId == id).Join(_context.Genres, m => m.GenreId, g => g.GenreId, (m, g) => new
            {
                MovieId = id,
                Title = m.Title,
                Image = m.Image,
                MovieDes = m.Description,
                Des = g.Description
            });
            dynamic list2 = list1.Select(x => new
            {
                MovieId = x.MovieId,
                Title = x.Title,
                Image = x.Image,
                MovieDes = x.MovieDes,
                Des = x.Des,
                Average = getAverageRating(id)
            }).FirstOrDefault();
            var list3 = _context.Rates.Where(r => r.MovieId == id).Join(_context.Persons, r => r.PersonId, p => p.PersonId, (r, p) => new {
                FullName = p.Fullname,
                Comment = r.Comment
            }).ToList();

            string json = HttpContext.Session.GetString("User");
            Person person = new Person();
            if (!string.IsNullOrEmpty(json))
            {
                person = JsonConvert.DeserializeObject<Person>(json);
            }

            var check = _context.Rates.FirstOrDefault(r => r.MovieId == id && r.PersonId==person.PersonId) ;

            ViewData["check"] = check;
            ViewData["comment"] = list3;
            ViewData["movieDetail"] = list2;
            return View(list2);
        }
        public double? getAverageRating(int movieId)
        {
            double? a = _context.Rates.Where(c => c.MovieId == movieId).Average(r => r.NumericRating);
            return a;

        }
        [HttpPost]
        public IActionResult AddNewCmt(double rating, string cmt)
        {
            string json = HttpContext.Session.GetString("User");
            Person person = new Person();
            
            if (!string.IsNullOrEmpty(json))
            {
                person = JsonConvert.DeserializeObject <Person>(json);
            }
            Rate rate = new Rate();
            rate.NumericRating = rating;
            rate.Comment = cmt;
            rate.PersonId = person.PersonId;
            _context.Rates.Add(rate);
            return View();
        }
    }
}
