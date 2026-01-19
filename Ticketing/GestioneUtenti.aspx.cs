using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using Ticketing.Models;


namespace Ticketing
{
    public partial class GestioneUtenti : System.Web.UI.Page
    {
        private String nome;
        private String cognome;
        private String ruolo;
        private String societa;
        private String livello;
        private String dipartimento;
        private String telefono;
        private String email;
        private String pass;

        const int Client = 1;
        const int Tec = 2;
        const int Tec_Admin = 3;
        const int Client_Admin = 4;


        private void LoadDropDownList(string query, DropDownList DDL, string dataTextField, string label)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;


            try
            {
                using (var con = new MySqlConnection(cs))
                {
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        con.Open();


                        using (var da = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);



                            DDL.DataTextField = dataTextField;
                            DDL.DataValueField = "ID";
                            DDL.DataSource = dt;
                            DDL.DataBind();
                        }


                        DDL.Items.Insert(0, new ListItem(label, ""));
                        DDL.Items[0].Attributes["disabled"] = "disabled";
                        DDL.Items[0].Attributes["selected"] = "selected";
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading {label}: {ex.Message}");
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            utente user = null;
            //user's data fetch from session
            if (Session["CR"] != null)
            {
                user = Session["CR"] as utente;
                if (user != null)
                {
                    string welcomeMessage = $"Benvenuto,{user.Nome} {user.Cognome}!";
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }

            //role's visibility permissions
            BindRubricaUtenti();

            bool isTecAdmin = (user.Ruolo == Tec_Admin);
            bool isTec = (user.Ruolo == Tec);
            bool isClientAdmin = (user.Ruolo == Client_Admin);

            PnlRuolo.Visible = isTecAdmin || isClientAdmin;
            PnlLivello.Visible = isTecAdmin;
            PnlDipartimento.Visible = isTecAdmin;

            if (!IsPostBack)
            {

                LoadDropDownList("SELECT ID, Ruolo FROM ruolo ORDER BY ID", DRuolo, "Ruolo", "Seleziona Ruolo");
                LoadDropDownList("SELECT ID, Nome FROM societa ORDER BY ID", DSocieta, "Nome", "Seleziona Società");
                LoadDropDownList("SELECT ID, Livello FROM livello ORDER BY ID", DLivello, "Livello", "Seleziona Livello");
                LoadDropDownList("SELECT ID, Dipartimento FROM dipartimento ORDER BY ID", DDipartimento, "Dipartimento", "Seleziona Dipartimento");

                BCrea.Visible = isTecAdmin || isClientAdmin || isTec;
                BModifica.Visible = isTecAdmin || isClientAdmin || isTec;
                BElimina.Visible = isTecAdmin || isTec || isClientAdmin;


                DRuolo.Enabled = isTecAdmin || isClientAdmin;
                DDipartimento.Enabled = isTecAdmin;
                DLivello.Enabled = isTecAdmin;

                bool isUserAdmin = isTecAdmin || isClientAdmin || isTec;
                TNome.ReadOnly = !isUserAdmin;
                TCognome.ReadOnly = !isUserAdmin;
                TTelefono.ReadOnly = !isUserAdmin;
                TEmail.ReadOnly = !isUserAdmin;
                TPassword.ReadOnly = !isUserAdmin;


                if (isClientAdmin)
                {

                    DSocieta.SelectedValue = user.Societa.ToString();
                    DSocieta.Enabled = false;


                }
                else
                {
                    DSocieta.Enabled = true;
                }

            }
        }

        protected void BindRubricaUtenti()
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                MySqlCommand command = new MySqlCommand("SELECT * FROM dashboard", con);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                var table = new DataTable();
                adapter.Fill(table);

                rubricaUtenti.DataSource = table;
                rubricaUtenti.DataBind();

            }
        }
        ///DOMAAANIIIII
        public void clickCrea(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            utente user = Session["CR"] as utente;


            string nome = TNome.Text;
            string cognome = TCognome.Text;
            string pass = TPassword.Text;
            string telefono = TTelefono.Text;
            string email = TEmail.Text;

            string ruoloStr = DRuolo.SelectedValue;
            string societaStr = DSocieta.SelectedValue;
            string livelloStr = DLivello.SelectedValue;
            string dipartimentoStr = DDipartimento.SelectedValue;


            int TargetRuolo = 0;
            int TargetSocieta = 0;
            int TargetLivello = 0;
            int TargetDipartimento = 0;


            if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(cognome) || string.IsNullOrEmpty(pass))
            {
                Response.Write("<script>alert('Errore: Campi Nome, Cognome, e Password sono obbligatori.')</script>");
                return;
            }

            bool success = false;
            string insertQuery = "";

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();


