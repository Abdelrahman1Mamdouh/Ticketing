using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI;

namespace Ticketing.Controls
{
    public partial class NotifichePopup : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public  void Show(String tabella)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                MySqlCommand command = new MySqlCommand($"SELECT * FROM {tabella}", con);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                gvNotifichePopup.DataSource = table;
                gvNotifichePopup.DataBind();

            }

            

           
            string script = $"document.getElementById('{pnlNotifichePopup.ClientID}').style.display='block';";
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowNotifichePopup", script, true);
        }

        public void Hide()
        {
            string script = $"document.getElementById('{pnlNotifichePopup.ClientID}').style.display='none';";
            ScriptManager.RegisterStartupScript(this, GetType(), "HideNotifichePopup", script, true);
        }
    }
}
