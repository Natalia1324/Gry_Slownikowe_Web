namespace Gry_Słownikowe.Models
{
    /**
     * Klasa reprezentująca instancję krzyżówki.
     * 
     * Obsługuje wstawianie słów i inne operacje
     */
    internal class CrosswordModel
    {
        /*
         * rozwiązanie tymczasowe, zastąpie inną strukturą
         */
        List<List<char>> _wordsPane;

        internal CrosswordModel()
        {
            _wordsPane = new List<List<char>>();
        }
        internal bool PlaceWord(string word, int cordX, int cordY)
        {
            //TODO
            return true;
        }
    }
}
