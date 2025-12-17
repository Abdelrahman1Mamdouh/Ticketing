using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Web.UI.WebControls;
using Ticketing.Models;

namespace Ticketing
{
    public partial class GestioneTicket : System.Web.UI.Page
    {
        //private String cliente;
        //private String tecnico;
        //private String livello;
        //private String stato;
        private String prodotto;
        private String categoria;
        //private String priorita;
        private String oggetto;
        private String messaggio;
        private String note;

        private int DefaultStato = 1;
        private int currentUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            utente user;
            if (Session["CR"] != null)
            {
                user = Session["CR"] as utente;
                if (!IsTecnico(user))
                {
                    currentUser = user.ID;
                    CampiCliente();
                }
                else
                {
                    currentUser = user.ID;
                    CampiTecnico();
                    
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadDropDownList("SELECT ID,Prodotto FROM prodotto;", DProdotto, "Scegli un prodotto:");
                LoadDropDownList("SELECT ID,Categoria FROM categoria ORDER BY Categoria;", DCategoria, "Scegli una categoria:");
            }
        }
        private bool IsTecnico(utente user)
        {
            if (user != null && (user.Dipartimento > 1 || user.Dipartimento == 1))
            {
                return true;
            }
            return false;
        }
        private void LoadDropDownList(string query, DropDownList DDL, string label)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            using (var con = new MySqlConnection(cs))
            {
                using (var cmd = new MySqlCommand(query, con))
                {

                    con.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        DDL.DataSource = dr;
                        DDL.DataBind();
                    }
                    DDL.Items.Insert(0, new ListItem(label, ""));
                    DDL.Items[0].Attributes["disabled"] = "disabled";
                    DDL.Items[0].Attributes["selected"] = "selected";
                    //if (query.Contains("prodotto"))
                    //{
                    //    con.Open();
                    //    using (var dr = cmd.ExecuteReader())
                    //    {
                    //        DProdotto.DataSource = dr;
                    //        DProdotto.DataBind();
                    //    }
                    //    DProdotto.Items.Insert(0, new ListItem("Scegli un prodotto", ""));
                    //    DProdotto.Items[0].Attributes["disabled"] = "disabled";
                    //    DProdotto.Items[0].Attributes["selected"] = "selected";
                    //}
                    //else if (query.Contains("categoria"))
                    //{
                    //    con.Open();
                    //    using (var dr = cmd.ExecuteReader())
                    //    {
                    //        DCategoria.DataSource = dr;
                    //        DCategoria.DataBind();
                    //    }
                    //    DCategoria.Items.Insert(0, new ListItem("Scegli un categoria", ""));
                    //    DCategoria.Items[0].Attributes["disabled"] = "disabled";
                    //    DCategoria.Items[0].Attributes["selected"] = "selected";
                    //}
                }
            }
        }
        public void clickCrea(object sender, EventArgs e)
        {
            try {
                string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

                //cliente = TCliente.Text.Trim();
                //tecnico = TTecnico.Text.Trim();
                //livello = TLivello.Text.Trim();
                //stato = DStato.Text.Trim();

                prodotto = DProdotto.Text;
                categoria = DCategoria.Text;
                //priorita = DPriorita.Text.Trim();
                oggetto = TOggetto.Text.Trim();
                messaggio = TMessaggio.Text.Trim();
                note = TComunicazione.Text.Trim();

                using (MySqlConnection con = new MySqlConnection(cs))
                {
                    con.Open();
                    string nuovaTicket = $"INSERT INTO ticket (Cliente, Prodotto, Categoria, Stato, Titolo, Descrizione, note) VALUES (@{currentUser}, @prodotto, @categoria, @{DefaultStato}, @oggetto, @messaggio, @note);";

                    MySqlCommand cmd = new MySqlCommand(nuovaTicket, con);

                    //cmd.Parameters.Add("@cliente", MySqlDbType.VarChar).Value = cliente;
                    //cmd.Parameters.Add("@tecnico", MySqlDbType.VarChar).Value = tecnico;
                    //cmd.Parameters.Add("@livello", MySqlDbType.VarChar).Value = livello;
                    //cmd.Parameters.Add("@stato", MySqlDbType.VarChar).Value = stato;
                    cmd.Parameters.Add("@prodotto", MySqlDbType.VarChar).Value = prodotto;
                    cmd.Parameters.Add("@categoria", MySqlDbType.VarChar).Value = categoria;
                    //cmd.Parameters.Add("@priorita", MySqlDbType.VarChar).Value = priorita;
                    cmd.Parameters.Add("@oggetto", MySqlDbType.VarChar).Value = oggetto;
                    cmd.Parameters.Add("@messaggio", MySqlDbType.VarChar).Value = messaggio;
                    cmd.Parameters.Add("@note", MySqlDbType.VarChar).Value = note;
                    cmd.ExecuteNonQuery();
                } 
            }catch (Exception ex)
            {
                Response.Write("Errore di connessione o del database: " + ex.Message);
            }
                Response.Write("<script>alert('Fatto')</script>");
            }

        public void ClickSceglie(object sender, EventArgs e)
        {
            ticket Tick = new ticket();
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

                using (MySqlConnection con = new MySqlConnection(cs))
                {
                    con.Open();
                    string nuovaTicket = $"UPDATE ticket SET Tecnico=@{currentUser} WHERE (ID=@{Tick.ID});";

                    MySqlCommand cmd = new MySqlCommand(nuovaTicket, con);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Response.Write("Errore di connessione o del database: " + ex.Message);
            }
            Response.Write("<script>alert('Fatto')</script>");
        
        }

        public void clickModifica(object sender, EventArgs e)
        {
            String cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            //cliente = TCliente.Text.Trim();
            //tecnico = TTecnico.Text.Trim();
            //livello = TLivello.Text.Trim();
            //stato = DStato.Text.Trim();
            prodotto = DProdotto.Text.Trim();
            categoria = DCategoria.Text.Trim();
            //priorita = DPriorita.Text.Trim();
            oggetto = TOggetto.Text.Trim();
            messaggio = TMessaggio.Text.Trim();
            note = TComunicazione.Text.Trim();

            using (MySqlConnection con = new MySqlConnection(cs))

            {
                con.Open();
                string ModificaTicket =
                "UPDATE societa SET Cliente= @cliente, Tecnico= @tecnico, Livello= @livello, Stato= @stato, Prodotto=" +
                " @prodotto, Categoria= @categoria, Priorita= @priorita, Oggetto= @oggetto, Messaggio= @messaggio, Comunicazione= @comunicazione " +
                "WHERE (ID => @ID)";

                MySqlCommand cmd = new MySqlCommand(ModificaTicket, con);

                //cmd.Parameters.Add("@cliente", MySqlDbType.VarChar).Value = cliente;
                //cmd.Parameters.Add("@tecnico", MySqlDbType.VarChar).Value = tecnico;
                //cmd.Parameters.Add("@livello", MySqlDbType.VarChar).Value = livello;
                //cmd.Parameters.Add("@stato", MySqlDbType.VarChar).Value = stato;
                cmd.Parameters.Add("@prodotto", MySqlDbType.VarChar).Value = prodotto;
                cmd.Parameters.Add("@categoria", MySqlDbType.VarChar).Value = categoria;
                //cmd.Parameters.Add("@priorita", MySqlDbType.VarChar).Value = priorita;
                cmd.Parameters.Add("@oggetto", MySqlDbType.VarChar).Value = oggetto;
                cmd.Parameters.Add("@messaggio", MySqlDbType.VarChar).Value = messaggio;
                cmd.Parameters.Add("@comunicazione", MySqlDbType.VarChar).Value = note;
                cmd.ExecuteNonQuery();

            }
        }
        public void clickElimina(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            ID = Tid.Text;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string deleteSocieta = "DELETE FROM `societa` [WHERE id== @ID]";
                MySqlCommand cmd = new MySqlCommand(deleteSocieta, con);

                cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = ID; //DOMANIIIIIIIIII 12/12/2025
                cmd.ExecuteNonQuery();
            }
        }

        protected void Storico(object sender, EventArgs e)
        {
            string tabella = "storico";


            this.NotifichePopup.Show(tabella);
}

        private void CampiCliente()
        {
            LProdotto.Visible = true;
            DProdotto.Visible = true;
            LCategoria.Visible = true;
            DCategoria.Visible = true;
            LOggetto.Visible = true;
            TOggetto.Visible = true;
            LMessaggio.Visible = true;
            TMessaggio.Visible = true;
            LComunicazione.Visible = true;
            TComunicazione.Visible = true;
            BCrea.Visible = true;
        }
        private void CampiTecnico()
        {
            LMessaggio.Visible = true;
            TMessaggio.Visible = true;
            BCrea.Visible = true;
            BModifica.Visible = true;
            IDlabelTicket.Visible = true;
        }
    }
}
