using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Ticketing.Controls;
using Ticketing.Models;


namespace Ticketing
{

    public partial class Dashboard : System.Web.UI.Page
    {
        private int currentUser;
        

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
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                MySqlCommand command = new MySqlCommand("SELECT * from tik", con);

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
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            ticket tik = new ticket();

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string nuovaTicket = $"select * from tik WHERE ID={ticketId}";

                using (MySqlCommand cmd = new MySqlCommand(nuovaTicket, con))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();

                    tik.ID = reader.GetInt32("ID");
                    tik.Cliente = reader.GetString("Cliente");
                    tik.Tecnico = reader.GetString("Tecnico");
                    tik.Livello = reader.GetString("Livello");
                    tik.Stato = reader.GetString("Stato");
                    tik.Prodotto = reader.GetString("Prodotto");
                    tik.Categoria = reader.GetString("Categoria");
                    tik.Priorita = reader.GetString("Priorita");
                    tik.Titolo = reader.GetString("Titolo");
                    tik.Descrizione = reader.GetString("Descrizione");
                   

                    Session["ticket"] = tik;
                    Session["action"] = "select";

                }
            }

            Response.Redirect("GestioneTicket.aspx");


            
        }

      

        protected void ClickDeleteTicket(object sender, EventArgs e)
        {

            
        }

        protected void CreateTicket(object sender, EventArgs e)
        {
            
            Session["action"] = "create";
            Response.Redirect("GestioneTicket.aspx");

        }
    }
}

        

