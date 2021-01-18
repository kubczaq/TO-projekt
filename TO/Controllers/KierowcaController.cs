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
    public class KierowcaController : Controller
    {
        private readonly ILogger<KierowcaController> _logger;
        private readonly KierowcaService _kierowcaService;
        private readonly PojazdService _pojazdService;

        public KierowcaController(ILogger<KierowcaController> logger, KierowcaService kierowcaService, PojazdService pojazdService)
        {
            _logger = logger;
            _kierowcaService = kierowcaService;
            _pojazdService = pojazdService;
        }

        public IActionResult Index()
        {
            ListAllClientsVM vm = new ListAllClientsVM()
            {
                Kierowcy = _kierowcaService.Get()
            };
            return View(vm);
        }
        public IActionResult Index_urzednik()
        {
            ListAllClientsVM vm = new ListAllClientsVM()
            {
                Kierowcy = _kierowcaService.Get()
            };
            return View(vm);
        }
        public IActionResult Index_Policja_Wyszukaj(string PESEL)
        {
            ListAllClientsVM vm = new ListAllClientsVM()
            {
                Kierowcy = _kierowcaService.Search(PESEL)
            };
            return View(vm);
        }
        public IActionResult Index_policja()
        {
            ListAllClientsVM vm = new ListAllClientsVM()
            {
                Kierowcy = _kierowcaService.Get()
            };
            return View(vm);
        }
        public IActionResult Add()
        {
            KierowcaVM model = new KierowcaVM();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(KierowcaVM vm)
        {
            if (ModelState.IsValid)
            {
                _kierowcaService.Create(vm.ToKierowca());
                return RedirectToAction("Index");
            }
            return View(vm);
        }
        public IActionResult Add_urzednik()
        {
            KierowcaVM model = new KierowcaVM();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add_urzednik(KierowcaVM vm)
        {
            if (ModelState.IsValid)
            {
                _kierowcaService.Create(vm.ToKierowca());
                return RedirectToAction("Index_urzednik");
            }
            return View(vm);
        }
        public IActionResult Delete(string id)
        {
            _kierowcaService.Remove(id);
            return RedirectToAction("Index");
        }
        public IActionResult Edit(string id)
        {
            var kierowca = _kierowcaService.Get(id);
            KierowcaVM model = new KierowcaVM(kierowca);
            return View(model);
        }
        public IActionResult Edit_urzednik(string id)
        {
            var kierowca = _kierowcaService.Get(id);
            KierowcaVM model = new KierowcaVM(kierowca);
            return View(model);
        }
        public IActionResult Edit_policja(string id)
        {
            var kierowca = _kierowcaService.Get(id);
            KierowcaVM model = new KierowcaVM(kierowca);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(KierowcaVM vm)
        {
            if (ModelState.IsValid)
            {
                _kierowcaService.Update(vm.Id, vm.ToKierowca());
                return RedirectToAction("Index");
            }
            return RedirectToAction("Edit", vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit_urzednik(KierowcaVM vm)
        {
            if (ModelState.IsValid)
            {
                _kierowcaService.Update(vm.Id, vm.ToKierowca());
                return RedirectToAction("Index");
            }
            return RedirectToAction("Edit", vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit_policja(KierowcaVM vm)
        {
            if (ModelState.IsValid)
            {
                _kierowcaService.Update(vm.Id, vm.ToKierowca());
                return RedirectToAction("Index_policja");
            }
            return RedirectToAction("Edit_policja", vm);
        }
        public IActionResult Pozycz(string id)
        {
            var wypozyczonePojazdy = _kierowcaService.Get().SelectMany(x => x.Pojazdy, (x, y) => new string(y.ToString())).ToList();
            PozyczVM model = new PozyczVM()
            {
                KierowcaId = id,
                Kierowca = new KierowcaVM(_kierowcaService.Get(id)),
                Pojazdy = _pojazdService.Get().Where(x => !wypozyczonePojazdy.Contains(x.Id)).ToList()
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Pozycz(PozyczVM model)
        {
            if (ModelState.IsValid && model.PojazdId != null && model.PojazdId != "")
            {
                if (_kierowcaService.Wypozycz(model.KierowcaId, model.PojazdId))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model);
                }
            }
            return View(model);
        }
        public IActionResult Zwroc(string id)
        {
            var wypozyczoneId = _kierowcaService.Get(id).Pojazdy.Select(x => x.ToString());
            ZwrotVM model = new ZwrotVM()
            {
                KierowcaId = id,
                Kierowca = new KierowcaVM(_kierowcaService.Get(id)),
                Pojazdy = _pojazdService.Get().Where(x => wypozyczoneId.Contains(x.Id)).ToList()
            };
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Zwroc(ZwrotVM model)
        {
            if(ModelState.IsValid)
            {
                _kierowcaService.Zwroc(model.KierowcaId, model.PojazdId);
                return RedirectToAction("Index");
            }
            model.Kierowca = new KierowcaVM(_kierowcaService.Get(model.KierowcaId));
            return View(model);
        }
        public IActionResult Wypozyczenia(string id)
        {
            var kierowca = _kierowcaService.Get(id);
            var pojazdy = kierowca.Pojazdy.Select(x => x.ToString()).ToList();
            WypozyczeniaVM vm = new WypozyczeniaVM()
            {
                Kierowca = new KierowcaVM(kierowca),
                Pojazdy = _pojazdService.Get().Where(x => pojazdy.Contains(x.Id)).ToList()
            };
            return View(vm);
        }

    }
    public class WypozyczeniaVM
    {
        public KierowcaVM Kierowca { get; set; }
        public List<Pojazd> Pojazdy { get; set; }
    }
    public class ZwrotVM
    {
        public string PojazdId { get; set; }
        public string KierowcaId { get; set; }
        public KierowcaVM Kierowca { get; set; }
        public List<Pojazd> Pojazdy { get; set; }
    }
    public class PozyczVM
    {
        public string KierowcaId { get; set; }
        public string PojazdId { get; set; }
        public KierowcaVM Kierowca { get; set; }
        public List<Pojazd> Pojazdy { get; set; }
    }
    public class ListAllClientsVM
    {
        public string TitleName { get; set; }
        public List<Kierowca> Kierowcy { get; set; }
    }
    public class KierowcaVM
    {
        public KierowcaVM()
        {
            Pojazdy = new List<ObjectId>();
        }
        public KierowcaVM(Kierowca kierowca)
        {
            Id = kierowca.Id;
            Imie = kierowca.Imie;
            Nazwisko = kierowca.Nazwisko;
            PESEL = kierowca.PESEL;
            DataUrodzenia = kierowca.DataUrodzenia;
            Kategorie = kierowca.Kategorie;
            Zatrzymanie = kierowca.Zatrzymanie;
            Zakaz = kierowca.Zakaz;
            Utrata = kierowca.Utrata;
            Punkty = kierowca.Punkty;
            Uwagi = kierowca.Uwagi;
            
            Pojazdy = kierowca.Pojazdy;
        }
        public string Id { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string PESEL { get; set; }
        public string DataUrodzenia { get; set; }
        public string Kategorie { get; set; }
        public decimal Punkty { get; set; }
        public string Zatrzymanie { get; set; }
        public string Zakaz { get; set; }
        public string Utrata { get; set; }
        public string Uwagi { get; set; }
        public List<ObjectId> Pojazdy { get; set; }
        public Kierowca ToKierowca()
        {
            return new Kierowca()
            {
                Imie = Imie,
                Nazwisko = Nazwisko,
                PESEL = PESEL,
                DataUrodzenia = DataUrodzenia,
                Kategorie = Kategorie,
                Zatrzymanie = Zatrzymanie,
                Zakaz = Zakaz,
                Utrata = Utrata,
                Punkty = Punkty,
                Uwagi = Uwagi,

                Id = Id,
                Pojazdy = Pojazdy
            };
        }
    }
}
