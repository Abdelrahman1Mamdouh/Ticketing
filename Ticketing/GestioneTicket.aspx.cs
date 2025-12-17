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

            if (!IsPostBack)
            {
                LoadDropDownList("SELECT ID,Prodotto FROM prodotto;", DProdotto , "Scegli un prodotto:");
                LoadDropDownList("SELECT ID,Categoria FROM categoria ORDER BY Categoria;" , DCategoria , "Scegli una categoria:");
            }
        }
        private void LoadDropDownList(string query, DropDownList DDL , string label)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            using (var con = new MySqlConnection(cs))
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

        public void clickCrea(object sender, EventArgs e)
        {
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
                string newSocieta = "INSERT INTO ticket (Prodotto, Categoria, Titolo, Descrizione, note) VALUES (@prodotto, @categoria, @oggetto, @messaggio, @note);";

                MySqlCommand cmd = new MySqlCommand(newSocieta, con);

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
                string ModificaSocieta =
                "UPDATE societa SET Cliente= @cliente, Tecnico= @tecnico, Livello= @livello, Stato= @stato, Prodotto=" +
                " @prodotto, Categoria= @categoria, Priorita= @priorita, Oggetto= @oggetto, Messaggio= @messaggio, Comunicazione= @comunicazione " +
                "WHERE (ID => @ID)";

                MySqlCommand cmd = new MySqlCommand(ModificaSocieta, con);

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
    }
}
