using Microsoft.Ajax.Utilities;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using Ticketing.Models;

namespace Ticketing
{
    public partial class GestioneTicket : System.Web.UI.Page
    {
        private int id;
        private string cliente;
        private string tecnico;
        private string livello;
        private string stato;
        private string prodotto;
        private string categoria;
        private string priorita;
        private string oggetto;
        private string messaggio;
        private string note;

        private int DefaultStato = 1;

        utente user;
        ticket currentTicket = new ticket();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CR"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            user = Session["CR"] as utente;

            // ✅ ticketId is OPTIONAL: only exists in details mode
            bool hasTicketId = int.TryParse(Request.QueryString["id"], out int ticketId);

            // If we have an id -> we are opening an existing ticket
            if (hasTicketId)
            {
                Tid.Text = ticketId.ToString();
                currentTicket.ID = ticketId;
            }

            if (!IsPostBack)
            {
                string[] tik = new string[9];

                if (Session["ticket"] == null)
                {
                    // ✅ CREATE MODE
                    for (int i = 0; i < tik.Length; i++) tik[i] = "";
                    CampiCliente(tik);

                    // ❗ don't load storico because there is no ticket id yet
                }
                else
                {
                    // ✅ DETAILS MODE (opened from dashboard)
                    tik = Session["ticket"] as string[];

                    if (user?.Societa != 0) CampiCliente(tik);
                    else CampiTecnico(tik);

                    // Load storico ONLY if we have a valid ticketId
                    if (hasTicketId)
                        LoadStorico(ticketId);
                }

                Session["ticket"] = null;
            }
        }
        public static void LoadDropDownList(string query, DropDownList DDL, string label)
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
            }
        }
        public async void clickCrea(object sender, EventArgs e)
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

                    string nuovaTicket = @"
                        INSERT INTO ticket (Cliente, Prodotto, Categoria, Stato, Titolo, Descrizione) 
                        VALUES (@cliente, @prodotto, @categoria, @stato, @oggetto, @messaggio);";

                    MySqlCommand cmd = new MySqlCommand(nuovaTicket, con);

                    cmd.Parameters.Add("@cliente", MySqlDbType.Int32).Value = user.ID;
                    cmd.Parameters.Add("@prodotto", MySqlDbType.Int32).Value = prodotto;
                    cmd.Parameters.Add("@categoria", MySqlDbType.Int32).Value = categoria;
                    cmd.Parameters.Add("@stato", MySqlDbType.Int32).Value = DefaultStato;
                    cmd.Parameters.Add("@oggetto", MySqlDbType.VarChar).Value = oggetto;
                    cmd.Parameters.Add("@messaggio", MySqlDbType.Text).Value = messaggio;

                    cmd.ExecuteNonQuery();

                    string idLastTicket = $"SELECT max(ID) as ID from ticket where cliente = {user.ID}";
                    MySqlCommand ver = new MySqlCommand(idLastTicket, con);
                    MySqlDataReader reader = ver.ExecuteReader();
                    reader.Read();
                    id = reader.GetInt32("ID");
                }
            }
            catch (MySqlException ex)
            {
                Response.Write("MySQL Error " + ex.Number + ": " + ex.Message);
            }

            Response.Write("<script>alert('Fatto')</script>");
        }
        protected void MandaComunicazione(object sender, EventArgs e)
        {
            string testo = TComunicazione.Text;

            // ✅ (Fix #3) safer parse
            if (!int.TryParse(Tid.Text, out int ticketId))
            {
                Response.Write("<script>alert('TicketID non valido')</script>");
                return;
            }

            // ✅ (Fix #4) If tecnico not assigned OR user not in ticket -> null
            string destinatario = GetAltroEmail(ticketId, user.ID);

            if (string.IsNullOrEmpty(destinatario))
            {
                Response.Write("<script>alert('Destinatario non disponibile (ticket non assegnato o accesso non valido)')</script>");
                return;
            }

            SaveToStorico(ticketId, user.Email, destinatario, testo);

            TComunicazione.Text = "";
            LoadStorico(ticketId);
        }
        private void LoadStorico(int idTicket)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                // ✅ (Fix #1) WHERE TicketID (not Ticket)
                // Keep Oggetto if your storico has it; if not, remove it from SELECT.
                var cmd = new MySqlCommand(@"
                    SELECT ID, nomeMittente, nomeDestinatario, Oggetto, Messaggio
                    FROM storico
                    WHERE Ticket = @TicketID
                    ORDER BY ID ASC;", con);

                cmd.Parameters.Add("@TicketID", MySqlDbType.Int32).Value = idTicket;

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                var table = new DataTable();
                adapter.Fill(table);

                Storico.DataSource = table;
                Storico.DataBind();
            }
        }
        private void SaveToStorico(int ticketId, string mittente, string destinatario, string messaggio)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            using (var con = new MySqlConnection(cs))
            {
                con.Open();

                var cmd = new MySqlCommand(@"
                    INSERT INTO storico (Ticket,idMittente ,nomeMittente, nomeDestinatario, Messaggio, letturaNotifica)
                    VALUES (@TicketID,@idMittente, @Mittente, @Destinatario, @Messaggio, @letturaNotifica);", con);

                cmd.Parameters.Add("@TicketID", MySqlDbType.Int32).Value = ticketId;
                cmd.Parameters.Add("@idMittente", MySqlDbType.Int32).Value = user.ID;
                cmd.Parameters.Add("@Mittente", MySqlDbType.VarChar).Value = mittente;
                cmd.Parameters.Add("@Destinatario", MySqlDbType.VarChar).Value = destinatario;
                cmd.Parameters.Add("@letturaNotifica", MySqlDbType.Int32).Value = 0;

                // ✅ (Fix #5) use TEXT (messages can be long)
                cmd.Parameters.Add("@Messaggio", MySqlDbType.Text).Value = messaggio ?? "";

                cmd.ExecuteNonQuery();
            }
        }
        private string GetAltroEmail(int ticketId, int currentUserId)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            using (var con = new MySqlConnection(cs))
            {
                con.Open();

                var cmd = new MySqlCommand(@"
                    SELECT 
                        t.Cliente,
                        t.Tecnico,
                        uc.Email AS ClienteEmail,
                        ut.Email AS TecnicoEmail
                    FROM ticket t
                    JOIN utente uc ON uc.ID = t.Cliente
                    LEFT JOIN utente ut ON ut.ID = t.Tecnico
                    WHERE t.ID = @TicketId;", con);

                cmd.Parameters.Add("@TicketId", MySqlDbType.Int32).Value = ticketId;

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return null;

                    int clientId = reader.GetInt32("Cliente");
                    string clienteEmail = reader.GetString("ClienteEmail");

                    object tecnicoObj = reader["Tecnico"];
                    int? tecnicoId = tecnicoObj == DBNull.Value ? (int?)null : Convert.ToInt32(tecnicoObj);

                    string tecnicoEmail =
                        tecnicoId != null && reader["TecnicoEmail"] != DBNull.Value
                            ? reader.GetString("TecnicoEmail")
                            : null;

                    // if current user is CLIENTE -> return TECNICO email
                    if (currentUserId == clientId)
                        return tecnicoEmail;

                    // if current user is TECNICO -> return CLIENTE email
                    if (tecnicoId != null && currentUserId == tecnicoId)
                        return clienteEmail;

                    // user not part of this ticket
                    return null;
                }
            }
        }
        protected void Annulla(object sender, EventArgs e)
        {
            Response.Redirect("Dashboard.aspx");
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
                LCliente.Visible = true;
                LCliente.Text += " : " + tik[0];

                LTecnico.Visible = true;
                LTecnico.Text += " : " + tik[1];

                LStato.Visible = true;
                LStato.Text += " : " + tik[3];

                LProdotto.Visible = true;
                LProdotto.Text += " : " + tik[4];

                LCategoria.Visible = true;
                LCategoria.Text += " : " + tik[5];

                LOggetto.Visible = true;
                TOggetto.Visible = true;
                TOggetto.Text = tik[7];

                LMessaggio.Visible = true;
                TMessaggio.Visible = true;
                TMessaggio.Text = tik[6];

                LComunicazione.Visible = true;
                TComunicazione.Visible = true;

                BChiudi.Visible = true;
                BRisposta.Visible = true;
                BAnnulla.Visible = true;
            }
        }
        private void CampiTecnico(string[] tik)
        {
            LCliente.Visible = true;
            LCliente.Text += " : " + tik[0];

            LTecnico.Visible = true;
            LTecnico.Text += " : " + tik[1];

            LLivello.Visible = true;
            LLivello.Text += " : " + tik[2];
            DLivello.Visible = true;

            LStato.Visible = true;
            LStato.Text += " : " + tik[3];

            LProdotto.Visible = true;
            LProdotto.Text += " : " + tik[4];

            LCategoria.Visible = true;
            LCategoria.Text += " : " + tik[5];

            LOggetto.Visible = true;
            TOggetto.Visible = true;
            TOggetto.Text = tik[7];

            LMessaggio.Visible = true;
            TMessaggio.Visible = true;
            TMessaggio.Text = tik[6];

            LPriorita.Visible = true;
            LPriorita.Text += " : " + tik[8];
            DPriorita.Visible = true;

            LComunicazione.Visible = true;
            TComunicazione.Visible = true;

         
            if (livello == null && priorita == null && tik[2].IsNullOrWhiteSpace()/*Llivello*/ && tik[8].IsNullOrWhiteSpace()/*LPriorita*/)
            {
                BbSalva.Visible = true;
            }
            else
            {
                BbSalva.Visible = false;
                DLivello.Enabled = false;
                DPriorita.Enabled = false;
            }


            if (user.Ruolo == 3)
            {
                DTecnici.Visible = true;
                BbAssegna.Visible = true;
                BbSalva.Visible = true;
                DLivello.Enabled = true;
                DPriorita.Enabled = true;

            }

            BRisposta.Visible = true;
            BAnnulla.Visible = true;
            BChiudi.Visible = true;

            if (tik[1].IsNullOrWhiteSpace() && user.Ruolo == 2)
            {
                BbAssegna.Visible = true;
            }
            else
            {
                BbAssegna.Visible = false;
            }

            if (!IsPostBack)
            {
                LoadDropDownList("SELECT ID,livello FROM Livello;", DLivello, "Scegli il livello:");
                LoadDropDownList("SELECT ID,priorita FROM Priorita;", DPriorita, "Scegli la priorita:");
                LoadDropDownList("SELECT ID,Nome FROM tecnici;", DTecnici, "Scegli il Tecnico:");
            }
        }
        protected void ClickAssegna(object sender, EventArgs e)
        {
            if (user.Ruolo == 3)
            {
                string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

                using (MySqlConnection con = new MySqlConnection(cs))
                {
                    con.Open();
                    string nuovaTicket = $"UPDATE ticket SET Tecnico=@tecnico WHERE ID=@id";

                    using (MySqlCommand cmd = new MySqlCommand(nuovaTicket, con))
                    {
                        cmd.Parameters.Add("@Tecnico", MySqlDbType.VarChar).Value = DTecnici.SelectedValue;
                        cmd.Parameters.AddWithValue("@id", MySqlDbType.Int32).Value = currentTicket.ID;
                        cmd.ExecuteNonQuery();
                    }
                }
                Response.Write("<script>alert('Fatto')</script>");
            }
            else
            {
                string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

                using (MySqlConnection con = new MySqlConnection(cs))
                {
                    con.Open();
                    string nuovaTicket = $"UPDATE ticket SET Tecnico=@tecnico WHERE ID=@id";

                    using (MySqlCommand cmd = new MySqlCommand(nuovaTicket, con))
                    {
                        cmd.Parameters.Add("@Tecnico", MySqlDbType.VarChar).Value = user.ID;
                        cmd.Parameters.AddWithValue("@id", MySqlDbType.Int32).Value = currentTicket.ID;
                        cmd.ExecuteNonQuery();
                    }
                }
                Response.Write("<script>alert('Fatto')</script>");
            }
        }
        protected void ClickSalva(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            livello = DLivello.SelectedValue;
            priorita = DPriorita.SelectedValue;
            if (livello != null && priorita.Equals(""))
            {
                using (MySqlConnection con = new MySqlConnection(cs))
                {
                    con.Open();
                    string updateLivelloPriorita = $"UPDATE ticket SET Livello=@livello WHERE ID=@id";
                    using (MySqlCommand cmd = new MySqlCommand(updateLivelloPriorita, con))
                    {
                        cmd.Parameters.Add("@livello", MySqlDbType.Int32).Value = livello;
                        cmd.Parameters.AddWithValue("@id", MySqlDbType.Int32).Value = currentTicket.ID;
                        cmd.ExecuteNonQuery();
                    }
                }
                Response.Write("<script>alert('Livello is updated')</script>");
            }
            else if (priorita != null && livello.Equals(""))
            {
                using (MySqlConnection con = new MySqlConnection(cs))
                {
                    con.Open();
                    string updateLivelloPriorita = $"UPDATE ticket SET Priorita=@priorita WHERE ID=@id";
                    using (MySqlCommand cmd = new MySqlCommand(updateLivelloPriorita, con))
                    {
                        cmd.Parameters.Add("@priorita", MySqlDbType.Int32).Value = priorita;
                        cmd.Parameters.AddWithValue("@id", MySqlDbType.Int32).Value = currentTicket.ID;
                        cmd.ExecuteNonQuery();
                    }
                }
                Response.Write("<script>alert('priorita is updated')</script>");
            }
            else
            {
                using (MySqlConnection con = new MySqlConnection(cs))
                {
                    con.Open();
                    string updateLivelloPriorita = $"UPDATE ticket SET Livello=@livello, Priorita=@priorita WHERE ID=@id";
                    using (MySqlCommand cmd = new MySqlCommand(updateLivelloPriorita, con))
                    {
                        cmd.Parameters.Add("@livello", MySqlDbType.Int32).Value = livello;
                        cmd.Parameters.Add("@priorita", MySqlDbType.Int32).Value = priorita;
                        cmd.Parameters.AddWithValue("@id", MySqlDbType.Int32).Value = currentTicket.ID;
                        cmd.ExecuteNonQuery();
                    }
                }
                Response.Write("<script>alert('priorita&livello are updated')</script>");
            }
        }
        
    }
}
