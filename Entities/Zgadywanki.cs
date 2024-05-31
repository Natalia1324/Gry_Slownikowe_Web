using Gry_Slownikowe.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gry_Slownikowe.Entions
{
    public class Zgadywanki
    {
        public int Id { get; set; }
        public int Punkty { get; set; }
        public TimeSpan GameTime { get; set; }

        public User User { get; set; }
        public int UserId { get; set; }
        
    }
}
