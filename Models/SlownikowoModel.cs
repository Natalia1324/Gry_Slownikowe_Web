namespace Gry_Słownikowe.Models
{
    public class SlownikowoModel
    {
        public SlownikowoModel(string slowo)
        {
            WylosowaneSlowo = slowo;
        }
        public string WylosowaneSlowo { get; set; }
    }
}
