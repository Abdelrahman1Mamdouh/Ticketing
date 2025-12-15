using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace Ticketing
{
    public partial class Dashboard : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
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

                MySqlCommand command = new MySqlCommand("SELECT * FROM ticket", con);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                var table = new DataTable();
                adapter.Fill(table);

                Tickets.DataSource = table;
                Tickets.DataBind();
            }
        }
    }
}