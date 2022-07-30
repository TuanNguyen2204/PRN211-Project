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
    public class LoginController : Controller
    {
        private readonly CenimaDBContext _context;

        public LoginController(CenimaDBContext context)
        {
            _context = context;
        }
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
                Person user = GetAccount(person.Email, person.Password);
                if(user != null)
                {
                    if (user.IsActive == true)
                    {
                        HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));
                        return Redirect("/Home");
                    }
                    else
                    {
                        ViewBag.error = "Bạn không có quyền truy cập!";
                        return View();

                    }

                }
                else
                {
                    ViewBag.error = "Email hoặc mật khẩu không đúng";
                    return View();
                }
            }
            return View();
        }
        public Person GetAccount(string Email, string Pass)
        {
            Person user = _context.Persons.FirstOrDefault(a => a.Email.Equals(Email) && a.Password.Equals(Pass));
            return user;
        }
    }
}
