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

        protected void ClickSelectTicket(object sender, EventArgs e)
        {
            Control btn = (Control)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            int ticketId = Convert.ToInt32(gvNotifichePopup.DataKeys[row.RowIndex].Value);

            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            string[] tik = new string[9];

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string nuovaTicket = $"SELECT * FROM tikdetails WHERE ID={ticketId}";

                using (MySqlCommand cmd = new MySqlCommand(nuovaTicket, con))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    tik[0] = reader.IsDBNull(reader.GetOrdinal("Cliente")) ? null : reader.GetString("Cliente");
                    tik[1] = reader.IsDBNull(reader.GetOrdinal("Tecnico")) ? null : reader.GetString("Tecnico");
                    tik[2] = reader.IsDBNull(reader.GetOrdinal("Livello")) ? null : reader.GetString("Livello");
                    tik[3] = reader.IsDBNull(reader.GetOrdinal("Stato")) ? null : reader.GetString("Stato");
                    tik[4] = reader.IsDBNull(reader.GetOrdinal("Prodotto")) ? null : reader.GetString("Prodotto");
                    tik[5] = reader.IsDBNull(reader.GetOrdinal("Categoria")) ? null : reader.GetString("Categoria");
                    tik[6] = reader.IsDBNull(reader.GetOrdinal("Descrizione")) ? null : reader.GetString("Descrizione");
                    tik[7] = reader.IsDBNull(reader.GetOrdinal("Titolo")) ? null : reader.GetString("Titolo");
                    tik[8] = reader.IsDBNull(reader.GetOrdinal("Priorita")) ? null : reader.GetString("Priorita");

                    Session["ticket"] = tik;

                    Response.Write("<script>alert('Fatto')</script>");
                    Response.Redirect("GestioneTicket.aspx?id=" + ticketId);
                }
            }
        }
    }
}