using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ticketing.Models;

namespace Ticketing
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private int currentUser;
        utente user;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["CR"] != null)
            {
                user = Session["CR"] as utente;
                currentUser = user.ID;
                if(user.Societa != null)
                {
                    BCrea.Visible = true;
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
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                MySqlCommand command = new MySqlCommand("SELECT * FROM detailsticket ORDER BY Creata_a DESC", con);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                var table = new DataTable();
                adapter.Fill(table);

                Tickets.DataSource = table;
                Tickets.DataBind();

            }
        }

        protected void ClickSelectTicket(object sender, EventArgs e)
        {
            Control btn = (Control)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
           //GridViewRow row = Tickets.SelectedRow;

            int ticketId = Convert.ToInt32(Tickets.DataKeys[row.RowIndex].Value);

            //int ticketId = Convert.ToInt32(row.Cells[0]);

            string cliente = row.Cells[2].Text;
            string tecnico = row.Cells[3].Text;
            string livello = row.Cells[4].Text;
            string stato = row.Cells[5].Text;
            string prodotto = row.Cells[6].Text;
            string categoria = row.Cells[7].Text;
            string descrizione = row.Cells[8].Text;
            string titolo = row.Cells[9].Text;
            string priorita = row.Cells[10].Text;

            

            string[] tik = new string[9];
            tik[0]= cliente;
            tik[1] = tecnico;
            tik[2] = livello;
            tik[3] = stato;
            tik[4] = prodotto;
            tik[5] = categoria;
            tik[6] = descrizione;
            tik[7] = titolo;
            tik[8] = priorita;

            Session["ticket"] = tik;
            

             
            //try
            //{
            //    if (user.Ruolo == 1 || user.Ruolo == 4)
            //    {
            //        return;
            //    }
            //    else
            //    {
            //        string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            //        using (MySqlConnection con = new MySqlConnection(cs))
            //        {
            //            con.Open();
            //            string nuovaTicket = $"UPDATE ticket SET Tecnico=@tecnico WHERE ID=@id";

            //            using (MySqlCommand cmd = new MySqlCommand(nuovaTicket, con))
            //            {
            //                cmd.Parameters.Add("@Tecnico", MySqlDbType.VarChar).Value = currentUser;
            //                cmd.Parameters.AddWithValue("@id", MySqlDbType.Int32).Value = ticketId;
            //                cmd.ExecuteNonQuery();
            //            }
            //        }                    
            //    }


            //}
            //catch (Exception ex)
            //{
            //    Response.Write("DB error: " + ex.Message);
            //    return;
            //}
            BindTickets();
            Response.Write("<script>alert('Fatto')</script>");
            Response.Redirect("GestioneTicket.aspx?id=" + ticketId);
        }
        protected void CreateTicket(object sender, EventArgs e)
        {

            Response.Redirect("GestioneTicket.aspx");

        }
        protected void ClickDeleteTicket(object sender, EventArgs e)
        {


        }
    }
}