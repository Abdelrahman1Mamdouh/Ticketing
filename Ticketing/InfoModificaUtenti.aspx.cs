using MySql.Data.MySqlClient;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            utente user = null;
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
                string Modifica = $"UPDATE utenti SET Nome= @nome, Cognome= @cognome, Telefono= @telefono, Password= @password, Email= @email WHERE (Email=> @email)";
                MySqlCommand cmd = new MySqlCommand(Modifica, con);

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
