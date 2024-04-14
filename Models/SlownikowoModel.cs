namespace Gry_Słownikowe.Models
{
    public class SlownikowoModel
    {
        public SlownikowoModel(string slowo, SJP_API api)
        {
            WylosowaneSlowo = slowo;
            this.api = api;
        }
        
        public void changeApi(SJP_API api)
        {
            this.api = api;
        }
        public SJP_API api { get; set; }
        public string WylosowaneSlowo { get; set; }
    }
}
