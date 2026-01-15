using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Configuration;
using System.Data;
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

        private int DefaultStato = 1;
        private int currentUser;
        utente user;
        ticket currentTicket;
        email comunicazione = new email();

        protected void Page_Load(object sender, EventArgs e)
        {
            user = Session["CR"] as utente;


            if (Session["CR"] != null)
            {
                user = Session["CR"] as utente;
                
                string welcomeMessage = $"Benvenuto,{user.Nome} {user.Cognome}!";
                if (!IsPostBack)
                {
                    string[] tik = new string[9];
                    if (Session["ticket"] == null)
                    {
                        for (int i = 0; i < tik.Length; i++) tik[i] = "";
                        CampiCliente(tik);
                    }
                    else
                    {
                        tik = Session["ticket"] as string[];
                        if (user?.Societa != 0) CampiCliente(tik);
                        else CampiTecnico(tik);
                    }
                    LoadStorico();
                }
                else
                {
                    // on postback do not rebind dropdowns
                }

                Session["ticket"] = null;
            }

            else
            {
                Response.Redirect("Login.aspx");
            }

        }
        private void LoadStorico()
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                MySqlCommand command = new MySqlCommand("SELECT * FROM storico", con);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                var table = new DataTable();
                adapter.Fill(table);

                Storico.DataSource = table;
                Storico.DataBind();

            }
        }
        //private bool IsTecnico(utente user)
        //{
        //    if (user != null && (user.Dipartimento > 1 || user.Dipartimento == 1))
        //    {
        //        return true;
        //    }
        //    return false;
        //}
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
                }
            }
        }
        public void clickCrea(object sender, EventArgs e)
        {
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

                prodotto = DProdotto.SelectedValue;
                categoria = DCategoria.SelectedValue; 
                oggetto = TOggetto.Text;
                messaggio = TMessaggio.Text;

                using (MySqlConnection con = new MySqlConnection(cs))
                {
                    con.Open();
                    string nuovaTicket = @"INSERT INTO ticket (Cliente, Prodotto, Categoria, Stato, Titolo, Descrizione) 
VALUES (@cliente, @prodotto, @categoria, @stato, @oggetto, @messaggio);
";

                    MySqlCommand cmd = new MySqlCommand(nuovaTicket, con);

                    cmd.Parameters.Add("@cliente", MySqlDbType.VarChar).Value = user.ID;
                    cmd.Parameters.Add("@prodotto", MySqlDbType.Int32).Value = prodotto;
                    cmd.Parameters.Add("@categoria", MySqlDbType.Int32).Value = categoria;
                    cmd.Parameters.Add("@stato", MySqlDbType.Int32).Value = DefaultStato;

                    cmd.Parameters.Add("@oggetto", MySqlDbType.VarChar).Value = oggetto;
                    cmd.Parameters.Add("@messaggio", MySqlDbType.Text).Value = messaggio;

                    cmd.ExecuteNonQuery();


                    //string idTicket = $"select ID,max(Creata_a) as max from `ticket` where id={TCliente.Text} AND Creata_a = max";
                    string idTicket = $"SELECT max(ID) as ID from ticket where cliente = {user.ID} ";
                    MySqlCommand ver = new MySqlCommand(idTicket, con);
                    MySqlDataReader reader = ver.ExecuteReader();
                    reader.Read();
                    id = reader.GetInt32("ID");
                }
                //email.sendMail(user.Email, "ticketingTest@outlook.it", $"Creato nuovo ticket {id} ,{oggetto}", messaggio);
                // qui bisogna aggiungere la logica dei booleani per gestire le notifiche 

                //using (MySqlConnection con = new MySqlConnection(cs))
                //{
                //    con.Open();
                //    string nuovaEmail = "INSERT INTO email (Sender, Receiver, Subject, Body) VALUES (@sender, @receiver, @subject, @body);";
                //    // bisogna creare le logiche di creazione delle notifiche ed email nel db



                //    MySqlCommand cmd = new MySqlCommand(nuovaEmail, con);

                //    cmd.Parameters.Add("@sender", MySqlDbType.VarChar).Value = user.Email;
                //    cmd.Parameters.Add("@receiver", MySqlDbType.VarChar).Value = "ticketingTest@outlook.it";
                //    cmd.Parameters.Add("@subject", MySqlDbType.VarChar).Value = oggetto;
                //    cmd.Parameters.Add("@body", MySqlDbType.VarChar).Value = messaggio;

                //    cmd.ExecuteNonQuery();
                //}
                comunicazione.subject = $"Creato ticket {id}, {oggetto}";
                comunicazione.body = $"L'utente {user.Nome} {user.Cognome} ha creato il ticket";
                email.sendMail(user.Email, "ticketdevtest@gmail.com", comunicazione.subject, comunicazione.body, id);

            }
            catch (MySqlException ex)
            {
                Response.Write("MySQL Error " + ex.Number + ": " + ex.Message);
            }
            //catch (Exception ex)
            //{
            //    Response.Write("Errore: " + ex.Message);
            //}
            Response.Write("<script>alert('Fatto')</script>");
        }

        
        public void clickElimina(object sender, EventArgs e)
        {
           string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            id = currentTicket.ID;
            cliente = TCliente.Text;
            tecnico = TTecnico.Text;
            livello = TLivello.Text;
            stato = DStato.Text;
            prodotto = DProdotto.Text;
            categoria = DCategoria.Text;
            priorita = DPriorita.Text;
            oggetto = TOggetto.Text;
            note = TMessaggio.Text;
            messaggio = TComunicazione.Text;

            
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string deleteSocieta = "DELETE FROM `ticket ` [WHERE id== @ID]";
                MySqlCommand cmd = new MySqlCommand(deleteSocieta, con);

                cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = ID; //DOMANIIIIIIIIII 12/12/2025
                cmd.ExecuteNonQuery();

                if (user.Societa == null)
                {

                    string cliente = $"select email from `clienti` where id={TCliente.Text} ";
                    MySqlCommand ver = new MySqlCommand(cliente, con);
                    MySqlDataReader reader = ver.ExecuteReader();
                    reader.Read();
                    comunicazione.receiver = reader.GetString("email");
                    comunicazione.mailer = user.Email;

                    

                }
                else
                {
                    string cliente = $"select email from `tecnici` where id={TTecnico.Text} ";
                    MySqlCommand ver = new MySqlCommand(cliente, con);
                    MySqlDataReader reader = ver.ExecuteReader();
                    reader.Read();
                    comunicazione.receiver = reader.GetString("email");
                    comunicazione.mailer = user.Email;
                }
                comunicazione.subject = $"Eliminato ticket {id}, {oggetto}";
                comunicazione.body = $"L'utente {user.Nome} {user.Cognome} ha eliminato il ticket";
                email.sendMail(comunicazione.receiver, comunicazione.mailer, comunicazione.subject, comunicazione.body, id);
                
                // qui bisogna aggiungere la logica dei booleani per gestire le notifiche 

            }
        }

        //protected void Storico(object sender, EventArgs e)
        //{
        //    string tabella = "storico";

        //    this.NotifichePopup.Show(tabella);
        //}


       private void CCSeleziona()
        {
            LStato.Visible = true;
            string stato = DStato.Text;
            LLStato.Visible = true;
            LLStato.Text = stato;
        }
        private void CampiCliente(string[] tik)
        {
            if (tik[0].Equals(""))
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
                LoadDropDownList("SELECT ID,Prodotto FROM prodotto;", DProdotto, "Scegli un prodotto:");
                LoadDropDownList("SELECT ID,Categoria FROM categoria ORDER BY Categoria;", DCategoria, "Scegli una categoria:");
            }
            else
            {
                LStato.Visible = true;
                LStato.Text += " : " + tik[3];
                LProdotto.Visible = true;
                LProdotto.Text += " : " + tik[4];
                //DProdotto.Visible = true;
                LCategoria.Visible = true;
                LCategoria.Text += " : " + tik[5];
                //DCategoria.Visible = true;
                LOggetto.Visible = true;
                TOggetto.Visible = true;
                TOggetto.Text = tik[7];
                LMessaggio.Visible = true;
                TMessaggio.Visible = true;
                TMessaggio.Text = tik[6];
                LComunicazione.Visible = true;
                TComunicazione.Visible = true;
                BChiudi.Visible = true;

                
            }
            //BCrea.Visible = true;
            //if (!IsPostBack)
            //{
            //    LoadDropDownList("SELECT ID,Prodotto FROM prodotto;", DProdotto, "Scegli un prodotto:");
            //    LoadDropDownList("SELECT ID,Categoria FROM categoria ORDER BY Categoria;", DCategoria, "Scegli una categoria:");

            //}
        
        }
        private void CampiTecnico(string[] tik)
        {
            LCliente.Visible = true;
            LCliente.Text += " : " + tik[0];
            LTecnico.Visible = true;
            LTecnico.Text += " : " + tik[1];
            LLivello.Visible = true;
            LLivello.Text += " : " + tik[2];
            LStato.Visible = true;
            LStato.Text += " : " + tik[3];
            LProdotto.Visible = true;
            LProdotto.Text += " : " + tik[4];
            //DProdotto.Visible = true;
            LCategoria.Visible = true;
            LCategoria.Text += " : " + tik[5];
            //DCategoria.Visible = true;
            LOggetto.Visible = true;
            TOggetto.Visible = true;
            TOggetto.Text = tik[7];
            LMessaggio.Visible = true;
            TMessaggio.Visible = true;
            TMessaggio.Text = tik[6];
            LPriorita.Visible = true;
            LPriorita.Text += " : " + tik[8];
            LComunicazione.Visible = true;
            TComunicazione.Visible = true;
            BRisposta.Visible = true;
            BAnnulla.Visible = true;
            BChiudi.Visible = true;
        }
        //private void GetTicketById(int ticketId)
        //{
        //    //ticket t = null;

        //    string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

        //    using (MySqlConnection con = new MySqlConnection(cs))
        //    {
        //        using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM detailsticket WHERE ID = @id", con))
        //        {
        //            cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = ticketId;
        //            con.Open();

        //            cmd.ExecuteNonQuery();

        //        }
        //    }
        //}
        protected void MandaComunicazione(object sender, EventArgs e)
        {


            email.sendMail(comunicazione.receiver, comunicazione.mailer, oggetto, messaggio,currentTicket.ID);
            // qui bisogna aggiungere la logica dei booleani per gestire le notifiche e il to e from delle email
        }

        protected void Annulla(object sender, EventArgs e)
        {


            Response.Redirect("Dashboard.aspx");
        }
    }
}


