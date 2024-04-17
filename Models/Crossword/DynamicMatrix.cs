namespace Gry_Słownikowe.Models.Crossword
{
    /**
     * Implementacja tablicy dwuwymiarowej automatycznie się rozszerzającej
     */
    public class DynamicMatrix<T>
    {
        private T[] _data;

        private int _numberOfRows;

        private int _numberOfColumns;

        /**
         * Konstruktor tablicy o rozmiarze [y:x] -> [0:0]
         */
        public DynamicMatrix()
        {
            _data = new T[0];
            _numberOfRows = 0;
            _numberOfColumns = 0;
        }
        /**
         * Konstruktor tablicy o zadanym rozmiarze [y:x]
         */
        public DynamicMatrix(int rows, int columns)
        {
            _data = new T[rows * columns];
            _numberOfRows = rows;
            _numberOfColumns = columns;
        }
        /**
         * Konstruktor kopiujący
         */
        public DynamicMatrix(DynamicMatrix<T> other)
        {
            _numberOfRows = other.Rows;
            _numberOfColumns = other.Columns;
            int size = _numberOfRows * _numberOfColumns;
            _data = new T[size];

            for (int i = 0; i < size; i++)
            {
                _data[i] = other[i];
            }
        }

        public int Size
        {
            get { return _data.Length; }
        }

        /**
         * Ilość rzędów
         */
        public int Rows
        {
            get
            {
                return _numberOfRows;
            }
        }

        /**
         * Ilość kolumn
         */
        public int Columns
        {
            get
            {
                return _numberOfColumns;
            }
        }

        public bool IsLastIndexInRow(int index)
        {
            if (_numberOfColumns == 0) return false;
            return (index + 1) % _numberOfColumns == 0;
        }

        /**
         * Obliczenie indexu rzędu
         */
        private int CalculateRowIndex(int index)
        {
            if (_numberOfColumns == 0) return 0;
            return index / _numberOfColumns;
        }

        /**
         * Obliczenie indexu kolumny
         */
        private int CalculateColumnIndex(int index)
        {
            if (_numberOfColumns == 0) return 0;
            return (index % _numberOfColumns);
        }

        /**
         * Obliczenie indexu dla nowego rozmiaru
         */
        private int CalculateShiftIndex(int rowIndex, int columnIndex, int newNumberOfColumns)
        {
            return rowIndex * newNumberOfColumns + columnIndex;
        }

        /**
         * Oblczeniie indexu
         */
        private int calculateIndex(int rowIndex, int columnIndex)
        {
            return rowIndex * _numberOfColumns + columnIndex;
        }

        /**
         * Dostęp do elementu według indexu (bez poszerzania)
         */
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _data.Length)
                {
                    throw new NullReferenceException("Index out of range.");
                }
                return _data[index];
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException($"Value of {typeof(T)} was null");
                }
                _data[index] = value;
            }
        }

        /**
         * Dostęp do elementu i poszerzanie tablicy wg. indeksu
         */
        public T this[int rowIndex, int columnIndex]
        {
            get
            {
                return this[calculateIndex(rowIndex, columnIndex)];
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException($"Value of {typeof(T)} was null");
                }
                if (rowIndex >= 0 && columnIndex >= 0 && rowIndex < _numberOfRows && columnIndex < _numberOfColumns)
                {
                    this[calculateIndex(rowIndex, columnIndex)] = value;
                    return;
                }
                int newNumberOfColumns, newNumberOfRows;
                int rowShift = 0, colShift = 0;

                if (columnIndex < 0 && rowIndex < 0)
                {
                    colShift = Math.Abs(columnIndex);
                    rowShift = Math.Abs(rowIndex);
                    if (_numberOfColumns == 0 || _numberOfRows == 0)
                    {
                        newNumberOfColumns = _numberOfColumns + colShift + 1;
                        newNumberOfRows = _numberOfRows + rowShift + 1;
                    }
                    else
                    {
                        newNumberOfColumns = _numberOfColumns + colShift;
                        newNumberOfRows = _numberOfRows + rowShift;
                    }

                }
                else if (columnIndex < 0)
                {
                    colShift = Math.Abs(columnIndex);
                    if (_numberOfColumns == 0)
                    {
                        newNumberOfColumns = _numberOfColumns + colShift + 1;
                    }
                    else
                    {
                        newNumberOfColumns = _numberOfColumns + colShift;
                    }
                    newNumberOfRows = Math.Max(rowIndex + 1, _numberOfRows);
                }
                else if (rowIndex < 0)
                {
                    rowShift = Math.Abs(rowIndex);
                    if (_numberOfRows == 0)
                    {
                        newNumberOfRows = _numberOfRows + rowShift + 1;
                    }
                    else
                    {
                        newNumberOfRows = _numberOfRows + rowShift;
                    }
                    newNumberOfColumns = Math.Max(columnIndex + 1, _numberOfColumns);
                }
                else
                {
                    newNumberOfColumns = Math.Max(columnIndex + 1, _numberOfColumns);
                    newNumberOfRows = Math.Max(rowIndex + 1, _numberOfRows);
                }

                T[] newArray = new T[newNumberOfColumns * newNumberOfRows];

                for (int i = 0; i < _data.Length; i++)
                {
                    int tempRowIndex = CalculateRowIndex(i);
                    int tempColumnIndex = CalculateColumnIndex(i);
                    newArray[CalculateShiftIndex(tempRowIndex + rowShift, tempColumnIndex + colShift, newNumberOfColumns)]
                        = _data[calculateIndex(tempRowIndex, tempColumnIndex)];
                }

                _data = newArray;
                _numberOfColumns = newNumberOfColumns;
                _numberOfRows = newNumberOfRows;
                _data[calculateIndex(rowIndex + rowShift, columnIndex + colShift)] = value;
            }
        }
    }
}


