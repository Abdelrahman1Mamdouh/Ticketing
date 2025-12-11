using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ticketing.Models
{
    public class ticket
    {
        public int ID { get; set; }
        public int Cliente { get; set; }
        public int Tecnico { get; set; }
        public int Livello { get; set; }
        public int Stato { get; set; }
        public int Prodotto { get; set; }
        public int Categoria { get; set; }
        public int Priorita { get; set; }
        public string Descrizione { get; set; }
        public string Titolo { get; set; }
        public DateTime Create_a { get; set; }
    }
}