                if (user.Ruolo == Tec_Admin)
                {

                    if (!int.TryParse(ruoloStr, out TargetRuolo) ||
                        !int.TryParse(societaStr, out TargetSocieta) ||
                        !int.TryParse(livelloStr, out TargetLivello) ||
                        !int.TryParse(dipartimentoStr, out TargetDipartimento))
                    {
                        Response.Write("<script>alert('Errore: Tech Admin deve selezionare tutti i campi di ruolo e società.')</script>");
                        return;
                    }


                    insertQuery = "INSERT INTO utente (Nome, Cognome, Ruolo, Societa, Livello, Dipartimento, Telefono, Email, Pass) " +
                                  "VALUES (@nome, @cognome, @ruolo, @societa, @livello, @dipartimento , @telefono, @email, @password)";

                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.Add("@ruolo", MySqlDbType.Int32).Value = TargetRuolo;
                        cmd.Parameters.Add("@societa", MySqlDbType.Int32).Value = TargetSocieta;
                        cmd.Parameters.Add("@livello", MySqlDbType.Int32).Value = TargetLivello;
                        cmd.Parameters.Add("@dipartimento", MySqlDbType.Int32).Value = TargetDipartimento;

                        AddBaseParameters(cmd, nome, cognome, telefono, email, pass);
                        success = cmd.ExecuteNonQuery() > 0;
                    }
                }


                else if (user.Ruolo == Client_Admin)
                {

                    TargetSocieta = user.Societa;


                    if (!int.TryParse(ruoloStr, out TargetRuolo) || (TargetRuolo != Client && TargetRuolo != Client_Admin))
                    {
                        Response.Write("<script>alert('Errore: Client Admin può creare solo Client o Admin Client.')</script>");
                        return;
                    }


                    insertQuery = "INSERT INTO utente (Nome, Cognome, Ruolo, Societa, Telefono, Email, Pass) VALUES (@nome, @cognome, @ruolo, @societa, @telefono, @email, @password)";

                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.Add("@ruolo", MySqlDbType.Int32).Value = TargetRuolo;
                        cmd.Parameters.Add("@societa", MySqlDbType.Int32).Value = TargetSocieta;

                        AddBaseParameters(cmd, nome, cognome, telefono, email, pass);
                        success = cmd.ExecuteNonQuery() > 0;
                    }
                }


                else if (user.Ruolo == Tec)
                {

                    if (!int.TryParse(societaStr, out TargetSocieta) || !int.TryParse(ruoloStr, out TargetRuolo) || (TargetRuolo != Client && TargetRuolo != Client_Admin))
                    {
                        Response.Write("<script>alert('Errore: Tecnico deve selezionare Società e può creare solo Client o Admin Client.')</script>");
                        return;
                    }


                    insertQuery = "INSERT INTO utente (Nome, Cognome, Ruolo, Societa, Telefono, Email, Pass) " +
                                  "VALUES (@nome, @cognome, @ruolo, @societa, @telefono, @email, @password)";

                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.Add("@ruolo", MySqlDbType.Int32).Value = TargetRuolo;
                        cmd.Parameters.Add("@societa", MySqlDbType.Int32).Value = TargetSocieta;

                        AddBaseParameters(cmd, nome, cognome, telefono, email, pass);
                        success = cmd.ExecuteNonQuery() > 0;
                    }
                }


                else
                {
                    Response.Write("<script>alert('Errore di autorizzazione: Non sei autorizzato a creare utenti.')</script>");
                    return;
                }


                if (success)
                {
                    Response.Write("<script>alert('Utente creato con successo!')</script>");
                }
                else if (!string.IsNullOrEmpty(insertQuery))
                {
                    Response.Write("<script>alert('Errore sconosciuto durante la creazione dell\\'utente. Controlla i log.')</script>");
                }
            }
        }

        private void AddBaseParameters(MySqlCommand cmd, string nome, string cognome, string telefono, string email, string pass)
        {
            cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = nome;
            cmd.Parameters.Add("@cognome", MySqlDbType.VarChar).Value = cognome;
            cmd.Parameters.Add("@telefono", MySqlDbType.VarChar).Value = telefono;
            cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
            cmd.Parameters.Add("@password", MySqlDbType.VarChar).Value = pass;
        }

        public void clickModifica(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            utente user = Session["CR"] as utente;
            string TargetEmail = TEmail.Text.Trim();



            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string ModificaUtenti =
                $"UPDATE utenti SET Nome= @nome, Cognome= @cognome, Ruolo= @ruolo, Societa= @societa, Livello= @livello, Dipartimento= @dipartimento, Telefono= @telefono, Email= @email WHERE Email= @email";
                MySqlCommand cmd = new MySqlCommand(ModificaUtenti, con);

                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = TNome.Text.Trim();
                cmd.Parameters.Add("@cognome", MySqlDbType.VarChar).Value = TCognome.Text.Trim();
                cmd.Parameters.Add("@ruolo", MySqlDbType.Int32).Value = int.Parse(DRuolo.SelectedValue);
                cmd.Parameters.Add("@societa", MySqlDbType.Int32).Value = int.Parse(DSocieta.SelectedValue);
                cmd.Parameters.Add("@livello", MySqlDbType.Int32).Value = int.Parse(DLivello.SelectedValue);
                cmd.Parameters.Add("@dipartimento", MySqlDbType.Int32).Value = int.Parse(DDipartimento.SelectedValue);
                cmd.Parameters.Add("@telefono", MySqlDbType.VarChar).Value = TTelefono.Text.Trim();
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = TEmail.Text.Trim();
                cmd.ExecuteNonQuery();
            }
        }

        public void clickElimina(object sender, EventArgs e)
        {
            String cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string deleteUtenti = "DELETE FROM `utenti` WHERE Email= @email";
                MySqlCommand cmd = new MySqlCommand(deleteUtenti, con);
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                cmd.ExecuteNonQuery();
            }
        }


    }
}