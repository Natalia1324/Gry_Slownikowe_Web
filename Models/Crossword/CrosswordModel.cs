using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicArray;
using CrosswordComponents;
using Gry_Słownikowe.Models;

namespace Crossword
{
    /**
     * Klasa reprezentująca model krzyżówki
     */
    internal class CrosswordModel : ICrosswordModelReadOnly
    {
        /**
         * Struktura reprezentująca możliwe położenie słowa
         */
        private struct Placement
        {
            private bool _horizontal;
            private int _x, _y;
            public Placement(int x, int y, bool isHorizontal)
            {
                _horizontal = isHorizontal;
                _x = x; 
                _y = y;
            }
            public bool IsHorizontal {  get { return _horizontal; } }
            public int X { get { return _x; } }
            public int Y { get { return _y; } }

        }
        /**
         * Słownik słów wraz z ich znaczeniami
         */
        private readonly List<string> _meanings;
        /**
         * Obiekt krzyżówki jako dynamiczna tablica 2d
         */
        private Dynamic2DArray<CrosswordTile> _crossword;

        /**
         * Ilość liter krzyżówki
         */
        private int _lettersTotal = 0;
        
        /**
         * Dostęp do ilości liter w krzyżowce
         */
        public int Letters {  get { return _lettersTotal; } }
        /**
         * Wiersze krzyżówki
         */
        public int Rows
        {
            get { return _crossword.Rows; }
        }
        /**
         * Kolumny krzyżówki
         */
        public int Columns
        {
            get { return _crossword.Columns; }
        }
        /**
         * Dostęp do odczytu komórki
         */
        public CrosswordTile this[int y, int x]
        {
            get
            {
                return _crossword[y, x];
            }
        }
        /**
         * Otrzymaj listę opisów słów
         */
        public List<string> GetDescriptions()
        {
            return _meanings;
        }
        /**
         * Konstruktor
         */
        internal CrosswordModel()
        {
            _crossword = new Dynamic2DArray<CrosswordTile>();
            _meanings = new List<string>();
        }
        /**
         * Metoda zwracająca ilość odgadniętych liter
         */
        public int GetGuessedLetters()
        {
            int count = 0;
            for(int i = 0; i < _crossword.Columns;  i++)
            {
                for(int j = 0;  j < _crossword.Rows; j++)
                {
                    if (_crossword[j, i] != null && _crossword[j, i].Visible)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        /**
         * Metoda wstawiająca słowo
         */
        internal bool InsertWord(string word, string meaning)
        {
            word = word.ToUpper();
            bool wordInserted = false;
            if (_meanings.Count == 0)
            {
                _crossword[0, 0] = new CrosswordTile(word[0]);
                _crossword[0, 0].FirstLetter = true;
                _crossword[0, 0].WordNumber = 1;
                for (int x = 1; x < word.Length; x++)
                {
                    _crossword[0,x] = new CrosswordTile(word[x]);
                    _lettersTotal++;
                }
                _meanings.Add(meaning);
                return true;
            }
            else
            {
                List<Placement> placements = new List<Placement>();
                for(int letterIndex = 0; letterIndex < word.Length; letterIndex++)
                {
                    for (int y = 0; y < _crossword.Rows; y++)
                    {
                        for (int x = 0; x < _crossword.Columns; x++)
                        {
                            if (_crossword[y, x] != null && _crossword[y, x].Value == word[letterIndex] && !_crossword[y, x].FirstLetter)
                            {
                                if (FindPlacement(x, y, word, letterIndex, placements))
                                    wordInserted = true;
                            }
                        }
                    }
                }
                if (!wordInserted) return false;
                _meanings.Add(meaning);
                float bestScore = 0;
                Dynamic2DArray<CrosswordTile> bestBoard = new Dynamic2DArray<CrosswordTile>(_crossword);
                foreach (Placement placement in placements)
                {
                    Dynamic2DArray<CrosswordTile> newBoard = new Dynamic2DArray<CrosswordTile>(_crossword);
                    PlaceWord(newBoard, placement, word);
                    float newScore = GetCrosswordScore(newBoard);
                    if(word == "koooka")
                    {
                        Console.WriteLine(newScore);
                        for (int y = 0; y < newBoard.Rows; y++)
                        {
                            for (int x = 0; x < newBoard.Columns; x++)
                            {
                                if (newBoard[y, x] == null)
                                {
                                    Console.Write($"[ ]");
                                }
                                else
                                {
                                    Console.Write($"[{newBoard[y, x].Value}]");
                                }

                            }
                            Console.WriteLine("");
                        }
                    }
                    if(newScore > bestScore) 
                    {
                        bestScore = newScore;
                        bestBoard = newBoard;
                    }
                }
                if(bestScore > 0)
                {
                    _crossword = bestBoard;
                }
            }
            return wordInserted;
        }
        /**
         * Metoda sprawdzająca czy można wstawić słowo, jak tak to umiejscowienie do listy
         */
        private bool FindPlacement(int letterX, int letterY, string word, int letterIndex, List<Placement> placements)
        {
            if(CheckVertical(letterX, letterY, word, letterIndex))
            {
                placements.Add(new Placement(letterX, letterY - letterIndex, false));
                return true;
            }
            else if(CheckHorizontal(letterX, letterY, word, letterIndex))
            {
                placements.Add(new Placement(letterX - letterIndex, letterY, true));
                return true;
            }
            else
            {
                return false;
            }
        }
        /**
         * Sprawdzenie w poziomie
         */
        private bool CheckHorizontal(int letterX, int letterY, string word, int letterIndex)
        {
            int startX = letterX - letterIndex;

            if(startX  < 0)
            {
                letterIndex = Math.Abs(startX);
                startX = 0;
                
            }
            else
            {
                letterIndex = 0;
            }

            //wycinek startx - 1 przed pętlą
            if (startX - 1 > 0 && _crossword[letterY, startX - 1] != null)
            {
                return false;
            }

            //w tym miejscu na pewno jesteśmy po prawej od lewej strony krawędzi.
            for (; letterIndex < word.Length; letterIndex++)
            {
                if (startX >= _crossword.Columns) //continue;
                    return true; 

                if (_crossword[letterY, startX] != null && _crossword[letterY, startX].Value != word[letterIndex])
                    return false;

                if (_crossword[letterY, startX] == null)
                {
                    if ((letterY > 0 && _crossword[letterY - 1, startX] != null) || (letterY < _crossword.Rows - 1 && _crossword[letterY + 1, startX] != null))
                        return false;

                    if ((letterY == 0 && _crossword[letterY + 1, startX] != null) || (letterY == _crossword.Rows - 1 && _crossword[letterY - 1, startX] != null))
                        return false;
                }

                startX++;
            }

            //wycinek startx = word.Length za pętlą, startx w tym miejscu jest równe word.Length, zatem nie inkrementuje 
            if (startX < _crossword.Columns && _crossword[letterY, startX] != null)
            {
                return false;
            }

            return true;
        }
        /**
         * Sprawdzenie w pionie
         */
        private bool CheckVertical(int letterX, int letterY, string word, int letterIndex)
        {
            int startY = letterY - letterIndex;

            if (startY < 0)
            {
                letterIndex = Math.Abs(startY);
                startY = 0;
            }
            else
            {
                letterIndex = 0;
            }

            if (startY - 1 > 0 && _crossword[startY - 1, letterX] != null)
            {
                return false;
            }

            for (; letterIndex < word.Length; letterIndex++)
            {
                if (startY >= _crossword.Rows) //continue;
                    return true; 

                if (_crossword[startY, letterX] != null && _crossword[startY, letterX].Value != word[letterIndex])
                    return false;

                if (_crossword[startY, letterX] == null)
                {

                    if ((letterX > 0 && _crossword[startY, letterX - 1] != null) || (letterX < _crossword.Columns - 1 && _crossword[startY, letterX + 1] != null))
                        return false;

                    if ((letterX == 0 && _crossword[startY, letterX + 1] != null) || (letterX == _crossword.Columns - 1 && _crossword[startY, letterX - 1] != null))
                        return false;
                }

                startY++;
            }

            if (startY < _crossword.Rows && _crossword[startY, letterX] != null)
            {
                return false;
            }

            return true;
        }
        /**
         * Metoda do wstawiania słowa
         */
        private void PlaceWord(Dynamic2DArray<CrosswordTile> crossword, Placement placement, string word)
        {
            int x = placement.X;
            int y = placement.Y;
            CrosswordTile firstLetter = new CrosswordTile(word[0]);
            firstLetter.FirstLetter = true;
            firstLetter.WordNumber = _meanings.Count;
            if (placement.IsHorizontal)
            {
                firstLetter.Direction = CrosswordTile.WordDirection.HORIZONTAL;

                if (x < 0)
                {
                    crossword[y, x] = firstLetter;
                    x = 1;
                }
                else
                {
                    crossword[y, x++] = firstLetter;
                }

                for (int i = 1; i < word.Length; i++)
                {
                    crossword[y, x++] = new CrosswordTile(word[i]);
                    _lettersTotal++;
                }
            }
            else
            {
                firstLetter.Direction = CrosswordTile.WordDirection.VERTICAL;
                if (y < 0)
                {
                    crossword[y, x] = firstLetter;
                    y = 1;
                }
                else
                {
                    crossword[y++, x] = firstLetter;
                }
                for(int i = 1; i < word.Length; i++) 
                {
                    crossword[y++, x] = new CrosswordTile(word[i]);
                    _lettersTotal++;
                }
 
            }
        }
        /**
         * Metoda obliczająca wynik dla każdej krzyżówki (jej optymalność)
         */
        private float GetCrosswordScore(Dynamic2DArray<CrosswordTile> crossword)
        {
            float sizeRatio;
            int filled = 0;
            int empty = 0;
            if (crossword.Rows > crossword.Columns)
                sizeRatio = crossword.Columns / (float)crossword.Rows;
            else
                sizeRatio = crossword.Rows / (float)crossword.Columns;
            for (int y = 0; y < crossword.Rows; y++)
            {
                for (int x = 0; x < crossword.Columns; x++)
                {
                    if (crossword[y,x] == null)
                    {
                        empty++;
                    }
                    else
                    {
                        filled++;
                    }
                }
            }
            
            float filledRatio = filled / (float)empty;
            return(sizeRatio * 10) + (filledRatio * 20);
        }
        /**
         * Metoda wypisująca krzyżówkę
         */
        public void PrintCrosswordInConsole()
        {
            for (int y = 0; y < _crossword.Rows; y++)
            {
                for (int x = 0; x < _crossword.Columns; x++)
                {
                    if(_crossword[y, x] == null)
                    {
                        Console.Write($"   ");
                    }
                    else
                    {
                        if(_crossword[y, x].Visible)
                        {
                            Console.Write($"[{_crossword[y, x].Value}]");
                        }
                        else
                        {
                            Console.Write($"[?]");
                        }
                        
                    }
                    
                }
                Console.WriteLine("");
            }
        }
    }

}

