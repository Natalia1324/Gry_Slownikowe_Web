namespace Gry_Slownikowe.Models
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
