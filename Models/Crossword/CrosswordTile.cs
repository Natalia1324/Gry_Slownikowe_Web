using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosswordComponents
{
    /**
     * Klasa reprezentująca kafelek (literkę) krzyżówki
     */
    public class CrosswordTile
    {

        private int? _wordNumber = null;

        private bool _visible = false; 

        private readonly char _letter;

        private bool _isFirstLetter = false;

        /**
         * Konstruktor
         */
        public CrosswordTile(char letter) 
        {
            _letter = char.ToUpper(letter);
        }
        /**
         * Accessor do wartości
         */
        public char Value
        {
            get { 
                return _letter;
            }
        }
        /**
         * Ustawienie widzialności
         */
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        /**
         * Ustawienie numerka słowa
         */
        public int? WordNumber
        {
            get { return _wordNumber; }
            set { _wordNumber = value; }
        }

        /**
         * Ustawienie jako pierwsza litera
         */
        public bool FirstLetter
        {
            get { return _isFirstLetter; }
            set { _isFirstLetter = value; }
        }

        /**
         * Zgadnięcie słowa
         */
        public bool GuessLetter(char letter)
        {
            if (_letter == char.ToUpper(letter))
            {
                _visible = true;
                return true;
            }
            return false;
        }
    }
}
