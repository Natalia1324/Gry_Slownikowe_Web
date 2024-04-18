using DynamicMatrix;

namespace CrosswordComponents
{
    public class CrosswordPlacementModel
    {
        public enum DirectionType
        {
            HORIZONTAL,
            VERTICAL
        }
        private readonly DirectionType _direction;
        private readonly int _rowIndex;
        private readonly int _columnIndex;
        private readonly string _word;
        private readonly int _wordNumber;


        public CrosswordPlacementModel(int rowIndex, int columnIndex, DirectionType direction, string word, int wordNumber)
        {
            _rowIndex = rowIndex;
            _columnIndex = columnIndex;
            _word = word;
            _wordNumber = wordNumber;
            _direction = direction;
        }
           
        public DirectionType Direction
        {
            get
            {
                return _direction;
            }
        }

        public int WordNumber
        {
            get
            {
                return _wordNumber;
            }
        }

        public void PlaceWord(DynamicMatrix<CrosswordLetterModel> crossword)
        {
            int tempRowIndex = _rowIndex;
            int tempColumnIndex = _columnIndex;
            CrosswordLetterModel firstLetter = new CrosswordLetterModel(_word[0]);
            firstLetter.FirstLetter = true;
            firstLetter.WordNumber = _wordNumber;
            if(Direction == DirectionType.HORIZONTAL)
            {
                if(_columnIndex < 0)
                {
                    crossword[tempRowIndex, tempColumnIndex] = firstLetter;
                    tempColumnIndex = 1;
                }
                else
                {
                    crossword[tempRowIndex, tempColumnIndex++] = firstLetter;
                }
                for(int i = 1; i < _word.Length; i++)
                {
                    if (crossword[tempRowIndex, tempColumnIndex] != null && crossword[tempRowIndex, tempColumnIndex].FirstLetter)
                    {
                        tempColumnIndex++;
                    }
                    else
                    {
                        crossword[tempRowIndex, tempColumnIndex++] = new CrosswordLetterModel(_word[i]);
                    }
                }
            }
            else if(Direction == DirectionType.VERTICAL)
            {
                if(_rowIndex < 0)
                {
                    crossword[tempRowIndex, tempColumnIndex] = firstLetter;
                    tempRowIndex = 1;
                }
                else
                {
                    crossword[tempRowIndex++, tempColumnIndex] = firstLetter;
                }
                for(int i = 1; i < _word.Length; i++)
                {
                    if (crossword[tempRowIndex, tempColumnIndex] != null && crossword[tempRowIndex, tempColumnIndex].FirstLetter)
                    {
                        tempRowIndex++;
                    }
                    else
                    {
                        crossword[tempRowIndex++, tempColumnIndex] = new CrosswordLetterModel(_word[i]);
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Something gone wrong");
            }
        }

    }
}
