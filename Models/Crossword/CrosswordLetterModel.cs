namespace CrosswordComponents
{
    /**
     * Klasa reprezentująca kafelek (literkę) krzyżówki
     */
    public class CrosswordLetterModel
    {

        private int? _wordNumber = null;

        private bool _visible = false; 

        private readonly char _letter;

        private bool _isFirstLetter = false;

        private bool _isClueLetter = false;

        /**
         * Konstruktor
         */
        public CrosswordLetterModel(char letter) 
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
            set {

                if (_isClueLetter)
                {
                    throw new InvalidOperationException("First letter can not be clue letter");
                }

                _isFirstLetter = value; 
            
            }
        }

        public void ResetFirstOrClueLetter()
        {
            _isClueLetter = false;
            _isFirstLetter = false;
            _wordNumber = null;
        }

        public bool ClueLetter
        {
            get { return _isClueLetter; }
            set {

                if (_isFirstLetter)
                {
                    throw new InvalidOperationException("Clue letter can not be first letter");
                }
                _isClueLetter = value; 
            
            }
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
