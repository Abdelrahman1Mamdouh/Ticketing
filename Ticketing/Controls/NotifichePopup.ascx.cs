using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Web.UI;
using Ticketing.Models;

namespace Ticketing.Controls
{
    public partial class NotifichePopup : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack) 
            //{
            //    Show();
            //}
        }

        //public void mostraNotifica(object sender, EventArgs e)
        //{
        //    utente user = Session["CR"] as utente;
            
        //    if (user == null) return;

        //    string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
        //    using (MySqlConnection con = new MySqlConnection(cs))
        //    {
        //        con.Open();

        //        //string sql = "SELECT * FROM storico";

        //        string sql = "SELECT nomeMittente, nomeDestinatario, Oggetto FROM storico WHERE Mittente = nomeMittente, Destinatario = nomeDestinatario, Oggetto = messaggio";
        //        MySqlCommand cmd = new MySqlCommand(sql, con);

        //        //cmd.Parameters.AddWithValue("@myID", user.ID);
        //        //int conteggio = Convert.ToInt32(cmd.ExecuteScalar());
        //    }
        //}


        public void Show()
        {
            utente user = Session["CR"] as utente;
            if (user == null) return;

            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                //MySqlCommand cmd = new MySqlCommand("SELECT Cliente, Descrizione FROM detailsticket", con);
                MySqlCommand cmd = new MySqlCommand("SELECT nomeMittente AS Mittente, nomeDestinatario AS Destinatario, Oggetto AS Messaggio FROM storico WHERE letturaNotifica = 0", con);


                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
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
