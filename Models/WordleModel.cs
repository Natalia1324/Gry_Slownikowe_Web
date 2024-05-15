namespace Gry_Slownikowe.Models
{
    public class WordleModel
    {
        public string slowo {  get; set; }  
        public string znaczenia { get; set; } 

        public WordleModel(string slowo, string znaczenia)
        {
            this.slowo = slowo;
            this.znaczenia = znaczenia;
        }
    }
}
