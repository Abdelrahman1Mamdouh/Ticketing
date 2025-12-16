using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Runtime.Remoting.Messaging;
using Ticketing.Models;

namespace Ticketing
{
    public partial class Login : System.Web.UI.Page
    {
        string userName;
        string passwordText;
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            userName = TUser.Text.Trim();
            passwordText = TPass.Text.Trim();
            string query = "SELECT ID, Nome, Cognome, Ruolo, Societa, Livello, Dipartimento, Telefono, Email, Pass FROM utente WHERE Email = @Email AND Pass = @Pass";

            using (MySqlConnection con = new MySqlConnection(cs))
            {


                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = userName;
                    cmd.Parameters.Add("@Pass", MySqlDbType.VarChar).Value = passwordText;

                    try
                    {
                        con.Open();
                        MySqlDataReader r = cmd.ExecuteReader();

                        if (r.Read())
                        {
                            utente currentUser = new utente
                            {
                                ID = r.GetInt32("ID"),
                                Nome = r.GetString("Nome"),
                                Cognome = r.GetString("Cognome"),
                               
                                Telefono = r.GetString("Telefono"),
                                Email = r.GetString("Email"),

                                Ruolo = r.IsDBNull(r.GetOrdinal("Ruolo")) ? 0 : r.GetInt32("Ruolo"),
                                Societa = r.IsDBNull(r.GetOrdinal("Societa")) ? 0 : r.GetInt32("Societa"),
                                Livello = r.IsDBNull(r.GetOrdinal("Livello")) ? 0 : r.GetInt32("Livello"),
                                Dipartimento = r.IsDBNull(r.GetOrdinal("Dipartimento")) ? 0 : r.GetInt32("Dipartimento")
                            };


                        
                            Session["CR"] = currentUser;
                            r.Close();
                            Response.Redirect("Dashboard.aspx");

                        }
                        else
                        {
                            r.Close();
                            Response.Write("<script>alert('Username o password sono invalidi')</script>");
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Write("Errore di connessione o del database: " + ex.Message);
                    }
                }
            }
        }
        protected void BtnAnnulla_Click(object sender, EventArgs e)
        {
            userName = TUser.Text = "";
            passwordText = TPass.Text = "";
        }
    }
}