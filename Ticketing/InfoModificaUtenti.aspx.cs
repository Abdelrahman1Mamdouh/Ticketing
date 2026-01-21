using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using Ticketing.Models;

namespace Ticketing
{
    public partial class InfoModificaUtenti : System.Web.UI.Page
    {
        private String nome;
        private String cognome;
        private String telefono;
        private String email;
        private String password;
        utente user ;
        protected void Page_Load(object sender, EventArgs e)
        {
            user = null;
            //user's data fetch from session
            if (Session["CR"] != null)
            {
                user = Session["CR"] as utente;
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
        public void clickSalvaModifiche(object sender, EventArgs e)
        {
            String cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            nome = TNome.Text;
            cognome = TCognome.Text;
            telefono = TTelefono.Text;
            email = TEmail.Text;
            password = TPassword.Text;

            using (MySqlConnection con = new MySqlConnection(cs)) 
            { 
                con.Open(); 
                string Modifica = $"UPDATE utente SET Nome= @nome, Cognome= @cognome, Telefono= @telefono, Pass= @password, Email= @email WHERE ID =@id";
                MySqlCommand cmd = new MySqlCommand(Modifica, con);

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = user.ID ;
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = nome;
                cmd.Parameters.Add("@cognome", MySqlDbType.VarChar).Value = cognome;
                cmd.Parameters.Add("@telefono", MySqlDbType.VarChar).Value= telefono;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value= email;
                cmd.Parameters.Add("@password", MySqlDbType.VarChar).Value= password;
                cmd.ExecuteNonQuery();

            }

        }
    }
}
