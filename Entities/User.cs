using Gry_Slownikowe.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gry_Slownikowe.Entions
{
    public class User
    {
        public int Id { get; set; }
        public string Nick { get; set; } // = new string("Jan Kowalski");
        public string Login { get; set; }
        public string Password { get; set; }
        public int Ranks { get; set; }
        public DateTime AccountCreationDate { get; set; }

        
        public ICollection<Krzyzowki> Krzyzowki { get; set; } = new List<Krzyzowki>();
        public ICollection<Wisielec> Wisielec {  get; set; } = new List<Wisielec>();
        public ICollection<Wordle> Wordle { get; set; } = new List<Wordle>();
        public ICollection<Zgadywanki> Zgadywanki { get; set; } = new List<Zgadywanki>();
        public ICollection<Slownikowo> Slownikowo { get; set; } = new List<Slownikowo>();


        //// 1 - admin
        //// 0 - user
        //// 69 - banned user
        //public string Bans { get; set; }

        //// Referensts
        //public ICollection<Comments> Commensts { get; set; }
        //public List<Reports> Reports { get; set; } = new List<Reports>();

        //public ICollection<Status> Status { get; set; }
        //// Properties
        [NotMapped]
        public bool isLogged { get; set; } = false;

        //[NotMapped]
        //public ListsModel lists { get; set; }

        //[NotMapped]
        //public SearchResults search { get; set; }
    }
}
