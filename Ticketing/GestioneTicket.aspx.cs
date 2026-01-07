using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Bcpg;
using System;
using System.Configuration;
using System.Drawing;
using System.Web.UI.WebControls;
using Ticketing.Models;

namespace Ticketing
{
    public partial class GestioneTicket : System.Web.UI.Page
    {
        private int id;
        private String cliente;
        private String tecnico;
        private String livello;
        private String stato;
        private String prodotto;
        private String categoria;
        private String priorita;
        private String oggetto;
        private String messaggio;
        private String note;
        utente user;
      
        private int DefaultStato = 1;
        private int currentUser;
        string to;
        string from;

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
            note = TMessaggio.Text.Trim();
            messaggio = TComunicazione.Text.Trim();

                using (MySqlConnection con = new MySqlConnection(cs))
                {
                    con.Open();
                    string nuovaTicket = $"INSERT INTO ticket (Cliente, Prodotto, Categoria, Stato, Titolo, Descrizione, note) VALUES (@{currentUser}, @prodotto, @categoria, @{DefaultStato}, @oggetto, @messaggio, @note);";
                    // bisogna creare le logiche di creazione delle notifiche ed email nel db



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
                
                    email.sendMail("info@dgs.it", user.Email, $"Creato nuovo ticket {id} ,{oggetto}", messaggio);
                // qui bisogna aggiungere la logica dei booleani per gestire le notifiche 
         
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


        public void clickElimina(object sender, EventArgs e)
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
            note = TMessaggio.Text.Trim();
            messaggio = TComunicazione.Text.Trim();

            ID = Tid.Text;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string deleteSocieta = "DELETE FROM `societa` [WHERE id== @ID]";
                MySqlCommand cmd = new MySqlCommand(deleteSocieta, con);

                cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = ID; //DOMANIIIIIIIIII 12/12/2025
                cmd.ExecuteNonQuery();

                if (user.Societa == null)
                {

                    string cliente = $"select email from clienti where id={TCliente.Text}";
                    MySqlCommand ver = new MySqlCommand(cliente, con);
                    MySqlDataReader reader = ver.ExecuteReader();

                cmd.Parameters.Add("@cliente", MySqlDbType.VarChar).Value = cliente;
                cmd.Parameters.Add("@tecnico", MySqlDbType.VarChar).Value = tecnico;
                cmd.Parameters.Add("@livello", MySqlDbType.VarChar).Value = livello;
                cmd.Parameters.Add("@stato", MySqlDbType.VarChar).Value = stato;
                cmd.Parameters.Add("@prodotto", MySqlDbType.VarChar).Value = prodotto;
                cmd.Parameters.Add("@categoria", MySqlDbType.VarChar).Value = categoria;
                cmd.Parameters.Add("@priorita", MySqlDbType.VarChar).Value = priorita;
                cmd.Parameters.Add("@oggetto", MySqlDbType.VarChar).Value = oggetto;
               // cmd.Parameters.Add("@messaggio", MySqlDbType.VarChar).Value = nota;
                cmd.Parameters.Add("@comunicazione", MySqlDbType.VarChar).Value = messaggio;
                cmd.ExecuteNonQuery();

                }
                else
                {
                    to = "tecnico";
                    from = user.Email;
                }

                if (user.Societa == null)
                {
                    to="teconico";

                }
                else
                {
                    to = "cliente";

                }
                email.sendMail(to, user.Email, $"Creato nuovo ticket {id}, {oggetto}", messaggio);
                // qui bisogna aggiungere la logica dei booleani per gestire le notifiche 

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
            LProdotto.Visible = true;
            DProdotto.Visible = true;
            LCategoria.Visible = true;
            DCategoria.Visible = true;
            LOggetto.Visible = true;
            TOggetto.Visible = true;
            LMessaggio.Visible = true;
            TMessaggio.Visible = true;
            BCrea.Visible = true;
            //BModifica.Visible = true;
            IDlabelTicket.Visible = true;
        }

       protected void MandaComunicazione(object sender, EventArgs e)
        {


            email.sendMail(to, from, oggetto, messaggio);
            // qui bisogna aggiungere la logica dei booleani per gestire le notifiche e il to e from delle email
        }
    }
}
