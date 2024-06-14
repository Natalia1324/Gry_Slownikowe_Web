using Crossword;
using CrosswordComponents;
using Gry_Slownikowe.Entions;
using Gry_Slownikowe.Entities;
using Gry_Slownikowe.Models;
//using Gry_Słownikowe.Models;
using Gry_Slownikowe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using System;

//using Newtonsoft.Json;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


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
        public IActionResult Wordle(int dlugosc, int enabledRowIndex)
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
            string polskieZnaki = HttpUtility.HtmlEncode(slowo);// ó
            string znaczeniePL = HttpUtility.HtmlAttributeEncode(znaczenia.First());
            var id = getLoggedUser().Id;
            if (enabledRowIndex != 0)
            {
                var rekord = new Wordle
                {
                    Win = enabledRowIndex,
                    Loss = 0,
                    UserId = id,
                    //  User = getLoggedUser(),
                    //  GameTime = TimeSpan(1s),

                };
                _context.Wordle.Add(rekord);
                _context.SaveChanges();
            }
            int[] counts = new int[7];
            for (int i = 0; i<counts.Length; i++)
            {
                counts[i]= _context.Wordle.Count(entity => entity.Win == (i+1) && entity.UserId == id);
            }
          


            WordleModel model = new WordleModel(polskieZnaki, znaczeniePL, dlugosc, counts);
            return View(model);
        }
        [HttpGet]
        public IActionResult Slownikowo()
        {
            
            SJP_API random;

            do
            {
                random = new SJP_API();
            } while (!random.getDopuszczalnosc());


            SlownikowoModel _slownikowoModel = new(random.getSlowo());
            return View(_slownikowoModel);
            
        }
        public async Task<IActionResult> Statistics()
        {
            if (getLoggedUser() == null)
            {
                return View("Login");
            }
            else { 
            var userId = getLoggedUser().Id;

            var user = await _context.User
            .Include(u => u.Krzyzowki)
            .Include(u => u.Wisielec)
            .Include(u => u.Wordle)
            .Include(u => u.Zgadywanki)
            .Include(u => u.Slownikowo)
            .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            var model = new StatisticsModel
            {
                Nick = user.Nick,
                GameStatistics = new List<GameStatistics>
            {
                new GameStatistics
                {
                    GameName = "Krzyzowki",
                    TotalGames = user.Krzyzowki.Count,
                    Wins = user.Krzyzowki.Sum(g => g.Win),
                    Losses = user.Krzyzowki.Sum(g => g.Loss)
                },
                new GameStatistics
                {
                    GameName = "Wisielec",
                    TotalGames = user.Wisielec.Count,
                    Wins = user.Wisielec.Count(g => g.wygrana_przegrana == true),
                    Losses = user.Wisielec.Count(g => g.wygrana_przegrana == false)
                },
                new GameStatistics
                {
                    GameName = "Wordle",
                    TotalGames = user.Wordle.Count,
                    Wins = user.Wordle.Sum(g => g.Win),
                    Losses = user.Wordle.Sum(g => g.Loss)
                },
                new ZgadywankiStatistics
                {
                    GameName = "Zgadywanki",
                    TotalGames = user.Zgadywanki.Count,
                    Wins = user.Zgadywanki.Count(g => g.Punkty >= 20),
                    Losses = user.Zgadywanki.Count(g => g.Punkty < 20),
                    Punkty = user.Zgadywanki.Sum(g => g.Punkty)
                },
                new GameStatistics
                {
                    GameName = "Slownikowo",
                    TotalGames = user.Slownikowo.Count,
                    Wins = user.Slownikowo.Count(g => g.Win == true),
                    Losses = user.Slownikowo.Count(g => g.Win == false)
                }
            }
            };

            return View(model);
            }
        }
        [HttpPost]
        public IActionResult SaveGame(bool win, int tries, int gameTime)
        {
          
            Console.WriteLine("Wygrana: " + win);
            Console.WriteLine("Proby: " + tries);
            Console.WriteLine("Czas: " + gameTime);

            if (getLoggedUser() != null)
            {

                try
                {
                    TimeSpan timespan = TimeSpan.FromMilliseconds(gameTime);
                    var newRecord = new Slownikowo
                    {
                        Win = win,
                        Tries = tries,
                        GameTime = timespan, // 1 godzina, 30 minut
                        GameData = DateTime.Now,
                        UserId = getLoggedUser().Id
                    };

                    getLoggedUser().Slownikowo.Add(newRecord);
                    _context.Slownikowo.Add(newRecord);
                    _context.SaveChanges();
                }
                catch
                {
                    return Json(new { success = false, message = "Couldn't push to database" });
                }
                return Json(new { success = true, message = "Data saved successfully." });
            }
            else return Json(new { success = false, message = "Didn't push to database - played as guest." });
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
        [HttpPost]
        public IActionResult LosujSlowo()
        {
            SJP_API random;

            do
            {
                random = new SJP_API();
            } while (!random.getDopuszczalnosc());

            // Zwracamy wynik sprawdzenia w formie JSON
            return Json(new { slowo = random.getSlowo() });
        }
        public IActionResult ZgadywankiPTrudności()
        {
            SJP_API api1, api2, api3, api4;
            string slowo1, slowo2, slowo3, slowo4;
            string polskieZnaki1, polskieZnaki2, polskieZnaki3, polskieZnaki4;


            do
            {

                // Losowanie pierwszego słowa
                do
                {
                    api1 = new SJP_API();
                } while (api1.getDopuszczalnosc() == false);
                slowo1 = api1.getSlowo();
                polskieZnaki1 = slowo1;

                // Losowanie drugiego słowa
                do
                {
                    api2 = new SJP_API();

                } while (api2.getDopuszczalnosc() == false);
                slowo2 = api2.getSlowo();
                polskieZnaki2 = slowo2;

                // Losowanie tzreciego słowa
                do
                {
                    api3 = new SJP_API();
                } while (api3.getDopuszczalnosc() == false);
                slowo3 = api3.getSlowo();
                polskieZnaki3 = slowo3; 

                // Losowanie czwarte słowa
                do
                {
                    api4 = new SJP_API();
                } while (api4.getDopuszczalnosc() == false);
                slowo4 = api4.getSlowo();
                polskieZnaki4 = slowo4;// HttpUtility.HtmlEncode(slowo4);// - Problem z "ó" 

            } while (slowo2 == null || slowo3 == null || slowo4 == null);

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
                    
                }
            }

            foreach (var litera in slowo3)
            {
                if (literyZPierwszego.Contains(litera))
                {
                    licznik3++;
                   
                }
            }

            foreach (var litera in slowo4)
            {
                if (literyZPierwszego.Contains(litera))
                {
                    licznik4++;
                 
                }
            }


            return licznik2 >= 4 && licznik3 >= 4 && licznik4 >= 4;
        }
        public IActionResult ZgadywankiMenu ()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ZgadywankiWynik(int punkty, int gameTime)
        {
            Console.WriteLine("Wygrana: " + punkty);
            Console.WriteLine("Czas: " + gameTime);

            if (getLoggedUser() == null) { return Json(new { success = false, message = "You not login in web page!!" }); }

                try
                {
                    TimeSpan timespan = TimeSpan.FromMilliseconds(gameTime);
                    var newRecord = new Zgadywanki
                    {

                        Punkty = punkty,
                        GameTime = timespan,
                        UserId = getLoggedUser().Id
                    };

                    getLoggedUser().Zgadywanki.Add(newRecord);
                    _context.Zgadywanki.Add(newRecord);
                    _context.SaveChanges();
                }
                catch
                {
                    return Json(new { success = false, message = "Couldn't push to database" });
                }
            
                return Json(new { success = true, message = "Data saved successfully." });
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

        [HttpPost]
        public IActionResult saveGameData(bool punkty, int gameTime)
        {
            Console.WriteLine("Wygrana: " + punkty);
            Console.WriteLine("Czas: " + gameTime);

            if (getLoggedUser() == null) { return Json(new { success = false, message = "You not login in web page!!" }); }

            try
            {
                TimeSpan timespan = TimeSpan.FromMilliseconds(gameTime);
                var newRecord = new Wisielec
                {

                    wygrana_przegrana = punkty,
                    GameTime = timespan,

                    UserId = getLoggedUser().Id
                };

                getLoggedUser().Wisielec.Add(newRecord);
                _context.Wisielecs.Add(newRecord);
                _context.SaveChanges();
            }
            catch
            {
                return Json(new { success = false, message = "Couldn't push to database" });
            }

            return Json(new { success = true, message = "Data saved successfully." });
        }

        public IActionResult Wisielec()
        {
            string slowo = "";
            bool isCorrect = false;
            SJP_API api;
            do
            {
                api = new SJP_API();
                isCorrect = api.getDopuszczalnosc();
            } while (isCorrect == false);

            slowo = api.getSlowo();
            string polskieZnaki = HttpUtility.HtmlEncode(slowo);// ó

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
