using Crossword;
using CrosswordComponents;
using Gry_Slownikowe.Entions;
using Gry_Slownikowe.Entities;
using Gry_Slownikowe.Models;
using Gry_Slownikowe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
//using Newtonsoft.Json;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;


namespace Gry_Slownikowe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IMemoryCache _memoryCache;

        private readonly CrosswordBuilder _crosswordBuilder;
        private readonly GryContext _context;


        public HomeController(ILogger<HomeController> logger, IMemoryCache memoryCache, GryContext context)
        {
            _logger = logger;

            _memoryCache = memoryCache;

            _crosswordBuilder = new CrosswordBuilder();
            _context = context;
        }

        private User getLoggedUser()
        {
            return HttpContext.Session.GetObject<User>("LoggedUser");
        }

        [HttpPost]
        public IActionResult Login(User newUser)
        {
            var u = _context.User
             .FirstOrDefault(u => u.Nick == newUser.Nick && u.Password == newUser.Password);

            if (u != null)
            {
                newUser.isLogged = true;
                var loggedUser = newUser;
                loggedUser.Id = u.Id;
                loggedUser.Login = u.Login;
                loggedUser.Ranks = u.Ranks;
                HttpContext.Session.SetObject("LoggedUser", loggedUser);
                return View("Index", loggedUser);

            }
            else
            {
                //return View("Index", getLoggedUser());
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View();
            }
        }

        [HttpPost]
        [Route("/Home/Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("LoggedUser");
            return Ok();
        }

        public IActionResult Register()
        {
            return View(getLoggedUser());
        }

        public IActionResult Login()
        {
            return View(getLoggedUser());
        }

        [HttpPost]
        public IActionResult Register(User newUser)
        {

            var u = _context.User
            .FirstOrDefault(u => (u.Nick == newUser.Nick && u.Login == newUser.Login) || u.Login == newUser.Login);

            if (u != null)
            {
                ModelState.AddModelError(string.Empty, "The user is already registered");
                return View();

            }
            else
            {

                newUser.Ranks = 0;
                _context.User.Add(newUser);
                _context.SaveChanges();
                newUser.isLogged = true;
                var loggedUser = newUser;
                HttpContext.Session.SetObject("LoggedUser", loggedUser);
                return View("Index", loggedUser);

            }

        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MainMenu()
        {
            return View();
        }
        public IActionResult Scrabble()
        {
            return View();
        }


        public IActionResult WordleGamemode()
        {
            return View();
        }
        public IActionResult Wordle(int dlugosc)
        {
            string slowo = "";
            if (dlugosc == 0)
            {
                dlugosc = 5;
            }
            List<string> znaczenia = new List<string>();
            while (slowo.Length != dlugosc)
            {
                if (znaczenia.Count > 0)
                {
                    znaczenia.Clear();
                }
                SJP_API api = new SJP_API();
                slowo = api.getSlowo();
                znaczenia = api.getZnaczenia();


            }
            string polskieZnaki = HttpUtility.HtmlEncode(slowo);
            string znaczeniePL = HttpUtility.HtmlAttributeEncode(znaczenia.First());
            var id = getLoggedUser().Id;
            int[] counts = new int[7];
            for (int i = 0; i<counts.Length; i++)
            {
                counts[i]= _context.Wordle.Count(entity => entity.Win == (i+1) && entity.UserId == id);
            }
          


            WordleModel model = new WordleModel(polskieZnaki, znaczeniePL, dlugosc, counts);
            return View(model);
        }




        //znaki unicode

        [HttpGet]
        public IActionResult Slownikowo()
        {
            SJP_API random;

            do
            {
                random = new SJP_API();
            } while (!random.getDopuszczalnosc());


            //SlownikowoModel _slownikowoModel = new(random.getSlowo());
            //random = new SJP_API("żółć");

            //string slowo = HttpUtility.HtmlEncode(random.getSlowo());
            SlownikowoModel _slownikowoModel = new(random.getSlowo());
            //Console.WriteLine(_slownikowoModel.WylosowaneSlowo);
            return View(_slownikowoModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveGameSlownikowo([FromBody] Slownikowo game)
        {
            if (game == null)
            {
                _logger.LogError("Invalid game data received");
                return BadRequest(new { message = "Invalid game data" });
            }

            try
            {
                game.GameData = DateTime.Now;
                _context.Slownikowo.Add(game);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Game saved successfully");
                return Ok(new { message = "Game saved successfully", game });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving game data");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }


        [HttpPost]
        public IActionResult SprawdzSlowo(string wpisaneSlowo)
        {
            // Tutaj dodaj logikę sprawdzania słowa
            SJP_API api = new SJP_API(wpisaneSlowo);

            bool isCorrect = api.getDopuszczalnosc();

            // Zwracamy wynik sprawdzenia w formie JSON
            return Json(new { IsCorrect = isCorrect });
        }

        public IActionResult ZgadywankiPTrudności()
        {
            // Losowanie pierwszego słowa
            SJP_API api1 = new SJP_API();
            string slowo1 = api1.getSlowo();
            string polskieZnaki1 = HttpUtility.HtmlEncode(slowo1);

            // Losowanie drugiego słowa
            SJP_API api2 = new SJP_API();
            string slowo2 = api2.getSlowo();
            string polskieZnaki2 = HttpUtility.HtmlEncode(slowo2);

            // Losowanie tzreciego słowa
            SJP_API api3 = new SJP_API();
            string slowo3 = api3.getSlowo();
            string polskieZnaki3 = HttpUtility.HtmlEncode(slowo3);

            // Losowanie czwarte słowa
            SJP_API api4 = new SJP_API();
            string slowo4 = api4.getSlowo();
            string polskieZnaki4 = HttpUtility.HtmlEncode(slowo3);

            // Losowanie piąte słowa
            SJP_API api5 = new SJP_API();
            string slowo5 = api5.getSlowo();
            string polskieZnaki5 = HttpUtility.HtmlEncode(slowo3);

            // Sprawdzanie, czy drugie słowo zawiera co najmniej trzy litery z pierwszego słowa
            while (!CzyDrugieSlowoMaMinTrzyLitery(slowo1, slowo2, slowo3, slowo4))
            {
                api2 = new SJP_API();
                slowo2 = api2.getSlowo();
                polskieZnaki2 = HttpUtility.HtmlEncode(slowo2);

                api3 = new SJP_API();
                slowo3 = api3.getSlowo();
                polskieZnaki3 = HttpUtility.HtmlEncode(slowo3);

                api4 = new SJP_API();
                slowo4 = api4.getSlowo();
                polskieZnaki4 = HttpUtility.HtmlEncode(slowo4);

            }

            ZgadywankiModel model = new ZgadywankiModel(polskieZnaki1, polskieZnaki2, polskieZnaki3, polskieZnaki4);
            return View(model);
        }

        private bool CzyDrugieSlowoMaMinTrzyLitery(string slowo1, string slowo2, string slowo3, string slowo4)
        {
            var literyZPierwszego = new HashSet<char>(slowo1);
            int licznik2 = 0, licznik3 = 0, licznik4 = 0;
            foreach (var litera in slowo2)
            {
                if (literyZPierwszego.Contains(litera))
                {
                    licznik2++;
                    if (licznik2 >= 2)
                    {
                        break;
                    }
                }
            }

            foreach (var litera in slowo3)
            {
                if (literyZPierwszego.Contains(litera))
                {
                    licznik3++;
                    if (licznik3 >= 2)
                    {
                        break;
                    }
                }
            }

            foreach (var litera in slowo4)
            {
                if (literyZPierwszego.Contains(litera))
                {
                    licznik4++;
                    if (licznik4 >= 2)
                    {
                        break;
                    }
                }
            }


            return licznik2 >= 2 && licznik3 >= 2 && licznik4 >= 2;
        }

        public IActionResult ZgadywankiMenu ()
        {

            return View();
        }

       // public IActionResult ZgadywankiPTrudności()
       // {
        //    return View();
       // }

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
            string slowo = "";
            SJP_API api = new SJP_API();
            slowo = api.getSlowo();
            string polskieZnaki = HttpUtility.HtmlEncode(slowo);

            WisielecModel model = new WisielecModel(polskieZnaki);
            return View(model);
        }

        public IActionResult KrzyzowkaMenu()
        {
            return View();
        }

        public IActionResult Krzyzowka(int crosswordSize = 10)
        {
            if (crosswordSize < 0) crosswordSize = 5;
            ICrosswordModelReadOnly crosswordModel = _crosswordBuilder.GenerateCrossword(crosswordSize).Get();
            crosswordModel.StartTimer();
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

        [HttpPost]
        public IActionResult CheckIfFinished()
        {
            ICrosswordModelReadOnly crossword = _memoryCache.Get<ICrosswordModelReadOnly>("CrosswordModel");
            if (crossword != null && crossword.Letters == crossword.GetGuessedLetters())
            {
                return Json(new { success = true, time = crossword.GetTime() });
            }
            else
            {
                return Json(new { success = false, time = 0 });
            }
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
