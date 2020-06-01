using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DomaciZadatak.Models.Interfaces;
using DomaciZadatak.Models.EFRepository;
using DomaciZadatak.Models;
using DomaciZadatak.Models.LinqSql;

namespace DomaciZadatak.Controllers
{
    public class AccountController : Controller
    {
        private IAuthRepository authRepository = new AuthRepository();
        KorisniciDataContext kDC = new KorisniciDataContext();
        // GET: Korisnik
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(KorisnikBO user)
        {
            if (authRepository.IsValid(user))
            {
                FormsAuthentication.SetAuthCookie(user.Email, false);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("CustomError", "Uneti podaci nisu validni.");
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(KorisnikBO user)
        {
            if (kDC.Korisniks.Any(t => t.JMBG == user.JMBG))
            {
                ViewBag.NeuspesnaRegistracija = "Postoji vec takav korisnik.";
                return View("Register", user);
            }
            else
            {
                authRepository.AddUser(user);
                ModelState.Clear();
                ViewBag.UspesnaRegistracija = "Uspesna registracija.";
            }
            return View("Register", new KorisnikBO());
        }
    }
}