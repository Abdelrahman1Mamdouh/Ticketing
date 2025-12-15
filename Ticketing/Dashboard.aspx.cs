using System;
using Ticketing.Models;

namespace Ticketing
{
    public partial class Dashboard : System.Web.UI.Page
    {
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
        }
    }
}