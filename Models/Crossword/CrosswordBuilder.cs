using Crossword;

//https://www.baeldung.com/cs/generate-crossword-puzzle

namespace CrosswordComponents
{
    /**
     * Klasa reprezentująca manager i generator krzyżówki
     */
    public class CrosswordBuilder
    {
        /**
         * Maksymalna ilość słów 
         */
        public int MaxWords { get; set; }

        /**
        * Obiekt krzyżówki
        */
        private CrosswordModel _crosswordModel;

        /**
         * Konstruktor pobiera referencje na pewien zbiór danych 
         */
        public CrosswordBuilder(Dictionary<string, string> wordsWithMeanings, int maxWords)
        {
            MaxWords = maxWords;

            //_wordsWithMeanings = wordsWithMeanings.Shuffle();

            _crosswordModel = new CrosswordModel();
        }

        /**
         * Metoda generująca krzyżówkę
         */
        public ICrosswordModelReadOnly GetCrossword()
        {
            return _crosswordModel;
        }
    }
}
