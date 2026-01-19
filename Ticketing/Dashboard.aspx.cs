using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ticketing.Models;


namespace Ticketing
{

    public partial class Dashboard : System.Web.UI.Page
    {
        private int currentUser;
        utente user;
        string[] cuser;

        string QTecnico = $"select ID, concat(Nome, \" \", Cognome) as Tecnico from utente where Ruolo=2;";
        string QSocieta = "SELECT ID, Nome as Societa FROM societa;";
        string QStato = "SELECT ID,Stato FROM stato;";
        string QProdotto = "SELECT ID,Prodotto FROM prodotto;";
        string QPriorita = "SELECT ID,Priorita FROM priorita;";
        string QLivello = "SELECT ID,Livello FROM livello;";

        

        string Usocieta;
        string Ulivello;
        string Uruolo;
        string Udipartimento;



        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (Session["CR"] != null)
            {
                BCrea.Visible = true;
                user = Session["CR"] as utente;
                currentUser = user.ID;

                switch (user.Ruolo)
                {

                    case 1:
                    case 2:

                        Alltick.Visible = true;
                        Mytick.Visible = true;

                        break;

                    case 3:
                        Alltick.Visible = true;
                        DTecnico.Visible = true;
                        DLivello.Visible = true;
                        DStato.Visible = true;
                        DPriorita.Visible = true;
                        DSocieta.Visible = true;
                        DProdotto.Visible = true;
                        BVedi.Visible = true;
                        break;

                    case 4:
                        Alltick.Visible = true;
                        DProdotto.Visible = true;
                        DStato.Visible = true;
                        DPriorita.Visible = true;
                        BVedi.Visible = true;
                        break;

                }
                string query2 = "SELECT Ruolo, Societa, Livello, Dipartimento FROM utenti WHERE ID=@id";

                string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(cs))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query2, con))
                    {
                        cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = user.ID;



                        con.Open();
                        MySqlDataReader r = cmd.ExecuteReader();

                        if (r.Read())
                        {
                            cuser = new string[4];
                            cuser[0] = r.IsDBNull(r.GetOrdinal("Ruolo")) ? null : r.GetString("Ruolo");
                            cuser[1] = r.IsDBNull(r.GetOrdinal("Societa")) ? null : r.GetString("Societa");
                            cuser[2] = r.IsDBNull(r.GetOrdinal("Livello")) ? null : r.GetString("Livello");
                            cuser[3] = r.IsDBNull(r.GetOrdinal("Dipartimento")) ? null : r.GetString("Dipartimento");


                            r.Close();


                        }
                    }
                }
                Usocieta = cuser[1];
                Uruolo = cuser[1];
                Ulivello = cuser[2];
                Udipartimento = cuser[1];

            }
            else
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                var table1 = new DataTable();
                table1 = filtri.Filtro($"Societa = \"{Usocieta}\"", "tikdetails");
                BindTickets(table1);
                GestioneTicket.LoadDropDownList(QTecnico, DTecnico, "Scegli un tecnico:");
                GestioneTicket.LoadDropDownList(QSocieta, DSocieta, "Scegli una societa:");
                GestioneTicket.LoadDropDownList(QStato, DStato, "Scegli un stato:");
                GestioneTicket.LoadDropDownList(QPriorita, DPriorita, "Scegli una priorità:");
                GestioneTicket.LoadDropDownList(QProdotto, DProdotto, "Scegli un prodotto:");
                GestioneTicket.LoadDropDownList(QLivello, DLivello, "Scegli una livello:");
            }



            

            


        }

        private void BindTickets(DataTable table)
        {
            

            Tickets.DataSource = table;
            Tickets.DataBind();
            

            
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


        // filtri Gridview

        protected void AllTicket(object sender, EventArgs e)
        {
           
            
            switch (user.Ruolo)
            {

                case 1:
                    var table1 = new DataTable();
                    table1 = filtri.Filtro($"Societa = \"{Usocieta}\"", "tikdetails");
                    BindTickets(table1);
                    break;
                case 2:
                    var table = new DataTable();
                    table = filtri.Filtro($" Livello = \"{Ulivello}\"","tikdetails");
                    BindTickets(table);
                    break;
                case 3:
                    var table3 = new DataTable();
                    table3 = filtri.Filtro("tikdetails");
                    BindTickets(table3);
                    break;
                case 4:
                    var table4 = new DataTable();
                    table4 = filtri.Filtro($"Societa = \"{Usocieta}\"",  "tikdetails");
                    BindTickets(table4);
                    break;
            }

        }
        protected void MyTicket(object sender, EventArgs e)
        {
           

            switch (user.Ruolo)
            {

                case 1:
                    var table1 = new DataTable();
                    table1 = filtri.Filtro($"Cliente= \"{user.Nome} {user.Cognome}\" and Societa = \"{Usocieta}\"", "tikdetails");
                    BindTickets(table1);
                    break;
                case 2:
                    var table = new DataTable();
                    table = filtri.Filtro($"Tecnico = \"{user.Nome} {user.Cognome}\" and Livello = \"{Ulivello}\"", "tikdetails");
                    BindTickets(table);
                    break;
            }

        }

        protected void MixTicket(object sender, EventArgs e)
        {
            
            ArrayList lista= new ArrayList();

            


            string Tecnico =DTecnico.SelectedValue;
            string Livello = DLivello.SelectedItem.Value;
            string Societa = DSocieta.SelectedValue;
            string Prodotto = DProdotto.SelectedValue;
            string Stato = DStato.SelectedValue;
            string Priorita = DPriorita.SelectedValue;

            if (Societa != "")
            {

                lista.Add($"Societa = \"{Societa}\"");
            }
            if (Tecnico!= "")
            {
                
                lista.Add($"Tecnico = \"{Tecnico}\"");
            }
            if (Livello != "")
            {
                
                lista.Add($"Livello = \"{Livello}\"");
            }
            if (Prodotto != "")
            {
                
                lista.Add($"Prodotto = \"{Prodotto}\"");
            }
            if (Stato != "")
            {
                
                lista.Add($"Stato = \"{Stato}\"");
            }
            if (Priorita != "")
            {
                
                lista.Add($"Priorita = \"{Priorita}\"");
            }

            string search = "";
            
                for (int i=0;i<lista.Count;i++)
                {
                    search += $" {lista[i]} and ";
                }
            
            

            if (user.Ruolo == 4)
            {
                search += $"Societa = \"{Usocieta}\"";
            }
            else
            {
                search = search.Substring(0, search.Length - 5);
            }

                var table1 = new DataTable();
            table1 = filtri.Filtro(search, "tikdetails");
            BindTickets(table1);
            

            
        }
    }
}

        

