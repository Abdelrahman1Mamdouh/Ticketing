using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using Ticketing.Models;

namespace Ticketing
{
    public partial class GestioneTicket : System.Web.UI.Page
    {
        private String cliente;
        private String tecnico;
        private String livello;
        private String stato;
        private String prodotto;
        private String categoria;
        private String priorita;
        private String oggetto;
        private String messaggio;
        private String comunicazione;

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

        public void clickCrea(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            cliente = TCliente.Text.Trim();
            tecnico = TTecnico.Text.Trim();
            livello = TLivello.Text.Trim();
            stato = DStato.Text.Trim();
            prodotto = DProdotto.Text.Trim();
            categoria = DCategoria.Text.Trim();
            priorita = DPriorita.Text.Trim();
            oggetto = TOggetto.Text.Trim();
            messaggio = TMessaggio.Text.Trim();
            comunicazione = TComunicazione.Text.Trim();

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string newSocieta = "INSERT INTO societa " +
                    "(Cliente, Tecnico, Livello, Stato, Prodotto, Categoria, Priorita, Oggetto, Messaggio, Comunicazione) " +
                    "VALUES (@cliente, @tecnico, @livello, @stato, @prodotto, @categoria, @priorita, @oggetto, @messaggio, @comunicazione)";

                MySqlCommand cmd = new MySqlCommand(newSocieta, con);

                cmd.Parameters.Add("@cliente", MySqlDbType.VarChar).Value = cliente;
                cmd.Parameters.Add("@tecnico", MySqlDbType.VarChar).Value = tecnico;
                cmd.Parameters.Add("@livello", MySqlDbType.VarChar).Value = livello;
                cmd.Parameters.Add("@stato", MySqlDbType.VarChar).Value = stato;
                cmd.Parameters.Add("@prodotto", MySqlDbType.VarChar).Value = prodotto;
                cmd.Parameters.Add("@categoria", MySqlDbType.VarChar).Value = categoria;
                cmd.Parameters.Add("@priorita", MySqlDbType.VarChar).Value = priorita;
                cmd.Parameters.Add("@oggetto", MySqlDbType.VarChar).Value = oggetto;
                cmd.Parameters.Add("@messaggio", MySqlDbType.VarChar).Value = messaggio;
                cmd.Parameters.Add("@comunicazione", MySqlDbType.VarChar).Value = comunicazione;
                cmd.ExecuteNonQuery();
            }
        }


        public void clickModifica(object sender, EventArgs e)
        {
            String cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            cliente = TCliente.Text.Trim();
            tecnico = TTecnico.Text.Trim();
            livello = TLivello.Text.Trim();
            stato = DStato.Text.Trim();
            prodotto = DProdotto.Text.Trim();
            categoria = DCategoria.Text.Trim();
            priorita = DPriorita.Text.Trim();
            oggetto = TOggetto.Text.Trim();
            messaggio = TMessaggio.Text.Trim();
            comunicazione = TComunicazione.Text.Trim();

            using (MySqlConnection con = new MySqlConnection(cs))

            {
                con.Open();
                string ModificaSocieta =
                "UPDATE societa SET Cliente= @cliente, Tecnico= @tecnico, Livello= @livello, Stato= @stato, Prodotto= @prodotto, Categoria= @categoria, Priorita= @priorita, Oggetto= @oggetto, Messaggio= @messaggio, Comunicazione= @comunicazione WHERE (ID => @ID)";

                MySqlCommand cmd = new MySqlCommand(ModificaSocieta, con);

                cmd.Parameters.Add("@cliente", MySqlDbType.VarChar).Value = cliente;
                cmd.Parameters.Add("@tecnico", MySqlDbType.VarChar).Value = tecnico;
                cmd.Parameters.Add("@livello", MySqlDbType.VarChar).Value = livello;
                cmd.Parameters.Add("@stato", MySqlDbType.VarChar).Value = stato;
                cmd.Parameters.Add("@prodotto", MySqlDbType.VarChar).Value = prodotto;
                cmd.Parameters.Add("@categoria", MySqlDbType.VarChar).Value = categoria;
                cmd.Parameters.Add("@priorita", MySqlDbType.VarChar).Value = priorita;
                cmd.Parameters.Add("@oggetto", MySqlDbType.VarChar).Value = oggetto;
                cmd.Parameters.Add("@messaggio", MySqlDbType.VarChar).Value = messaggio;
                cmd.Parameters.Add("@comunicazione", MySqlDbType.VarChar).Value = comunicazione;
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
    }
}