//using (MySqlDataReader dr = cmd.ExecuteReader())
                //{
                    //if (dr.Read())
                    //{
                    //    int oID = dr.GetOrdinal("ID");
                    //    int oCliente = dr.GetOrdinal("Cliente");
                    //    int oTecnico = dr.GetOrdinal("Tecnico");
                    //    int oLivello = dr.GetOrdinal("Livello");
                    //    int oStato = dr.GetOrdinal("Stato");
                    //    int oProdotto = dr.GetOrdinal("Prodotto");
                    //    int oCategoria = dr.GetOrdinal("Categoria");
                    //    int oPriorita = dr.GetOrdinal("Priorita");
                    //    int oTitolo = dr.GetOrdinal("Titolo");
                    //    int oDescrizione = dr.GetOrdinal("Descrizione");
                    //    int oNote = dr.GetOrdinal("Note");
                    //    int oCreata = dr.GetOrdinal("creata_a");

                    //    t = new ticket
                    //    {
                    //        ID = dr.IsDBNull(oID) ? 0 : dr.GetInt32(oID),
                    //        Cliente = dr.IsDBNull(oCliente) ? 0 : dr.GetInt32(oCliente),

                    //        Tecnico = dr.IsDBNull(oTecnico) ? 0 : dr.GetInt32(oTecnico),
                    //        Livello = dr.IsDBNull(oLivello) ? 0 : dr.GetInt32(oLivello),
                    //        Stato = dr.IsDBNull(oStato) ? 0 : dr.GetInt32(oStato),
                    //        Prodotto = dr.IsDBNull(oProdotto) ? 0 : dr.GetInt32(oProdotto),
                    //        Categoria = dr.IsDBNull(oCategoria) ? 0 : dr.GetInt32(oCategoria),
                    //        Priorita = dr.IsDBNull(oPriorita) ? 0 : dr.GetInt32(oPriorita),

                    //        Titolo = dr.IsDBNull(oTitolo) ? "" : dr.GetString(oTitolo),
                    //        Descrizione = dr.IsDBNull(oDescrizione) ? "" : dr.GetString(oDescrizione),

                    //        Note = dr.IsDBNull(oNote) ? null : dr.GetString(oNote),

                    //        Create_a = dr.IsDBNull(oCreata) ? DateTime.MinValue : dr.GetDateTime(oCreata)
                    //    };
                    //}
                //}
            //}

            //return t;