using Crossword;
using CrosswordComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gry_Słownikowe.Models
{
    public interface ICrosswordModelReadOnly
    {
        /**
         * Metoda zwracająca ilość odgadniętych liter
         */
        public int GetGuessedLetters();
        /**
         * Metoda wypisująca krzyżówkę
         */
        public void PrintCrosswordInConsole();
        /**
         * Wiersze krzyżówki
         */
        int Rows { get; }

        /**
         * Kolumny krzyżówki
         */
        public int Columns { get; }
        /**
         * Dostęp do ilości liter w krzyżowce
         */
        public int Letters { get; }

        /**
         * Dostęp do ilości słów w krzyżówce
         */
        public int Words { get;  }

        /**
         * Dostęp do hasła krzyżówki
         */
        public string? ClueWord {  get; }

        /**
         * Dostęp do definicji hasła krzyżówki
         */
        public string? ClueDefinition {  get; }

        /**
         * Dostęp do odczytu komórki
         */
        public CrosswordLetterModel this[int y, int x] { get; }

        /**
         * Otrzymaj listę opisów słów
         */
        public List<string> GetDescriptions();

        /**
         * Otrzymaj czas wygenerowania krzyżówki
         */
        public double GetGenerationTimeInMils();

        /**
         * Zacznij liczyć czas rozwiązywania
         */
        public void StartTimer();

        /**
         * Otrzymaj czas rozwiązywania krzyżówki
         */
        public double GetTime();
    }
}
