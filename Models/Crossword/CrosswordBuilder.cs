using Crossword;
using Gry_Słownikowe.Models;
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
        private CrosswordModel _crosswordModel;

        /**
         * Konstruktor pobiera referencje na pewien zbiór danych 
         */
        public CrosswordBuilder(int maxWords)
        {
            MaxWords = maxWords;

            wordAPI = new SJP_API();

            _words = new List<string>();

            _crosswordModel = new CrosswordModel();
        }
        /**
         * Metoda generująca krzyżówkę
         */
        public ICrosswordModelReadOnly GenerateCrossword()
        {
            string word;
            string meaning;
            List<string> meanings;
            while (_words.Count < MaxWords)
            {
                Random random = new Random();
                meanings = wordAPI.getZnaczenia();

                // wczytywanie słów
                word = HttpUtility.HtmlEncode(wordAPI.getSlowo());
                meaning = HttpUtility.HtmlAttributeEncode(meanings.ElementAt(random.Next(meanings.Count)));

                // nowe słowo
                wordAPI = new SJP_API();

                if (_words.Contains(word))
                {
                    continue;
                }

                if(_crosswordModel.InsertWord(word, meaning))
                {
                    _words.Add(word);
                }
            }
            return _crosswordModel;
        }
    }
}
