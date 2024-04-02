using Gry_Słownikowe.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Gry_Słownikowe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MainMenu()
        {
            return View();
        }

        [Route("Wordle")]
        public IActionResult Wordle()
        {
            // Tutaj możesz przekierować użytkownika na stronę Wordle
            return View();
        }

        public IActionResult Scrabble()
        {
            // Tutaj możesz przekierować użytkownika na stronę Scrabble
            return RedirectToAction("Scrabble", "Game");
        }

        public IActionResult Krzyzowki()
        {
            // Tutaj możesz przekierować użytkownika na stronę Krzyzowki
            return RedirectToAction("Krzyzowki", "Game");
        }




        public IActionResult Zgadywanki()
        {
            // Tutaj możesz przekierować użytkownika na stronę Zgadywanki
           return RedirectToAction("Zgadywanki", "Game");
        }
        public IActionResult ZgadywankiMenu()
        {
            return View();
        }

        public IActionResult ZgadywankiZasady()
        {
            return View();
        }

        public IActionResult ZgadywankiPTrudności()
        {
            return View();
        }

        public IActionResult ZgadywankiSlowotok(string poziom)
        {
            ViewBag.PoziomTrudnosci = poziom;
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
