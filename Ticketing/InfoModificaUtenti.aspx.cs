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
            if (!IsPostBack) 
            { 
                CaricaDati(user); 
            }

        }

        private void CaricaDati(utente user)
        {
            TNome.Text = user.Nome;
            TCognome.Text = user.Cognome;
            TTelefono.Text = user.Telefono;
            TEmail.Text = user.Email;  
        }

     


        public void clickSalvaModifiche(object sender, EventArgs e)
        {
            String cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            nome = TNome.Text;
            cognome = TCognome.Text;
            telefono = TTelefono.Text;
            email = TEmail.Text;

            using (MySqlConnection con = new MySqlConnection(cs)) 
            { 
                con.Open(); 

                string Modifica = $"UPDATE utente SET Nome= @nome, Cognome= @cognome, Telefono= @telefono, Email= @email WHERE ID= @id";
                MySqlCommand cmd = new MySqlCommand(Modifica, con);

                utente user = Session["CR"] as utente;

                cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = user.ID;
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = nome;
                cmd.Parameters.Add("@cognome", MySqlDbType.VarChar).Value = cognome;
                cmd.Parameters.Add("@telefono", MySqlDbType.VarChar).Value= telefono;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value= email;
               
                cmd.ExecuteNonQuery();



             

            }

        }
    }
}
