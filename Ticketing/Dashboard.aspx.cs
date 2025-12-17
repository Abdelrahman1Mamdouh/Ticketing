using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using Ticketing.Models;

namespace Ticketing
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private int currentUser;
        string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

            utente user;
            if (Session["CR"] != null)
            {
                user = Session["CR"] as utente;
                currentUser = user.ID;
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

                MySqlCommand command = new MySqlCommand("SELECT * FROM ticket ORDER BY Creata_a DESC", con);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                var table = new DataTable();
                adapter.Fill(table);

                Tickets.DataSource = table;
                Tickets.DataBind();

            }
        }

        protected void ClickSelectTicket(object sender, EventArgs e)
        {
            
            int ticketId = Convert.ToInt32(Tickets.SelectedDataKey.Value); 
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

                using (MySqlConnection con = new MySqlConnection(cs))
                {
                    con.Open();
                    string nuovaTicket = $"UPDATE ticket SET Tecnico=@tecnico WHERE ID=@id";

                    using (MySqlCommand cmd = new MySqlCommand(nuovaTicket, con))
                    {
                        cmd.Parameters.AddWithValue("@Tecnico", currentUser);
                        cmd.Parameters.AddWithValue("@id", ticketId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("DB error: " + ex.Message);
                return;
            }
            BindTickets();
            Response.Write("<script>alert('Fatto')</script>");
            Response.Redirect("GestioneTicket.aspx");
        }
    }
}