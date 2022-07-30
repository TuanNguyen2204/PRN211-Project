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
    public class SignupController : Controller
    {
        private readonly CenimaDBContext _context;

        public SignupController(CenimaDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Person person)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Persons.FirstOrDefault(p => p.Email.Equals(person.Email));
                if (user == null)
                {
                    person.IsActive = true;
                    person.Type = 2;
                    _context.Persons.Add(person);
                    _context.SaveChanges();
                    //HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));
                    return Redirect("../Login");
                }
                else
                {
                    ViewBag.error = "Email này đã tồn tại. Vui lòng chọn email khác.";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
    }
}
