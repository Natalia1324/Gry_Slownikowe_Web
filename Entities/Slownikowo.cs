using Gry_Slownikowe.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gry_Slownikowe.Entions
{
    public class Slownikowo
    {
        public int Id { get; set; }
        //0 - lose, 1 - win
        public bool Win { get; set; }
        public int Tries { get; set; }
        public TimeSpan GameTime { get; set; }
        public DateTime? GameData { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
       
    }
}
