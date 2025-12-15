namespace Ticketing.Models
{
    public class utente
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public int Ruolo { get; set; }
        public int Societa { get; set; }
        public int Livello { get; set; }
        public int Dipartimento { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
    }
}