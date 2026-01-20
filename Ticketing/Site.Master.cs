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
            Navigation.Visible = login;

            if (login)
            {
                utente user = Session["CR"] as utente;
                switch (user.Ruolo)
                {
                    case 1:
                        Utenti.Visible = false;
                        Societa.Visible = false;
                        break;

                    case 2:
                        Societa.Visible = false;
                        break;

                    case 3:

                        Societa.Visible = true;
                        break;

                    case 4:
                        Societa.Visible = false;
                        break;


                }
                
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
                NotifichePopup.Show();    
        }
        protected void MostraInfoUtente(object sender, EventArgs e)
        {
            Response.Redirect("~/InfoModificaUtenti.aspx");
        }
    }
}