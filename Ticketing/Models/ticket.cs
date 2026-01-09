using System;

namespace Ticketing.Models
{
    public class ticket
    {
        public int ID { get; set; }
        public string Cliente { get; set; }
        public string Tecnico { get; set; }
        public string Livello { get; set; }
        public string Stato { get; set; }
        public string Prodotto { get; set; }
        public string Categoria { get; set; }
        public string Priorita { get; set; }
        public string Descrizione { get; set; }
        public string Titolo { get; set; }
        public DateTime Create_a { get; set; }
    }
}