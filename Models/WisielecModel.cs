using System.Web;

namespace Gry_Slownikowe.Models
{
    public class WisielecModel
    {
        public string slowo { get; set; }

        public WisielecModel(string slowo)
        {
            this.slowo = slowo;
        }
        public string Losuj() {
            bool isCorrect = false;
            SJP_API api = new SJP_API();
            do
            {
                api.losuj_w_obiekcie();
                isCorrect = api.getDopuszczalnosc();
            } while (isCorrect == false);

            slowo = api.getSlowo();
            slowo = HttpUtility.HtmlEncode(slowo);
            return slowo;
        }
    }
}
