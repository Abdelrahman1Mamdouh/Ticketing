using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ticketing.Models;

namespace Ticketing.Controls
{
    public partial class NotifichePopup : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void Show()
        {
            
            utente user = Session["CR"] as utente;
            if (user == null) return;

            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();


                string sql = "SELECT TicketID, Mittente, Messaggio, letturaNotifica FROM Notificas WHERE idDestinatario = @userID ORDER BY DataCreazione DESC";

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.Add("@userID", MySqlDbType.Int32).Value = user.ID;

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    
                    gvNotifichePopup.DataSource = table;
                    gvNotifichePopup.DataBind();
                }
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