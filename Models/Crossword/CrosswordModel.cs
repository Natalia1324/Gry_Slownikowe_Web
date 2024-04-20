using DynamicMatrix;
using CrosswordComponents;
using Gry_Słownikowe.Models;
using DynamicArray;
using System.Diagnostics;


namespace Crossword
{
    /**
     * Klasa reprezentująca model krzyżówki
     */
    internal class CrosswordModel : ICrosswordModelReadOnly
    {

        /**
         * Słownik słów wraz z ich znaczeniami
         */
        private readonly List<string> _meanings;

        /**
         * Obiekt krzyżówki jako dynamiczna macierz mxn
         */
        private DynamicMatrix<CrosswordLetterModel> _crossword;

        private Stopwatch _stopwatch;

        /**
         * Ilość słów w krzyżówce
         */

        private int _wordsNumber = 0;

        
        /**
         * Dostęp do ilości liter w krzyżowce
         */
        public int Letters {  
            get {
                int letters = 0;
                for(int i = 0; i < _crossword.Size; i++)
                {
                    if (_crossword[i] != null)
                        letters++;
                }
                return letters;
            } 
        }
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
         * Dostęp do ilości słów
         */
        public int Words
        {
            get
            {
                return _wordsNumber;
            }
        }

        /**
         * Dostęp do odczytu komórki
         */
        public CrosswordLetterModel this[int y, int x]
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
            _crossword = new DynamicMatrix<CrosswordLetterModel>();
            _meanings = new List<string>();
            _stopwatch = new Stopwatch();
        }
        /**
         * Metoda zwracająca ilość odgadniętych liter
         */
        public int GetGuessedLetters()
        {
            int count = 0;
            for(int i = 0; i < _crossword.Size; i++)
            {
                if (_crossword[i] != null && _crossword[i].Visible)
                {
                    count++;
                }
            }
            return count;
        }

        /**
         * Otrzymaj czas wygenerowania krzyżówki
         */
        public double GetGenerationTimeInMils()
        {
            return Math.Round(_stopwatch.Elapsed.TotalMilliseconds, 2);
        }

        public void StartTimer()
        {
            _stopwatch.Restart();
        }

        /**
         * Otrzymaj czas rozwiązywania krzyżówki
         */
        public double GetTime()
        {
            double elapsedSeconds = _stopwatch.Elapsed.TotalSeconds;

            double roundedSeconds = Math.Round(elapsedSeconds, 2);

            return roundedSeconds;
        }


        /**
         * Metoda wstawiająca słowo
         */
        internal bool InsertWord(string word, string meaning)
        {
            _stopwatch.Start();
            if (word == null || word.Length == 0 || meaning == null || meaning.Length == 0)
            {
                _stopwatch.Stop();
                return false;
            }

            word = word.ToUpper();
            _wordsNumber++;
            if (_wordsNumber == 1)
            {
                CrosswordPlacementModel cWP = new CrosswordPlacementModel(0, 0, 
                    CrosswordPlacementModel.DirectionType.HORIZONTAL, word, _wordsNumber);
                cWP.PlaceWord(_crossword);
            }
            else
            {          
                bool anyWordInserted = false;
                List<CrosswordPlacementModel> placements = new List<CrosswordPlacementModel>();
                for(int letterIndex = 0; letterIndex < word.Length; letterIndex++)
                {
                    for(int crosswordIndex = 0; crosswordIndex < _crossword.Size; crosswordIndex++)
                    {
                        CrosswordLetterModel tempLetter = _crossword[crosswordIndex];
                        if (tempLetter != null && tempLetter.Value == word[letterIndex] && !tempLetter.FirstLetter)
                        {
                            CrosswordPlacementModel tempPlacement = FindPlacement(crosswordIndex, word, letterIndex);
                            if (tempPlacement != null)
                            {
                                anyWordInserted = true;
                                placements.Add(tempPlacement); 
                            }
                                
                        }
                    }
                }
                if (!anyWordInserted)
                {
                    _wordsNumber--;
                    _stopwatch.Stop();
                    return false;
                }
                else
                {
                    getBestCrossword(placements);
                }
            }
            _meanings.Add(meaning);
            _stopwatch.Stop();
            return true;
        }

        /**
         * Kod odpowiedzialny za wybór najlepszej krzyzówki
         */
        private void getBestCrossword(List<CrosswordPlacementModel> placements) {
            float bestScore = 0;
            DynamicMatrix<CrosswordLetterModel> bestCrossword = new DynamicMatrix<CrosswordLetterModel>(_crossword);
            foreach (CrosswordPlacementModel placement in placements)
            {
                DynamicMatrix<CrosswordLetterModel> newCrossword = new DynamicMatrix<CrosswordLetterModel>(_crossword);
                placement.PlaceWord(newCrossword);
                float newScore = GetCrosswordScore(newCrossword);
                if (newScore > bestScore)
                {
                    bestScore = newScore;
                    bestCrossword = newCrossword;
                }
            }
            if (bestScore > 0)
            {
                _crossword = bestCrossword;
            }
        }


