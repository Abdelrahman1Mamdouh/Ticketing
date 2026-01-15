using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using Ticketing.Models;

namespace Ticketing
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool login = Session["CR"] != null;
            phUserArea.Visible = login;
            LogoNotifica.Visible = login;
            LogoUtente.Visible = login;


            if (login)
            {
                utente user = Session["CR"] as utente;
                lblWelcome.Text = "Welcome, " + user.Nome + " " + user.Cognome;
            }
        }
        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }

        protected void MostraNotifica(object sender, EventArgs e)
        {
            string tabella = "notifica";
            

                NotifichePopup.Show(tabella);
            
        }
        protected void MostraInfoUtente(object sender, EventArgs e)
        {
            Response.Redirect("~/InfoModificaUtenti.aspx");
        }
    }
}