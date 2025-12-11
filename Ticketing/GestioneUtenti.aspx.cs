using MySql.Data.MySqlClient;
using System;
using System.Configuration;


namespace Ticketing
{
    public partial class GestioneUtenti : System.Web.UI.Page
    {
        private String nome;
        private String cognome;
        private String ruolo;
        private String societa;
        private String livello;
        private String dipartimento;
        private String telefono;
        private String email;
        private String comunicazione;
        private String pass;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void clickCrea(object sender, EventArgs e)
        {

            String cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            nome = TNome.Text;
            cognome = TCognome.Text;
            ruolo = DRuolo.Text;
            societa = TSocieta.Text;
            livello = DLivello.Text;
            dipartimento = DDipartimento.Text;
            pass = TPassword.Text;
            telefono = TTelefono.Text;
            email = TeMail.Text;
            comunicazione = TComunicazione.Text;

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                String newUtenti = "INSERT INTO societa (Nome, Cognome, Ruolo, Societa, Livello, Dipartimento, Password, Telefono, Email, Comunicazione) VALUES (@nome, @cognome, @ruolo, @societa, @livello, @dipartimento, @password, @telefono, @email, @comunicazione)";

                MySqlCommand cmd = new MySqlCommand(newUtenti, con);

                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = nome;
                cmd.Parameters.Add("@cognome", MySqlDbType.VarChar).Value = cognome;
                cmd.Parameters.Add("@ruolo", MySqlDbType.VarChar).Value = ruolo;
                cmd.Parameters.Add("@societa", MySqlDbType.VarChar).Value = societa;
                cmd.Parameters.Add("@livelloo", MySqlDbType.VarChar).Value = livello;
                cmd.Parameters.Add("@dipartimento", MySqlDbType.VarChar).Value = dipartimento;
                cmd.Parameters.Add("@password", MySqlDbType.VarChar).Value = pass;
                cmd.Parameters.Add("@telefono", MySqlDbType.VarChar).Value = telefono;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                cmd.Parameters.Add("@comunicazione", MySqlDbType.VarChar).Value = comunicazione;
                cmd.ExecuteNonQuery();
            }

        }

        public void clickModifica(object sender, EventArgs e)
        {
            String cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            nome = TNome.Text.Trim();
            cognome = TCognome.Text.Trim();
            ruolo = DRuolo.Text.Trim();
            societa = TSocieta.Text.Trim();
            livello = DLivello.Text.Trim();
            dipartimento = DDipartimento.Text.Trim();
            pass = TPassword.Text.Trim();
            telefono = TTelefono.Text.Trim();
            email = TeMail.Text.Trim();
            comunicazione = TComunicazione.Text.Trim();

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string ModificaUtenti =
                $"UPDATE utenti SET Nome= @nome, Cognome= @cognome, Ruolo= @ruolo, Societa= @societa, Livello= @livello, Dipartimento= @dipartimento, Telefono= @telefono, Email= @email, Comunicazione= @comunicazione WHERE (Email=> @email)";
                MySqlCommand cmd = new MySqlCommand(ModificaUtenti, con);

                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = nome;
                cmd.Parameters.Add("@cognome", MySqlDbType.VarChar).Value = cognome;
                cmd.Parameters.Add("@ruolo", MySqlDbType.VarChar).Value = ruolo;
                cmd.Parameters.Add("@societa", MySqlDbType.VarChar).Value = societa;
                cmd.Parameters.Add("@livelloo", MySqlDbType.VarChar).Value = livello;
                cmd.Parameters.Add("@dipartimento", MySqlDbType.VarChar).Value = dipartimento;
                cmd.Parameters.Add("@password", MySqlDbType.VarChar).Value = pass;
                cmd.Parameters.Add("@telefono", MySqlDbType.VarChar).Value = telefono;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                cmd.Parameters.Add("@comunicazione", MySqlDbType.VarChar).Value = comunicazione;
                cmd.ExecuteNonQuery();
            }
        }

        public void clickElimina(object sender, EventArgs e)
        {
            String cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string deleteUtenti = "DELETE FROM `utenti` [WHERE Email== @email]";
                MySqlCommand cmd = new MySqlCommand(deleteUtenti, con);
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                cmd.ExecuteNonQuery();
            }
        }
    }
}