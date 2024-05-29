namespace Gry_Slownikowe.Models
{
    public class ZgadywankiModel
    {
        public string Slowo1 { get; set; }
        public string Slowo2 { get; set; }

        public string Slowo3 { get; set; }

        public string Slowo4 { get; set; }

        public ZgadywankiModel(string slowo1, string slowo2, string slowo3, string slowo4)
        {
            Slowo1 = slowo1;
            Slowo2 = slowo2;
            Slowo3 = slowo3;
            Slowo4 = slowo4;
        }
    }
}
