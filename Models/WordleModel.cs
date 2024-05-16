namespace Gry_Słownikowe.Models
{
    public class WordleModel
    {
        public string slowo { get; set; }
        public string znaczenia { get; set; }
        public int dlugosc { get; set; }

        public WordleModel(string slowo, string znaczenia, int dlugosc)
        {
            this.slowo = slowo;
            this.znaczenia = znaczenia;
            this.dlugosc = dlugosc;
        }
    }
}
