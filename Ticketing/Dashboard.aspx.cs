using MySql.Data.MySqlClient;
using System;
using System.Collections;
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
                        BCrea.Visible = true;
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
                        BCrea.Visible = true;
                        break;

                    case 4:
                        Alltick.Visible = true;
                        DProdotto.Visible = true;
                        DStato.Visible = true;
                        DPriorita.Visible = true;
                        BVedi.Visible = true;
                        BCrea.Visible = true;
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
                Uruolo = cuser[0];
                Ulivello = cuser[2];
                Udipartimento = cuser[3];
            }
            else
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                BindDefaultTicketsByRole();

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
            Control btn = (Control)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            //GridViewRow row = Tickets.SelectedRow;

            int ticketId = Convert.ToInt32(Tickets.DataKeys[row.RowIndex].Value);

            //string cliente = row.Cells[2].Text;
            //string tecnico = row.Cells[3].Text;
            //string livello = row.Cells[4].Text;
            //string societa = row.Cells[5].Text;
            //string stato = row.Cells[6].Text;
            //string prodotto = row.Cells[7].Text;
            //string descrizione = row.Cells[8].Text;
            //string priorita = row.Cells[9].Text;


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
            BindDefaultTicketsByRole();
        }
        private void BindDefaultTicketsByRole()
        {
            DataTable table;

            switch (user.Ruolo)
            {
                case 1:
                    table = filtri.Filtro($"Societa = \"{Usocieta}\"", "tikdetails");
                    break;

                case 2:
                    table = filtri.Filtro($"(Livello = \"{Ulivello}\" OR Livello IS NULL)", "tikdetails");
                    break;

                case 3:
                    table = filtri.Filtro("tikdetails");
                    break;

                case 4:
                    table = filtri.Filtro($"Societa = \"{Usocieta}\"", "tikdetails");
                    break;

                default:
                    table = filtri.Filtro($"Societa = \"{Usocieta}\"", "tikdetails");
                    break;
            }

            BindTickets(table);
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
            ArrayList lista = new ArrayList();

            string Tecnico = DTecnico.SelectedValue;
            string Livello = DLivello.SelectedItem.Value;
            string Societa = DSocieta.SelectedValue;
            string Prodotto = DProdotto.SelectedValue;
            string Stato = DStato.SelectedValue;
            string Priorita = DPriorita.SelectedValue;

            if (Societa != "")
            {

                lista.Add($"Societa = \"{Societa}\"");
            }
            if (Tecnico != "")
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
            if (lista.Count == 0)
            {
                BindDefaultTicketsByRole();
                return;
            }

            string search = "";

            for (int i = 0; i < lista.Count; i++)
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



