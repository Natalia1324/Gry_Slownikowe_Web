using HtmlAgilityPack;
using System.Net;
using System.Diagnostics;
using System.Text;
using System.Web;
using System;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Mvc;

namespace Gry_Slownikowe.Models
{
    public class SJP_API
    {
        //link losujacy
        //

        private readonly string baseUrl = "https://sjp.pl/";

        private List<string> znaczenia = new List<string>();
        private bool czy_dopuszczalne = false;
        private bool czy_wystepuje = true;
        private string slowo = "";
        public SJP_API(string word)
        {
            GetMeaning(baseUrl + Uri.EscapeUriString(word));
        }

        public SJP_API() {
            GetMeaning("https://sjp.pl/sl/los/");
        }

        public bool getDopuszczalnosc()
        {
            return czy_dopuszczalne;
        }

        public bool getCzyIstnieje()
        {
            return czy_wystepuje;
        }

        public List<string> getZnaczenia()
        {
            return znaczenia;
        }

        public string getSlowo()
        {
            return slowo;
        }

        private void GetMeaning(string url)
        {
            try
            {
                // Tworzenie żądania HTTP
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                // Pobieranie odpowiedzi
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Sprawdzanie czy odpowiedź jest OK
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        // Tworzenie obiektu HtmlDocument
                        HtmlDocument htmlDoc = new HtmlDocument();

                        // Wczytywanie zawartości strony
                        htmlDoc.Load(response.GetResponseStream());


                        
                        string input = getDefinitions(htmlDoc.DocumentNode.InnerText);
                        

                        if (input != null)
                        {
                            znaczenia = splitDefinitions(input);
                        }

                    }
                    else
                    {
                        Console.WriteLine("Błąd podczas pobierania strony: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd podczas pobierania znaczenia: " + ex.Message);
            }
        }

        private void getInformation (string input)
        {
            //czy slowo jest dopuszczalne w grze
            if (!(input.Contains("niedopuszczalne w grach")))
            {
                czy_dopuszczalne = true;
            }

        }
        private string removeDoubleDef(string input)
        {
            //czy slowo posiada podwojona definicje (np. slowo i nazwisko, slowo i stara definicja)
            if (input.Contains("\n-\n"))
            {
                //zostaw tylko 1 definicje
                int dividerIndex = input.IndexOf("\n-\n");
                return input.Substring(0, dividerIndex + 3);
            }
            else
            {
                //jak nie, to nic nie rób
                return input;
            }
        }

        private string getWord(string input)
        {
            Console.OutputEncoding = Encoding.UTF8;
            string startWord = "sprawdź";
            string endWord = "niedopuszczalne";
            string endWord2 = "dopuszczalne";

            int firstIndex = input.IndexOf(startWord); // Pierwsze wystąpienie
            int startIndex = input.IndexOf(startWord, firstIndex + 1); // Drugie wystąpienie, szukamy od indeksu po pierwszym wystąpieniu + 1
            int endIndex = input.IndexOf(endWord);
            if (endIndex == -1) endIndex = input.IndexOf(endWord2);
            
            if (startIndex != -1 && endIndex != -1)
            {
                //zwroc definicje

                //Console.WriteLine(input.Substring(startIndex + 13, endIndex - startIndex - 14));
                //Console.WriteLine(input);
                //Console.WriteLine(input.Substring(startIndex + 13, endIndex - startIndex - 14));
                //var temp = HttpUtility.HtmlEncode(input);
                //var temp2 = temp.Substring(startIndex + 13, endIndex - startIndex - 14);
                //Console.WriteLine("HERE IS THE SHIT");
                //Console.WriteLine(HttpUtility.HtmlDecode(temp2));
                Console.WriteLine(CustomSubstring(input, startIndex + 13, endIndex - startIndex - 14));
                //return CustomSubstring(input, startIndex + 13, endIndex - startIndex - 14);
                return input.Substring(startIndex + 13, endIndex - startIndex - 14);
            }
            else return null;
        }
        public string CustomSubstring(string text, int startIndex, int endIndex)
        {
            Console.WriteLine(text);
            // Sprawdzamy, czy indeksy są poprawne
            if (startIndex < 0)
            {
                startIndex = 0;
            }
            if (endIndex > text.Length)
            {
                endIndex = text.Length;
            }

            // Inicjalizujemy zmienną przechowującą wynik
            string result = "";

            // Pobieramy podciąg tekstu pomiędzy indeksami
            for (int i = startIndex; i < endIndex; i++)
            {
                Console.WriteLine(text[i]);
                result += text[i];
            }

            // Zwracamy wynik
            return result;
        }


        private string removeComments(string input)
        {
            //najpierw usun podwojne definicje
            input = removeDoubleDef(input);
            string komentarzeWord = "KOMENTARZE";
            int komentarzeIndex = input.IndexOf(komentarzeWord);
            //czy jest sekcja komentarzy
            if (komentarzeIndex != -1)
            {
                //usun komentarze
                return input.Substring(0, komentarzeIndex + komentarzeWord.Length);
            }
            else
            {
                return input;
            }
        }

        private string getDefinitions(string input)
        {
            //usuwanie komentarzy (niepotrzebne)
            string newinput = removeComments(input);
            //sprawdzenie czy slowo jest dopuszczalne
            getInformation(newinput);
            slowo = getWord(newinput);
            //slowa klucze po ktorych szukam definicji
            string startWord = "znaczenie";
            string endWord1 = "POWIĄZANE";
            string endWord2 = "KOMENTARZE";
            string endWord3 = "\n-\n";

            int startIndex = newinput.IndexOf(startWord);
            //wybranie poprawnego slowa koncowego (nie kazde zawsze wystepuje)
            int endIndex = newinput.IndexOf(endWord1);
            if (endIndex == -1)
                endIndex = newinput.IndexOf(endWord2);
            if (endIndex == -1)
                endIndex = newinput.IndexOf(endWord3);

            if (startIndex != -1 && endIndex != -1)
            {
                //zwroc definicje
                return newinput.Substring(startIndex + 21, endIndex - startIndex - 21);
            }
            else
            {
                //brak definicji na stronie
                return null;
            }
        }

        private List<string> splitDefinitions(string input)
        {
            List<string> definitions = new List<string>();

            if (input.StartsWith("["))
            {
                int endIndex = input.IndexOf("]");
                input = input.Substring(endIndex + 1).TrimStart();
            }
            if (input.Contains("1."))
            {
                // Usunięcie numeracji i podział na definicje przy użyciu znaku średnika jako separatora
                List<string> parts = input.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                if (!parts[0].StartsWith("1."))
                {
                    parts.RemoveAt(0);
                }
                    // Usunięcie numeracji z każdej definicji
                    foreach (string part in parts)
                    {
                        int dotIndex = part.IndexOf('.');
                        if (dotIndex != -1)
                        {
                            definitions.Add(part.Substring(dotIndex + 1).Trim());
                        }
                    }

            }
            else
            {
                definitions.Add(input);
            }
            return definitions;
        }
    }
}
