﻿using Crossword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordComponents
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
         * Dostęp do odczytu komórki
         */
        public CrosswordTile this[int y, int x] { get; }

        /**
         * Otrzymaj listę opisów słów
         */
        public List<string> GetDescriptions();
    }
}