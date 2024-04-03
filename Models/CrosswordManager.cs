namespace Gry_Słownikowe.Models
{
    /**
     * Klasa reprezentująca manager i generator krzyżówki
     * 
     * testowanie: CrosswordManager crosswordManager = new CrosswordManager(new Dictionary<string, string> { { "v1", "d" }, { "v2", "d" } }, 1);
     */
    internal class CrosswordManager
    {
        public int WordCount { get; set; }

        private Dictionary<string, string> _selectedWordsWithMeanings;

        /**
         * Obiekt krzyżówki
         */
        private CrosswordModel _crosswordModel;

        /**
         * Konstruktor pobiera referencje na pewien zbiór danych 
         * (narazie słownik do testów {słowo, definicja}, jak będzie baza słów to odpowiednio się przerbi ) 
         * i losuje zbiór słów
         */
        public CrosswordManager(Dictionary<string, string> wordsWithMeanings, int maxWords)
        {
            WordCount = maxWords;

            _selectedWordsWithMeanings = new Dictionary<string, string>();

            List<int> randomIndexes = Enumerable.Range(0, wordsWithMeanings.Count).OrderBy(x => Guid.NewGuid()).Take(maxWords).ToList();

            foreach (int index in randomIndexes)
            {
                var kvp = wordsWithMeanings.ElementAt(index);
                _selectedWordsWithMeanings.Add(kvp.Key, kvp.Value);
            }
            _crosswordModel = new CrosswordModel();
        }

        /**
         * Metoda generująca krzyżówkę
         */
        public void GenerateCrossword()
        {
            //TODO https://www.baeldung.com/cs/generate-crossword-puzzle
            //Docelowo Algorytm 4
        }

        /**
         * Metoda testowa
         */
        public void PrintSelected()
        {
            Console.WriteLine("Selected Words with Meanings:");
            foreach (var pair in _selectedWordsWithMeanings)
            {
                Console.WriteLine("Word: " + pair.Key + ", Meaning: " + pair.Value);
            }
        }

    }
}
