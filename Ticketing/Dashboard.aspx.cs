using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using Ticketing.Models;

namespace Ticketing
{
    public partial class Dashboard : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

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



            if (!IsPostBack)
            {
                BindTickets();
            }
        }

        private void BindTickets()
        {
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                MySqlCommand command = new MySqlCommand("SELECT ID,Cliente,Prodotto,Descrizione,Creata_a FROM ticket", con);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                var table = new DataTable();
                adapter.Fill(table);

                Tickets.DataSource = table;
                Tickets.DataBind();

            }
        }
    }
}

