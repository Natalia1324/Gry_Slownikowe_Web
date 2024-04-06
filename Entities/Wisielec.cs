using Gry_Słownikowe.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gry_Słownikowe.Entions
{
    public class Wisielec
    {
        public int Id { get; set; }
        public string Nick { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Ranks { get; set; }

    }
}
