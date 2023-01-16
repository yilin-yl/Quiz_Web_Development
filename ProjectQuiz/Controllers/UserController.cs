using System;
using Microsoft.AspNetCore.Mvc;
using ProjectQuiz.Models;
using ProjectQuiz.DAO;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

//action: login, register

namespace ProjectQuiz.Controllers
{

	public class UserController: Controller
	{
        private readonly UserDAO _userdao;

        public UserController(UserDAO userdao)
        {
            _userdao = userdao;
        }

        //Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _userdao.GetUser(username);

            if (user != null && password == user.password)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString())

            };
                var identity = new ClaimsIdentity(claims, "Login");
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("Login", principal);
                return Redirect("/Home/Index");
            } else
            {
                return View();
            }
        }

        //Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserInfo user)
        {
            _userdao.AddUser(user);
            return RedirectToAction("Login"); 
        }

        //Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Login");
            return Redirect("/User/Login");
        }
    }
}

