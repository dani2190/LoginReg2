using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoginReg2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LoginReg2.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context {get; set;}
        private PasswordHasher<User> regHasher = new PasswordHasher<User>();
        private PasswordHasher<LoginUser> logHasher = new PasswordHasher<LoginUser> ();
        public  User GetUser()
        {
            return _context.Users.FirstOrDefault( u =>  u.UserId == HttpContext.Session.GetInt32("userId"));
        }
        public HomeController (MyContext context)
        {
            _context = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.FirstOrDefault (usr => usr.Email == user.Email) != null)
                {
                    ModelState.AddModelError("Email", "Email is already use !");
                    return View ("Index");
                }
                string hash = regHasher.HashPassword(user, user.Password);
                user.Password = hash;
                _context.Users.Add(user);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", user.UserId);
                return Redirect ("/Success");
            }
            return View("Index");
        }
        [HttpPost("Login")]
        public IActionResult Login(LoginUser login)
        {
            if (ModelState.IsValid)
            {
                User userInDB = _context.Users.FirstOrDefault(u => u.Email == login.LoginEmail);
                if (userInDB == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email or Password");
                    return View( "Index");
                }
                var result = logHasher.VerifyHashedPassword(login, userInDB.Password, login.LoginPassword);
                if(result == 0)
                {
                    ModelState.AddModelError("Password","Invalid Email or Password!");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("userId", userInDB.UserId);
                return Redirect("/Success");
            }
            return View ("Index");
        }
        [HttpGet("Success")]
        public IActionResult Success()
        {
            User current = GetUser();
            if (current == null)
            {
                return Redirect ("Login");
            }
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    
}
}