        /**
         * Metoda sprawdzająca czy można wstawić słowo, jak tak to umiejscowienie do listy
         */
        private CrosswordPlacementModel FindPlacement(int crosswordIndex, string word, int letterIndex)
        {
            int rowIndex = _crossword.CalculateRowIndex(crosswordIndex);
            int columnsIndex = _crossword.CalculateColumnIndex(crosswordIndex);
            if (CheckVertical(rowIndex, columnsIndex, word, letterIndex))
            {
                return new CrosswordPlacementModel(rowIndex-letterIndex,columnsIndex, 
                    CrosswordPlacementModel.DirectionType.VERTICAL ,word, _wordsNumber);

            }
            else if(CheckHorizontal(rowIndex, columnsIndex, word, letterIndex))
            {
                return new CrosswordPlacementModel(rowIndex, columnsIndex - letterIndex,
                    CrosswordPlacementModel.DirectionType.HORIZONTAL, word, _wordsNumber);
            }
            else
            {
                return default;
            }
        }
        /**
         * Sprawdzenie w poziomie
         */
        private bool CheckVertical(int rowIndex, int columnIndex, string word, int letterIndex)
        {
            int numberOfRows = _crossword.Rows;
            int numberOfColumns = _crossword.Columns;
            int startRowIndex = rowIndex - letterIndex;

            if(startRowIndex  < 0)
            {
                letterIndex = Math.Abs(startRowIndex);
                startRowIndex = 0;
                
            }
            else
            {
                letterIndex = 0;
            }

            // przed pętlą
            if (startRowIndex - 1 > 0 && _crossword[startRowIndex - 1, columnIndex] != null)
            {
                return false;
            }

            //w tym miejscu na pewno jesteśmy po prawej od lewej strony krawędzi.
            for (; letterIndex < word.Length; letterIndex++)
            {

                CrosswordLetterModel temp = _crossword[startRowIndex, columnIndex];
                if (temp != null && temp.Value != word[letterIndex])
                    return false;

                if (temp == null)
                {
                    if ((columnIndex > 0 && _crossword[startRowIndex, columnIndex - 1] != null) || (columnIndex < numberOfColumns - 1 && _crossword[startRowIndex, columnIndex + 1] != null))
                        return false;

                    if ((columnIndex == 0 && _crossword[startRowIndex, columnIndex + 1] != null) || (columnIndex == numberOfColumns - 1 && _crossword[startRowIndex, columnIndex - 1] != null))
                        return false;
                }

                if (startRowIndex >= _crossword.Rows) //continue;
                    return true;

                startRowIndex++;
            }

            //wycinek startx = word.Length za pętlą, startx w tym miejscu jest równe word.Length, zatem nie inkrementuje 
            return startRowIndex >= numberOfRows || _crossword[startRowIndex, columnIndex] == null;
        }
        /**
         * Sprawdzenie w pionie
         */
        private bool CheckHorizontal(int rowIndex, int columnIndex, string word, int letterIndex)
        {
            int numberOfRows = _crossword.Rows;
            int numberOfColumns = _crossword.Columns;
            int startColumnIndex = columnIndex - letterIndex;

            if (startColumnIndex < 0)
            {
                letterIndex = Math.Abs(startColumnIndex);
                startColumnIndex = 0;
            }
            else
            {
                letterIndex = 0;
            }

            if (startColumnIndex - 1 > 0 && _crossword[rowIndex, startColumnIndex - 1] != null)
            {
                return false;
            }

            for (; letterIndex < word.Length; letterIndex++)
            {
                
                CrosswordLetterModel temp = _crossword[rowIndex, startColumnIndex];
                if (temp != null && temp.Value != word[letterIndex])
                    return false;

                if (temp == null)
                {

                    if ((rowIndex > 0 && _crossword[rowIndex - 1, startColumnIndex] != null) || (rowIndex < numberOfRows - 1 && _crossword[rowIndex + 1, startColumnIndex] != null))
                        return false;

                    if ((rowIndex == 0 && _crossword[rowIndex + 1, startColumnIndex] != null) || (rowIndex == numberOfRows - 1 && _crossword[rowIndex - 1, startColumnIndex] != null))
                        return false;
                }

                if (startColumnIndex >= _crossword.Columns) //continue;
                    return true;

                startColumnIndex++;
            }

            return startColumnIndex >= numberOfColumns || _crossword[rowIndex, startColumnIndex] == null;
        }
 
        /**
         * Metoda obliczająca wynik dla każdej krzyżówki (jej optymalność)
         */
        private float GetCrosswordScore(DynamicMatrix<CrosswordLetterModel> crossword)
        {
            int crosswordRows = crossword.Rows;
            int crosswordColumns = crossword.Columns;
            if (crosswordRows == 0 || crosswordColumns == 0) return 0;

            int filled = 0;
            int empty = 0;
            float sizeRatio = crosswordColumns / (float)crosswordRows;
            if (sizeRatio > 1)
            {
                sizeRatio = crosswordRows / (float)crosswordColumns;
            }
            for(int i = 0; i < crossword.Size; i++)
            {
                if (crossword[i] == null)
                {
                    empty++;
                }
                else
                {
                    filled++;
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
            for(int i = 0; i < _crossword.Size; i++)
            {
                if (_crossword[i] == null)
                {
                    Console.Write($"   ");
                }
                else
                {
                    if (_crossword[i].Visible)
                    {
                        Console.Write($"[{_crossword[i].Value}]");
                    }
                    else
                    {
                        Console.Write($"[?]");
                    }

                }
                if (_crossword.IsLastIndexInRow(i))
                {
                    Console.WriteLine();
                }
            }
           
        }
    }

}

