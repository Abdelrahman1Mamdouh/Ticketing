using Microsoft.Ajax.Utilities;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ticketing.Models;

namespace Ticketing
{
    public partial class GestioneTicket : System.Web.UI.Page
    {
        private int id;
        private string livello;
        private string prodotto;
        private string categoria;
        private string priorita;
        private string oggetto;
        private string messaggio;

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

            bool hasTicketId = int.TryParse(Request.QueryString["id"], out int ticketId);

            if (hasTicketId)
            {
                Tid.Text = ticketId.ToString();
                currentTicket.ID = ticketId;
            }

            if (!IsPostBack)
            {
                // --- START NOTIFICATION UPDATE ---
                // If we are viewing a ticket, mark notifications for this ticket/user as read
                if (hasTicketId && user != null)
                {
                    string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
                    using (MySqlConnection con = new MySqlConnection(cs))
                    {
                        con.Open();
                        // Update notifications for THIS ticket and THIS user email
                        string sql = "UPDATE storico SET letturaNotifica = 1 WHERE Ticket = @tickId AND nomeDestinatario = @userEmail";
                        using (MySqlCommand cmd = new MySqlCommand(sql, con))
                        {
                            cmd.Parameters.Add("@tickId", MySqlDbType.Int32).Value = ticketId;
                            cmd.Parameters.Add("@userEmail", MySqlDbType.VarChar).Value = user.Email;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                // --- END NOTIFICATION UPDATE ---

                string[] tik = new string[9];

                if (Session["ticket"] == null)
                {
                    for (int i = 0; i < tik.Length; i++) tik[i] = "";
                    CampiCliente(tik);
                }
                else
                {
                    //opened from dashboard
                    tik = Session["ticket"] as string[];

                    if (user?.Societa != 0) CampiCliente(tik);
                    else CampiTecnico(tik);


                    if (hasTicketId && IsTicketClosed(ticketId))
                    {
                        LoadStorico(ticketId);
                        SetTicketReadOnlyUI();
                    }
                    else if (hasTicketId)
                    {
                        LoadStorico(ticketId);
                    }

                    Session["ticket"] = null;

                }
                LoadStatiFromDb();
            }
        }
        public static void LoadDropDownList2(string query, DropDownList DDL, string label)
        {
            // Clear existing items
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            // Bind data to DropDownList
            using (var con = new MySqlConnection(cs))
            using (var cmd = new MySqlCommand(query, con))
            {
                con.Open();
                using (var dr = cmd.ExecuteReader())
                {
                    DDL.DataSource = dr;
                    DDL.DataBind();
                }
                // Add default item at the top
                DDL.Items.Insert(0, new ListItem(label, ""));
                DDL.Items[0].Attributes["disabled"] = "disabled";
                DDL.Items[0].Attributes["selected"] = "selected";
            }
        }
        public static void LoadDropDownList(DropDownList DDL, string label)
        {
            // Add default item at the top
            DDL.Items.Insert(0, new ListItem(label, ""));
            DDL.Items[0].Attributes["disabled"] = "disabled";
            DDL.Items[0].Attributes["selected"] = "selected";
        }
        // Create a new ticket
        protected void ClickCrea(object sender, EventArgs e)
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
                    reader.Close();
                }
                using (MySqlConnection con = new MySqlConnection(cs))
                {
                    con.Open();
                    string idLastTicket = $"SELECT max(ID) as ID from ticket where cliente = {user.ID}";
                    MySqlCommand ver = new MySqlCommand(idLastTicket, con);
                    MySqlDataReader reader = ver.ExecuteReader();
                    reader.Read();
                    id = reader.GetInt32("ID");
                    reader.Close();
                }
            }
            catch (MySqlException ex)
            {
                Response.Write("MySQL Error " + ex.Number + ": " + ex.Message);
            }
            NotificaCrea();
        }
        // Send a communication message
        protected void MandaComunicazione(object sender, EventArgs e)
        {
            string testo = TComunicazione.Text;

            if (!int.TryParse(Tid.Text, out int ticketId))
            {
                Response.Write("<script>alert('TicketID non valido')</script>");
                return;
            }

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
        // Load storico messages for a ticket
        private void LoadStorico(int idTicket)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();


                var cmd = new MySqlCommand(@"
                    SELECT nomeMittente, nomeDestinatario, Messaggio,Creata_a
                    FROM storico
                    WHERE Ticket = @TicketID
                    ORDER BY Creata_a ASC;", con);

                cmd.Parameters.Add("@TicketID", MySqlDbType.Int32).Value = idTicket;

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                var table = new DataTable();
                adapter.Fill(table);

                Storico.DataSource = table;
                Storico.DataBind();
            }
        }
        // Save a message to storico
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
                cmd.Parameters.Add("@Messaggio", MySqlDbType.Text).Value = messaggio ?? "";

