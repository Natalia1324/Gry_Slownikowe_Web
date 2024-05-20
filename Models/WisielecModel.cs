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
            SJP_API api = new SJP_API();
            slowo = api.getSlowo();
            slowo = HttpUtility.HtmlEncode(slowo);
            return slowo;
        }
    }
}
