using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicArray
{
    /**
     * Implementacja tablicy dwuwymiarowej automatycznie się rozszerzającej
     */
    public class DynamicArray2D<T>
    {
        private T[,] _array;

        /**
         * Konstruktor tablicy o rozmiarze [y:x] -> [0:0]
         */
        public DynamicArray2D()
        {
            _array = new T[0, 0];
        }
        /**
         * Konstruktor tablicy o zadanym rozmiarze [y:x]
         */
        public DynamicArray2D(int rows, int columns)
        {
            _array = new T[rows, columns];
        }
        /**
         * Konstruktor kopiujący
         */
        public DynamicArray2D(DynamicArray2D<T> other)
        {

            int rows = other.Rows;
            int columns = other.Columns;

            _array = new T[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    _array[i, j] = other[i, j];
                }
            }
        }
        /**
         * Dostęp do elementu i poszerzanie tablicy wg. indeksu
         */
        public T this[int y, int x]
        {
            get
            {
                return _array[y, x];
            }
            set
            {
                int nRows, nColumns;
                int rowShift = 0, colShift = 0;

                if (x < 0 && y < 0)
                {
                    colShift = Math.Abs(x);
                    rowShift = Math.Abs(y);
                    if (Columns == 0 || Rows == 0)
                    {
                        rowShift += 1;
                        colShift += 1;
                    }
                    nColumns = Columns + colShift;
                    nRows = Rows + rowShift;
                }
                else if (x < 0)
                {

                    colShift = Math.Abs(x);
                    if (Columns == 0)
                    {
                        colShift += 1;
                    }
                    nRows = Math.Max(y + 1, Rows);
                    nColumns = Columns + colShift;
                }
                else if (y < 0)
                {
                    rowShift = Math.Abs(y);
                    if (Rows == 0)
                    {
                        rowShift += 1;
                    }
                    nRows = Rows + rowShift;
                    nColumns = Math.Max(x + 1, Columns);
                }
                else
                {
                    nRows = Math.Max(y + 1, Rows);
                    nColumns = Math.Max(x + 1, Columns);
                }

                if (nRows != Rows || nColumns != Columns)
                {
                    T[,] newArray = new T[nRows, nColumns];

                    for (int i = 0; i < Rows; i++)
                    {
                        for (int j = 0; j < Columns; j++)
                        {
                            newArray[i + rowShift, j + colShift] = _array[i, j];
                        }
                    }

                    _array = newArray;
                }

                _array[y + rowShift, x + colShift] = value;
            }
        }

        /**
         * Ilość rzędów
         */
        public int Rows
        {
            get { return _array.GetLength(0); }
        }
        /**
         * Ilość kolumn
         */
        public int Columns
        {
            get { return _array.GetLength(1); }
        }
    }
}
