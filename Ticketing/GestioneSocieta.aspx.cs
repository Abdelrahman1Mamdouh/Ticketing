using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using Ticketing.Models;


namespace Ticketing
{
    public partial class GestioneSocieta : System.Web.UI.Page
    {

        private string nome;
        private string indirizzo;
        private string citta;
        private string cap;
        private string telefono;
        private string email;
        private string piva;
        private string note;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CR"] != null)
            {
                utente user = Session["CR"] as utente;
                if (user != null)
                {
                    string welcomeMessage = $"Benvenuto,{user.Nome} {user.Cognome}!";
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        public void clickCrea(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            nome = TNome.Text;
            indirizzo = TIndirizzo.Text;
            citta = TCitta.Text;
            cap = TCap.Text.Trim();
            telefono = TTelefono.Text.Trim();
            email = TeMail.Text.Trim();
            piva = TPIva.Text.Trim();
            note = TNote.Text;


            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string newSocieta = "INSERT INTO societa (Nome, Indirizzo, Citta, Cap, Telefono, Email,PIva,Note) VALUES (@nome, @indirizzo, @citta, @cap, @telefono, @email,@piva,@note)";

                MySqlCommand cmd = new MySqlCommand(newSocieta, con);

                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = nome; //More secure, advised (To be researched)

                cmd.Parameters.Add("@indirizzo", MySqlDbType.VarChar).Value = indirizzo;
                cmd.Parameters.Add("@citta", MySqlDbType.VarChar).Value = citta;
                cmd.Parameters.Add("@cap", MySqlDbType.VarChar).Value = cap;
                cmd.Parameters.Add("@telefono", MySqlDbType.VarChar).Value = telefono;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                cmd.Parameters.Add("@piva", MySqlDbType.VarChar).Value = piva;
                cmd.Parameters.Add("@note", MySqlDbType.VarChar).Value = note;

                cmd.ExecuteNonQuery();
            }

        }

        public void clickModifica(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;


            nome = TNome.Text;
            indirizzo = TIndirizzo.Text;
            citta = TCitta.Text;
            cap = TCap.Text.Trim();
            telefono = TTelefono.Text.Trim();
            email = TeMail.Text.Trim();
            piva = TPIva.Text.Trim();
            note = TNote.Text;


            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string modificaSocieta =
                "UPDATE societa SET Nome= @nome, Indirizzo= @indirizzo, Citta= @citta, Cap= @cap, Telefono= @telefono, Email= @email, PIva=@piva WHERE PIva = @piva"; //change textbox to piva
                MySqlCommand cmd = new MySqlCommand(modificaSocieta, con);

                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = nome;
                cmd.Parameters.Add("@indirizzo", MySqlDbType.VarChar).Value = indirizzo;
                cmd.Parameters.Add("@citta", MySqlDbType.VarChar).Value = citta;
                cmd.Parameters.Add("@cap", MySqlDbType.VarChar).Value = cap;
                cmd.Parameters.Add("@telefono", MySqlDbType.VarChar).Value = telefono;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                cmd.Parameters.Add("@piva", MySqlDbType.VarChar).Value = piva;
                cmd.Parameters.Add("@note", MySqlDbType.VarChar).Value = note;
                cmd.ExecuteNonQuery();
            }

        }
        public void clickElimina(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            piva = TPIva.Text.Trim();
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string deleteSocieta = "DELETE FROM `societa` WHERE PIva= @piva";
                MySqlCommand cmd = new MySqlCommand(deleteSocieta, con);

                cmd.Parameters.Add("@piva", MySqlDbType.VarChar).Value = piva;
                cmd.ExecuteNonQuery();
            }
        }
    }
}