                cmd.ExecuteNonQuery();
            }
        }
        // Get the email of the other party in the ticket
        private string GetAltroEmail(int ticketId, int currentUserId)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            using (var con = new MySqlConnection(cs))
            {
                con.Open();

                // Query to get Cliente and Tecnico emails
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
                // Execute the query
                using (var reader = cmd.ExecuteReader())
                {
                    // If no record found, return null
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
            TComunicazione.Text = "";
        }
        protected void Chiudi(object sender, EventArgs e)
        {
            Response.Redirect("Dashboard.aspx");
        }
        //mostra campi cliente
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

                LoadDropDownList(DProdotto, "Scegli un prodotto:");
                LoadDropDownList(DCategoria, "Scegli una categoria:");
            }
            else
            {
                LCliente.Visible = true;
                LCliente.Text += " : " + tik[0];

                LTecnico.Visible = true;
                LTecnico.Text += " : " + tik[1];

                LStato.Visible = true;
                LStato.Text += " : " + tik[3];


                string currentFullName = (user.Nome + " " + user.Cognome).Trim();
                string ticketClienteName = (tik[0] ?? "").Trim();



                LProdotto.Visible = true;
                LProdotto.Text += " : " + tik[4];

                LCategoria.Visible = true;
                LCategoria.Text += " : " + tik[5];

                LOggetto.Visible = true;
                TOggetto.Visible = true;
                TOggetto.Text = tik[7];
                TOggetto.ReadOnly = true;

                LMessaggio.Visible = true;
                TMessaggio.Visible = true;
                TMessaggio.Text = tik[6];
                TMessaggio.ReadOnly = true;

                BChiudi.Visible = true;

                if (user.Ruolo == 1 || user.Ruolo == 4)
                {
                    if (ticketClienteName.ToLower() == currentFullName.ToLower())
                    {
                        DStato.Visible = true;
                        BCambiaStato.Visible = true;
                        LComunicazione.Visible = true;
                        TComunicazione.Visible = true;
                        BRisposta.Visible = true;
                        BAnnulla.Visible = true;
                    }
                    else
                    {
                        DStato.Visible = false;
                        BCambiaStato.Visible = false;
                        LComunicazione.Visible = false;
                        TComunicazione.Visible = false;
                        BRisposta.Visible = false;
                        BAnnulla.Visible = false;
                    }
                }

            }
        }
        //mostra campi tecnico
        private void CampiTecnico(string[] tik)
        {
            LCliente.Visible = true;
            LCliente.Text += " : " + tik[0];

            LTecnico.Visible = true;
            LTecnico.Text += " : " + tik[1];

            LLivello.Visible = true;
            LLivello.Text += " : " + tik[2];
            DLivelllo.Visible = true;

            LStato.Visible = true;
            LStato.Text += " : " + tik[3];
            DStato.Visible = true;
            BCambiaStato.Visible = true;

            LProdotto.Visible = true;
            LProdotto.Text += " : " + tik[4];

            LCategoria.Visible = true;
            LCategoria.Text += " : " + tik[5];

            LOggetto.Visible = true;
            TOggetto.Visible = true;
            TOggetto.Text = tik[7];
            TOggetto.ReadOnly = true;

            LMessaggio.Visible = true;
            TMessaggio.Visible = true;
            TMessaggio.Text = tik[6];
            TMessaggio.ReadOnly = true;

            LPriorita.Visible = true;
            LPriorita.Text += " : " + tik[8];
            DPriorita.Visible = true;

            LComunicazione.Visible = true;
            TComunicazione.Visible = true;

            BbAssegna.Visible = true;
            BbSalva.Visible = true;
            DLivelllo.Enabled = true;
            DPriorita.Enabled = true;
            DStato.Visible = true;

            BRisposta.Visible = true;
            BAnnulla.Visible = true;
            BChiudi.Visible = true;



            string currentFullName = (user.Nome + " " + user.Cognome).Trim();
            string ticketTecnicoName = (tik[1] ?? "").Trim();

            // ADMIN 
            if (user.Ruolo == 3)
            {
                BbAssegna.Visible = true;
                DTecnici.Visible = true;
                BbSalva.Visible = true;
                BCambiaStato.Visible = true;
                DLivelllo.Enabled = true;
                DPriorita.Enabled = true;
                DStato.Enabled = true;

                if (ticketTecnicoName.ToLower() == currentFullName.ToLower())
                {
                    LComunicazione.Visible = true;
                    TComunicazione.Visible = true;
                    BRisposta.Visible = true;
                    BAnnulla.Visible = true;
                }
                else
                {
                    LComunicazione.Visible = false;
                    TComunicazione.Visible = false;
                    BRisposta.Visible = false;
                    BAnnulla.Visible = false;
                }
            }
            //TECNICO ASSEGNATO A SE STESSO
            else if (ticketTecnicoName != "" &&
                     ticketTecnicoName.ToLower() == currentFullName.ToLower())
            {
                BbAssegna.Visible = false;
                BbSalva.Visible = true;
                BCambiaStato.Visible = true;
                DLivelllo.Enabled = true;
                DPriorita.Enabled = true;
                DStato.Enabled = true;

                LComunicazione.Visible = true;
                TComunicazione.Visible = true;
                BRisposta.Visible = true;
                BAnnulla.Visible = true;
            }//TECNICO NON ASSEGNATO
            else if (ticketTecnicoName == "")
            {
                BbAssegna.Text = "Prendi in carico";
                BbAssegna.Visible = true;
                BbSalva.Visible = false;
                BCambiaStato.Visible = false;
                DLivelllo.Enabled = false;
                DPriorita.Enabled = false;
                DStato.Enabled = false;

                LComunicazione.Visible = false;
                TComunicazione.Visible = false;
                BRisposta.Visible = false;

                BAnnulla.Visible = false;

            }
            //Altro TECNICO ASSEGNATO
            else
            {
                BbAssegna.Visible = false;

                BbSalva.Visible = false;
                BCambiaStato.Visible = false;
                DLivelllo.Enabled = false;
                DPriorita.Enabled = false;
                DStato.Enabled = false;

                LComunicazione.Visible = false;
                TComunicazione.Visible = false;
                BRisposta.Visible = false;

                BAnnulla.Visible = false;
            }


            if (!IsPostBack)
            {
                LoadDropDownList(DLivelllo, "Scegli il livello:");
                LoadDropDownList(DPriorita, "Scegli la priorita:");
                LoadDropDownList(DTecnici, "Scegli il Tecnico:");
                LoadDropDownList(DStato, "Scegli lo stato");
            }
        }
        //Assegna tecnico
        protected void ClickAssegna(object sender, EventArgs e)
        {
            string Tecnico;
            if (user.Ruolo == 3)
            {
                string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

                using (MySqlConnection con = new MySqlConnection(cs))
                {
                    con.Open();
                    string nuovaTicket = $"UPDATE ticket SET Tecnico=@tecnico,Stato=@stato WHERE ID=@id";

                    using (MySqlCommand cmd = new MySqlCommand(nuovaTicket, con))
                    {
                        cmd.Parameters.Add("@tecnico", MySqlDbType.VarChar).Value = DTecnici.SelectedValue;
                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = currentTicket.ID;
                        cmd.Parameters.Add("@stato", MySqlDbType.Int32).Value = 4;
                        cmd.ExecuteNonQuery();
                    }
                }
                Tecnico = DTecnici.SelectedItem.Text;
            }
            else
            {
                string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

                using (MySqlConnection con = new MySqlConnection(cs))
                {
                    con.Open();
                    string nuovaTicket = $"UPDATE ticket SET Tecnico=@tecnico,Stato=@stato WHERE ID=@id";
                    using (MySqlCommand cmd = new MySqlCommand(nuovaTicket, con))
                    {
                        cmd.Parameters.Add("@tecnico", MySqlDbType.VarChar).Value = user.ID;
                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = currentTicket.ID;
                        cmd.Parameters.Add("@stato", MySqlDbType.Int32).Value = 4;
                        cmd.ExecuteNonQuery();
                    }
                }
                Tecnico = user.Nome + " " + user.Cognome;
            }
            ShowAlert($"il Ticket e' assegnato a {Tecnico}");
        }
        //Salva livello e priorita
        protected void ClickSalva(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            livello = DLivelllo.SelectedValue;
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
                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = currentTicket.ID;
                        cmd.ExecuteNonQuery();
                    }
                }
                if (user.Ruolo == 2)
                {
                    using (MySqlConnection con = new MySqlConnection(cs))
                    {
                        con.Open();
                        string resetTecnico = $"UPDATE ticket SET Tecnico=@tecnico WHERE ID=@id";
                        using (MySqlCommand cmd = new MySqlCommand(resetTecnico, con))
                        {
                            cmd.Parameters.Add("@tecnico", MySqlDbType.Int32).Value = null;
                            cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = currentTicket.ID;
                            cmd.ExecuteNonQuery();
                        }
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
                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = currentTicket.ID;
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
                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = currentTicket.ID;
                        cmd.ExecuteNonQuery();
                    }
                }
                Response.Write("<script>alert('priorita&livello are updated')</script>");
            }
        }
        //Cambia stato ticket
        protected void CambiaStato(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string chiudiTicket = $"UPDATE ticket SET Stato=@stato WHERE ID=@id";
                using (MySqlCommand cmd = new MySqlCommand(chiudiTicket, con))
                {
                    cmd.Parameters.Add("@stato", MySqlDbType.VarChar).Value = DStato.SelectedValue;
                    cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = currentTicket.ID;
                    cmd.ExecuteNonQuery();
                }
            }
            string statoTesto = DStato.SelectedItem.Text;
            Response.Write($"<script>alert('Stato ticket aggiornato a: {statoTesto}')</script>");
        }
        // Set UI to read-only for closed tickets
        private void SetTicketReadOnlyUI()
        {
            BCambiaStato.Visible = false;
            BbSalva.Visible = false;
            BbAssegna.Visible = false;
            BRisposta.Visible = false;
            BChiudi.Visible = true;
            BAnnulla.Visible = false;
            DStato.Visible = false;

            DLivelllo.Enabled = false;
            DPriorita.Enabled = false;
            DTecnici.Enabled = false;

            TOggetto.ReadOnly = true;
            TMessaggio.ReadOnly = true;

            TComunicazione.Visible = false;
            LComunicazione.Visible = false;
        }
        // Load stati into dropdown from database
        private void LoadStatiFromDb()
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            using (var con = new MySqlConnection(cs))
            {
                con.Open();

                string sql;

                if (user.Ruolo == 1 || user.Ruolo == 4)
                {
                    sql = @"SELECT ID, Stato 
                    FROM stato
                    WHERE ID IN (2,3)
                    ORDER BY ID";
                }
                else
                {
                    sql = @"SELECT ID, Stato 
                    FROM stato
                    ORDER BY ID";
                }

                using (var cmd = new MySqlCommand(sql, con))
                using (var adp = new MySqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adp.Fill(dt);

                    DStato.DataSource = dt;
                    DStato.DataTextField = "Stato";
                    DStato.DataValueField = "ID";
                    DStato.DataBind();
                }
            }
        }
        // Check if a ticket is closed
        private bool IsTicketClosed(int ticketId)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            using (var con = new MySqlConnection(cs))
            {
                con.Open();
                using (var cmd = new MySqlCommand("SELECT Stato FROM ticket WHERE ID=@id", con))
                {
                    cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = ticketId;
                    object result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value) return false;

                    int statoId = Convert.ToInt32(result);
                    return statoId == 2;
                }
            }
        }
        private void ShowAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "created", $"alert({System.Web.HttpUtility.JavaScriptStringEncode(msg, true)});", true);
        }
        private void NotificaCrea()
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "created", "alert(\"Il Ticket e' creato\"); window.location='Dashboard.aspx';", true);
        }
    }
}
