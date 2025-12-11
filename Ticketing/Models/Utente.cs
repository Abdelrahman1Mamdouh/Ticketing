using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ticketing.Models
{
    public class Utente
    {
        private int ID { get; set; }
        private string Nome { get; set; }
        private string Cognome { get; set; }
        private int Ruolo { get; set; }
        private int Societa { get; set; }
        private int Livello { get; set; }
        private int Dipartimento { get; set; }
        private string Telefono { get; set; }
        private string Email { get; set; }
        private string Pass { get; set; }
    }
}