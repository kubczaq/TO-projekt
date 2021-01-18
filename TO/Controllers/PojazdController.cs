using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    public class PojazdController : Controller
    {
        private readonly ILogger<PojazdController> _logger;
        private readonly PojazdService _pojazdService;
        private readonly KierowcaService _kierowcaService;
        public PojazdController(ILogger<PojazdController> logger, PojazdService pojazdService, KierowcaService kierowcaService)
        {
            _logger = logger;
            _pojazdService = pojazdService;
            _kierowcaService = kierowcaService;
        }

        public IActionResult Index()
        {
            ListAllVehiclesVM vm = new ListAllVehiclesVM()
            {
                Pojazdy = _pojazdService.Get()
            };
            return View(vm);
        }
        public IActionResult Index_urzednik()
        {
            ListAllVehiclesVM vm = new ListAllVehiclesVM()
            {
                Pojazdy = _pojazdService.Get()
            };
            return View(vm);
        }
        public IActionResult Index_Petent_Wyszukaj(string vin)
        {
            ListAllVehiclesVM vm = new ListAllVehiclesVM()
            {
                Pojazdy = _pojazdService.Search(vin)
            };
            return View(vm);
        }
        public IActionResult Index_Policja_Wyszukaj(string vin)
        {
            ListAllVehiclesVM vm = new ListAllVehiclesVM()
            {
                Pojazdy = _pojazdService.Search(vin)
            };
            return View(vm);
        }
        public IActionResult Index_petent()
        {
            ListAllVehiclesVM vm = new ListAllVehiclesVM()
            {
                Pojazdy = _pojazdService.Get()
            };
            return View(vm);
        }
        public IActionResult Index_policja()
        {
            ListAllVehiclesVM vm = new ListAllVehiclesVM()
            {
                Pojazdy = _pojazdService.Get()
            };
            return View(vm);
        }
        public IActionResult Add()
        {
            PojazdVM model = new PojazdVM();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(PojazdVM vm)
        {
            if (ModelState.IsValid)
            {
                _pojazdService.Create(vm.ToPojazd());
                return RedirectToAction("Index");
            }
            return View(vm);
        }
        public IActionResult Add_urzednik()
        {
            PojazdVM model = new PojazdVM();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add_urzednik(PojazdVM vm)
        {
            if (ModelState.IsValid)
            {
                _pojazdService.Create(vm.ToPojazd());
                return RedirectToAction("Index_urzednik");
            }
            return View(vm);
        }
        public IActionResult Delete(string id)
        {
            var klienciPojazdy = _kierowcaService.Get().SelectMany(x => x.Pojazdy, (x, y) => new { x.Id, PojazdId = y.ToString() }).ToList();
            var klienctWypozyczone = klienciPojazdy.FirstOrDefault(x => x.PojazdId == id);

            if(klienctWypozyczone != null)
            {
                _kierowcaService.Zwroc(klienctWypozyczone.Id, klienctWypozyczone.PojazdId);
            }

            _pojazdService.Remove(id);
            return RedirectToAction("Index");
        }
        public IActionResult Edit(string id)
        {
            var pojazd = _pojazdService.Get(id);
            PojazdVM model = new PojazdVM(pojazd);
            return View(model);
        }
        public IActionResult Edit_urzednik(string id)
        {
            var pojazd = _pojazdService.Get(id);
            PojazdVM model = new PojazdVM(pojazd);
            return View(model);
        }
        public IActionResult Edit_policja(string id)
        {
            var pojazd = _pojazdService.Get(id);
            PojazdVM model = new PojazdVM(pojazd);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PojazdVM vm)
        {
            if(ModelState.IsValid)
            {
                _pojazdService.Update(vm.Id, vm.ToPojazd());
                return RedirectToAction("Index");
            }
            return RedirectToAction("Edit", vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit_urzednik(PojazdVM vm)
        {
            if (ModelState.IsValid)
            {
                _pojazdService.Update(vm.Id, vm.ToPojazd());
                return RedirectToAction("Index_urzednik");
            }
            return RedirectToAction("Edit_urzednik", vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit_policja(PojazdVM vm)
        {
            if (ModelState.IsValid)
            {
                _pojazdService.Update(vm.Id, vm.ToPojazd());
                return RedirectToAction("Index_policja");
            }
            return RedirectToAction("Edit_policja", vm);
        }
    }
    public class ListAllVehiclesVM
    {
        public string TitleName { get; set; }
        public List<Pojazd> Pojazdy { get; set; }
    }
    public class PojazdVM
    {
        public PojazdVM()
        {

        }
        public PojazdVM(Pojazd pojazd)
        {
            Typ = pojazd.Typ;
            Marka = pojazd.Marka;
            Model = pojazd.Model;
            Vin = pojazd.Vin;
            DataProdukcji = pojazd.DataProdukcji;
            Pojemnosc = pojazd.Pojemnosc;
            Waga = pojazd.Waga;
            Rejestracja = pojazd.Rejestracja;
            Wlasciciel = pojazd.Wlasciciel;
            Ubezpieczenie = pojazd.Ubezpieczenie;
            Badanie = pojazd.Badanie;
            ZatrzymanieDowodu = pojazd.ZatrzymanieDowodu;
            UtrataDowodu = pojazd.UtrataDowodu;
            Kradziez = pojazd.Kradziez;
            Uwagi = pojazd.Uwagi;

            Id = pojazd.Id;
        }
        public enum TypPojazduEnum
        {
            [Display(Name = "Samochód osobowy")]
            Osobowy,
            [Display(Name = "Samochód ciężarowy")]
            Ciezarowy,
            [Display(Name = "Motocykl")]
            Motocykl,
            [Display(Name = "Autobus")]
            Autobus
        }
        public string Id { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        public string Typ { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        public string Marka { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane")]
        public string Model { get; set; }
        public string Vin { get; set; }
        public string DataProdukcji { get; set; }
        public decimal Pojemnosc { get; set; }
        public decimal Waga { get; set; }
        public string Rejestracja { get; set; }
        public string Wlasciciel { get; set; }
        public string Ubezpieczenie { get; set; }
        public string Badanie { get; set; }
        public string ZatrzymanieDowodu { get; set; }
        public string UtrataDowodu { get; set; }
        public string Kradziez { get; set; }
        public string Uwagi { get; set; }
        public Pojazd ToPojazd()
        {
            return new Pojazd()
            {
                Typ = Typ,
                Marka = Marka,
                Model = Model,
                Vin = Vin,
                DataProdukcji = DataProdukcji,
                Pojemnosc = Pojemnosc,
                Waga = Waga,
                Rejestracja = Rejestracja,
                Wlasciciel = Wlasciciel,
                Ubezpieczenie = Ubezpieczenie,
                Badanie = Badanie,
                ZatrzymanieDowodu = ZatrzymanieDowodu,
                UtrataDowodu = UtrataDowodu,
                Kradziez = Kradziez,
                Uwagi = Uwagi,

                Id = Id
            };
        }
    }
}
