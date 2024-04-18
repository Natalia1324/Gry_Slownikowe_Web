using Crossword;
using CrosswordComponents;
using Gry_Słownikowe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace Gry_Słownikowe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IMemoryCache _memoryCache;

        private readonly CrosswordBuilder _crosswordBuilder;

        private SlownikowoModel _smodel;

        public HomeController(ILogger<HomeController> logger, IMemoryCache memoryCache)
        {
            _logger = logger;

            _memoryCache = memoryCache;

            _crosswordBuilder = new CrosswordBuilder();

        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MainMenu()
        {
            //SJP_API api = new SJP_API("półlok");
            //Console.WriteLine(api.getCzyIstnieje());
            //Console.WriteLine(api.getDopuszczalnosc());
            //Console.WriteLine(api.getSlowo());
            //foreach (var def in api.getZnaczenia())
            //{
            //    Console.WriteLine(def);
            //}

            //SJP_API api2 = new SJP_API();
            //Console.WriteLine(api2.getCzyIstnieje());
            //Console.WriteLine(api2.getDopuszczalnosc());
            //Console.WriteLine(api2.getSlowo());
            //foreach (var def in api2.getZnaczenia())
            //{
            //    Console.WriteLine(def);
            //}
            return View();
        }
        public IActionResult Wordle()
        {
            string slowo = "";
            List<string> znaczenia= new List<string>();
            while (slowo.Length != 5)
            {
                if (znaczenia.Count > 0)
                {
                    znaczenia.Clear();
                }
                SJP_API api = new SJP_API();
                slowo = api.getSlowo();
                znaczenia= api.getZnaczenia();


            }
            string polskieZnaki = HttpUtility.HtmlEncode(slowo);
            string znaczeniePL = HttpUtility.HtmlAttributeEncode(znaczenia.First());
            WordleModel model = new WordleModel(polskieZnaki, znaczeniePL);
            return View(model);
        }

        SlownikowoModel _slownikowoModel;

        [HttpGet]
        public IActionResult Slownikowo()
        {
            SJP_API random = new SJP_API();
            SJP_API api = new SJP_API("3D");
            _slownikowoModel = new SlownikowoModel(random.getSlowo(), api);
            return View(_slownikowoModel);
        }

        [HttpPost]
        public IActionResult Slownikowo(string slowo)
        {
            SJP_API api = new SJP_API(slowo);
           _slownikowoModel.changeApi(api);
            return View(_slownikowoModel);
        }

        public IActionResult ZgadywankiMenu ()
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

        public IActionResult ZgadywankiZasady()
        {
            return View();
        }

        public IActionResult Wisielec()
        {
            return View();
        }

        public IActionResult Krzyzowka()
        {
            ICrosswordModelReadOnly crosswordModel = _crosswordBuilder.GenerateCrossword(10).Get();
            _memoryCache.Set("CrosswordModel", crosswordModel);
            return View(crosswordModel);
        }

        /**
         * Obsługa literek
         */
        [HttpPost]
        public IActionResult GuessLetter(int row, int column, char letter)
        {
            bool success = false;
            ICrosswordModelReadOnly crossword = _memoryCache.Get<ICrosswordModelReadOnly>("CrosswordModel");
            if (crossword != null)
            {
                success = crossword[row, column].GuessLetter(letter);
            }
            return Json(new { success = success });
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
