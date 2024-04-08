using System;
using Crossword;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
         * Pobrane słowa i ich znaczenia
         */
        private Dictionary<string, string> _wordsWithMeanings;

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
        public ICrosswordModelReadOnly GenerateCrossword()
        {
            int count = 0;
            foreach (var pair in _wordsWithMeanings)
            {
               if(_crosswordModel.InsertWord(pair.Key, pair.Value))
                {
                    count++;
                }
                if (count >= MaxWords) break;
            }
            return _crosswordModel;
        }
    }
}
