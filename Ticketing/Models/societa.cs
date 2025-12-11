using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ticketing.Models
{
    public class societa
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Indirizzio { get; set; }
        public string Citta { get; set; }
        public string Cap {  get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
    }
}