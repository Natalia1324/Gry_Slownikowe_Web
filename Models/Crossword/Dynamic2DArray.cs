using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicArray
{
    /**
     * Implementacja tablicy dwuwymiarowej automatycznie się rozszerzającej
     */
    public class Dynamic2DArray<T>
    {
        private T[,] _array;

        /**
         * Konstruktor tablicy o rozmiarze [y:x] -> [0:0]
         */
        public Dynamic2DArray() 
        {
            _array = new T[0,0];
        }
        /**
         * Konstruktor tablicy o zadanym rozmiarze [y:x]
         */
        public Dynamic2DArray(int rows, int columns)
        {
            _array = new T[rows, columns];
        }
        /**
         * Konstruktor kopiujący
         */
        public Dynamic2DArray(Dynamic2DArray<T> other)
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

                if (x < 0)
                {             
                    colShift = Math.Abs(x);
                    nRows = Math.Max(y + 1, Rows);
                    nColumns = Columns + colShift + 1;
                }else if(y < 0)
                {
                    rowShift = Math.Abs(y);
                    nRows = Rows + rowShift + 1;
                    nColumns = Math.Max(x + 1, Columns);
                }else if(x < 0 && y < 0)
                {
                    colShift = Math.Abs(x);
                    rowShift = Math.Abs(y);
                    nColumns = Columns + colShift + 1;
                    nRows = Rows + rowShift + 1;
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
            get { return _array.GetLength(0);}
        }
        /**
         * Ilość kolumn
         */
        public int Columns
        {
            get { return _array.GetLength(1);}
        }
    }
}
