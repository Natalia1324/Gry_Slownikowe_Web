using Crossword;
using Gry_Slownikowe.Models;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;

//https://www.baeldung.com/cs/generate-crossword-puzzle

namespace CrosswordComponents
{
    /**
     * Klasa reprezentująca manager i generator krzyżówki
     * 
     * TODO - połączyć z API
     */
    public class CrosswordBuilder
    {
        /**
         * Maksymalna ilość słów 
         */
        public int MaxWords { get; set; }

        /**
         * API słownika
         */
        SJP_API wordAPI;

        /**
         * Lista słów do określania, czy się nie powtarzają
         */
        public List<string> _words;
        /**
        * Obiekt krzyżówki
        */
        private CrosswordModel? _crosswordModel;

        /**
         * Konstruktor pobiera referencje na pewien zbiór danych 
         */
        public CrosswordBuilder()
        {
            wordAPI = new SJP_API();

            _words = new List<string>();
        }

        public ICrosswordModelReadOnly Get()
        {
            if (_crosswordModel == null) throw new NullReferenceException("The crossword has not been inicialized.");
            return _crosswordModel;
        }

        /**
         * Metoda generująca krzyżówkę
         */
        public CrosswordBuilder GenerateCrossword(int maxWords)
        {
            _crosswordModel = new CrosswordModel();
            _words.RemoveAll(word => true);
            MaxWords = maxWords;
            string word;
            string meaning;
            List<string> meanings;
            while (_words.Count < MaxWords)
            {
                wordAPI = new SJP_API();
                if (!wordAPI.getDopuszczalnosc()) continue;

                Random random = new Random();
                meanings = wordAPI.getZnaczenia();

                if (meanings.Count <= 0) continue;

                word = HttpUtility.HtmlEncode(wordAPI.getSlowo());
                meaning = HttpUtility.HtmlAttributeEncode(meanings.ElementAt(random.Next(meanings.Count-1)));
 
                if (word.Length < 1 || word.Length > 32 || !ContainsOnlyPolishLetters(word)) { continue; }

                if (_words.Contains(word))
                {
                    continue;
                }

                if(_crosswordModel.InsertWord(word, meaning))
                {
                    _words.Add(word);
                }
            }
            return this;
        }
        /**
         * Sprawdzenie czy zawiera polskie litery
         */
        private bool ContainsOnlyPolishLetters(string word)
        {
            Regex polishLettersRegex = new Regex(@"^[a-zA-ZęóąśłżźćńĘÓĄŚŁŻŹĆŃ]+$");
            return polishLettersRegex.IsMatch(word);
        }
    }
}
