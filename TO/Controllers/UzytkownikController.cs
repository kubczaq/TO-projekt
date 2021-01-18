using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TO.DbModels;
using TO.Models;
using TO.Services;

namespace TO.Controllers
{
    public class UzytkownikController : Controller
    {
        private readonly ILogger<UzytkownikController> _logger;
        private readonly UzytkownikService _uzytkownikService;

        public UzytkownikController(ILogger<UzytkownikController> logger, UzytkownikService uzytkownikService)
        {
            _logger = logger;
            _uzytkownikService = uzytkownikService;
        }
        public IActionResult Logowanie(string nazwa)
        {
            ListAllWorkersVM vm = new ListAllWorkersVM()
            {
                Uzytkownicy = _uzytkownikService.Loguj(nazwa)
            };
            if (nazwa == "Admin")
            {
                return RedirectToAction("Index_Admin");
            }
            else
            {
                if (vm.Uzytkownicy.Count > 0)
                {
                    if (nazwa == "Policja")
                    {
                        return RedirectToAction("Index_Policja");
                    }
                    else if (nazwa == "Urzednik")
                    {
                        return RedirectToAction("Index_Urzednik");
                    }
                    else if (nazwa == "Petent")
                    {
                        return RedirectToAction("Index_Petent");
                    }
                    else
                    {
                        return RedirectToAction("Index_Bad");
                    }
                }
                else
                {
                    return RedirectToAction("Index_Bad");
                }
            }
        }
        public IActionResult Index_Admin()
        {
            return View();
        }
        public IActionResult Index_Urzednik()
        {
            return View();
        }
        public IActionResult Index_Policja()
        {
            return View();
        }
        public IActionResult Index_Petent()
        {
            return View();
        }
        public IActionResult Index_Bad()
        {
            return View();
        }
        public IActionResult Index()
        {
            ListAllWorkersVM vm = new ListAllWorkersVM()
            {
                Uzytkownicy = _uzytkownikService.Get()
            };
            return View(vm);
        }
        public IActionResult Add()
        {
            UzytkownikVM model = new UzytkownikVM();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(UzytkownikVM vm)
        {
            if (ModelState.IsValid)
            {
                _uzytkownikService.Create(vm.ToUzytkownik());
                return RedirectToAction("Index");
            }
            return View(vm);
        }
        public IActionResult Delete(string id)
        {
            _uzytkownikService.Remove(id);
            return RedirectToAction("Index");
        }
        public IActionResult Edit(string id)
        {
            var uzytkownik = _uzytkownikService.Get(id);
            UzytkownikVM model = new UzytkownikVM(uzytkownik);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UzytkownikVM vm)
        {
            if (ModelState.IsValid)
            {
                _uzytkownikService.Update(vm.Id, vm.ToUzytkownik());
                return RedirectToAction("Index");
            }
            return RedirectToAction("Edit", vm);
        }
    }
    public class ListAllWorkersVM
    {
        public string TitleName { get; set; }
        public List<Uzytkownik> Uzytkownicy { get; set; }
    }
    public class UzytkownikVM
    {
        public UzytkownikVM()
        {

        }
        public UzytkownikVM(Uzytkownik uzytkownik)
        {
            Id = uzytkownik.Id;
            Rola = uzytkownik.Rola;
            Haslo = uzytkownik.Haslo;

        }
        public string Id { get; set; }

        public string Rola { get; set; }
        public string Haslo { get; set; }
        public Uzytkownik ToUzytkownik()
        {
            return new Uzytkownik()
            {
                Id = Id,
                Rola = Rola,
                Haslo = Haslo
            };
        }
    }
}
