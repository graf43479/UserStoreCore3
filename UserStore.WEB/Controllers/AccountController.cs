using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserStore.DAL.Entities;
using UserStore.WEB.Models;

namespace UserStore.WEB.Controllers
{

    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        //public AccountController(ILogger<AccountController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        //{
        //    _logger = logger;
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //}
        public AccountController(ILogger<AccountController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home"); //  View("Error", new string[] { "В доступе отказано" });
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            //Два способа подсчета неудачных попыток входа
            //1. Через встроенные средства Identity. Минус: работает только для зарегистрированного логина
            //2. Через сессию. Минус: сессия может сбрасываться роботом. 
            //Тем не менее совместим оба подхода. Можно будет ещё куки добавить       
            string key = "attempt";
            string keyCapthca = "Captcha";
            HttpContext.Session.SetInt32(key, (HttpContext.Session.GetInt32(key) == null ? 0 : Int32.Parse(HttpContext.Session.GetString(key)) + 1));
            //   Session["attempt"] = (Session["attempt"] == null) ? 0 : (int)Session["attempt"] + 1;

            //if ((Session["Captcha"] == null || Session["Captcha"].ToString() != model.Captcha) && (int)Session["attempt"] > 3)
            string captcha = HttpContext.Session.GetString(keyCapthca);

            //    if ((Session["Captcha"] == null || Session["Captcha"].ToString() != model.Captcha) && (int)Session["attempt"] > 3)
            if ((string.IsNullOrEmpty(captcha) || captcha != model.Captcha) && (int)HttpContext.Session.GetInt32(key) > 3)
            {
                ModelState.AddModelError("Captcha", "Сумма введена неверно! Пожалуйста, повторите ещё раз!");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                ClaimsIdentity claim = null;
                AppUser user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);

                    if (result.Succeeded)
                    {
                        HttpContext.Session.SetInt32(key, 0);
                        if (String.IsNullOrEmpty(returnUrl) || returnUrl.Contains(Url.Action("Login", "Account")))
                        {
                            return RedirectToAction("Index", "Home", null);
                        }
                        else
                        {
                            Redirect(returnUrl);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                    }
                }
                

                //    UserDTO userDto = new UserDTO { Email = model.Email, Password = model.Password };
                //    ClaimsIdentity claim = await service.AuthenticateAsync(userDto);

                //    ClaimsIdentity claim = await _userManager.

                //    if (claim == null)
                //    {
                //        Session["attempt"] = await service.CheckForAttemptsAsync(model.Email);
                //        ModelState.AddModelError("", "Неверный логин или пароль.");

                //    }
                //    else
                //    {
                //        OperationDetails isConfirmed = await service.IsEmailConfirmedAsync(userDto);
                //        if (isConfirmed.Succedeed)
                //        {
                //            AuthenticationManager.SignOut();
                //            AuthenticationManager.SignIn(new AuthenticationProperties
                //            {
                //                IsPersistent = true
                //            }, claim);

                //            HttpContext.Session.SetInt32(key, 0);                        
                //            if (String.IsNullOrEmpty(returnUrl) || returnUrl.Contains(Url.Action("Login", "Account")))
                //            {
                //                return RedirectToAction("Index", "Home", null);
                //            }
                //            else
                //            {
                //                Redirect(returnUrl);
                //            }
                //        }
                //        else
                //        {
                //            AddErrorsFromResult(isConfirmed);
                //        }
                //    }
                //}
                //return View(model);
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                //AppUser user = new AppUser { Email = model.Email, UserName = model.Email, ClientProfile = new ClientProfile { Adress = "Titova5", Name = "Oleg" } };
                AppUser user = new AppUser { Email = model.Email, UserName = model.Email, ClientProfile = new ClientProfile() { Adress = model.Address, Name=model.Name} };
                // добавляем пользователя
                //ClientProfile profile 

               

                //var result = await _userManager.CreateAsync(user, model.Password);
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                   // await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}