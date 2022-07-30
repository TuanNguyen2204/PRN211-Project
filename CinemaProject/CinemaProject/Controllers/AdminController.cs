using CinemaProject.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CinemaProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly CenimaDBContext _context;

        public AdminController()
        {
            _context = new CenimaDBContext();
        }

        public IActionResult User()
        {
            var users = _context.Persons.ToList();
            return View(users);
        }

        [HttpPost]
        public IActionResult User(bool status, string email)
        {
            var users = _context.Persons.ToList();
            try
            {
                Person person = _context.Persons.FirstOrDefault(x => x.Email.Equals(email));
                person.IsActive = status;
                _context.SaveChanges();
                ViewData["Message"] = "Cập nhật nhật trạng thái thành công!";
            }
            catch (Exception)
            {
                ViewData["Message"] = "Cập nhật nhật trạng thái không thành công!";
            }
            return View(users);

        }

        [HttpGet]
        public IActionResult Genres()
        {
            var genres = _context.Genres.ToList();

            return View(genres);
        }

        [HttpPost]
        public IActionResult Genres(string des, int gid)
        {
            var genres = _context.Genres.ToList();

            try
            {
                Genre genre = _context.Genres.FirstOrDefault(x => x.GenreId == gid);
                genre.Description = des;
                _context.SaveChanges();
                ViewData["Message"] = "Cập nhật nhật thành công!";
            }
            catch (Exception)
            {
                ViewData["Message"] = "Cập nhật nhật không thành công!";
            }



            return View(genres);
        }

        [HttpGet]
        public IActionResult AddGenre()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddGenre(Genre genre)
        {
            _context.Genres.Add(genre);
            _context.SaveChanges();
            return RedirectToAction("Genres");
        }

        [HttpGet]
        public IActionResult Movies()
        {
            var movies = _context.Movies.ToList();
            return View(movies);
        }

        [HttpGet]
        public IActionResult MovieDetail(int id)
        {
            var movie = _context.Movies.FirstOrDefault(x => x.MovieId == id);
            var genres = _context.Genres.ToList();

            ViewData["genre"] = genres;
            return View(movie);
        }

        [HttpPost]
        public IActionResult MovieDetail(Movie moviex)
        {
            try
            {
                Movie v = _context.Movies.FirstOrDefault(x => x.MovieId == moviex.MovieId);
                v.Title = moviex.Title;
                v.Description = moviex.Description;
                v.Year = moviex.Year;
                v.GenreId = moviex.GenreId;
                v.Image = moviex.Image;
                _context.SaveChanges();
                ViewData["Message"] = "Cập nhật thành công!";

            }
            catch (Exception)
            {
                ViewData["Message"] = "Cập nhật không thành công!";

            }
            var movie = _context.Movies.FirstOrDefault(x => x.MovieId == moviex.MovieId);
            var genres = _context.Genres.ToList();
            ViewData["genre"] = genres;

            return View(movie);
        }

        [HttpGet]
        public IActionResult NewMovie()
        {
            var genres = _context.Genres.ToList();
            ViewData["genre"] = genres;
            return View();
        }

        [HttpPost]
        public IActionResult NewMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();
            return RedirectToAction("Movies");
        }
    }
}

