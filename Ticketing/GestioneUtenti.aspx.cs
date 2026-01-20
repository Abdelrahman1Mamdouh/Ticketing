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
        const int Client = 1;
        const int Tec = 2;
        const int Tec_Admin = 3;
        const int Client_Admin = 4;

        protected void Page_Load(object sender, EventArgs e)
        {
            utente user = Session["CR"] as utente;
            if (user == null || user.Ruolo == Client)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            bool isTecAdmin = (user.Ruolo == Tec_Admin);
            PnlRuolo.Visible = true;
            PnlLivello.Visible = isTecAdmin;
            PnlDipartimento.Visible = isTecAdmin;

            if (!IsPostBack)
            {
                LoadDropDownList("SELECT ID, Ruolo FROM ruolo ORDER BY ID", DRuolo, "Ruolo", "Seleziona Ruolo");
                LoadDropDownList("SELECT ID, Nome FROM societa ORDER BY ID", DSocieta, "Nome", "Seleziona Società");
                LoadDropDownList("SELECT ID, Livello FROM livello ORDER BY ID", DLivello, "Livello", "Seleziona Livello");
                LoadDropDownList("SELECT ID, Dipartimento FROM dipartimento ORDER BY ID", DDipartimento, "Dipartimento", "Seleziona Dipartimento");

                if (user.Ruolo == Client_Admin || user.Ruolo == Tec)
                {
                    ListItem tecItem = DRuolo.Items.FindByValue(Tec.ToString());
                    ListItem adminTecItem = DRuolo.Items.FindByValue(Tec_Admin.ToString());
                    if (tecItem != null) DRuolo.Items.Remove(tecItem);
                    if (adminTecItem != null) DRuolo.Items.Remove(adminTecItem);
                }

                if (user.Ruolo == Client_Admin)
                {
                    DSocieta.SelectedValue = user.Societa.ToString();
                    DSocieta.Enabled = false;
                }

                BindRubricaUtenti();
            }
        }

        protected void HandleButtonClick(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            int mode = ViewState["ButtonMode"] != null ? (int)ViewState["ButtonMode"] : 1;

            if (mode == 1)
                this.ActionCrea();
            else
                this.ActionModifica();

            BindRubricaUtenti();
            ResetCampi();

            ViewState["ButtonMode"] = 1;
            BCrea.Text = "Crea";
            ViewState["SelectedUserID"] = null;
        }

        public void ActionCrea()
        {
            utente creator = Session["CR"] as utente;
            int.TryParse(DRuolo.SelectedValue, out int targetRuolo);

            if (creator.Ruolo != Tec_Admin && (targetRuolo == Tec || targetRuolo == Tec_Admin))
            {
                Response.Write("<script>alert('Non hai i permessi per creare questo ruolo.');</script>");
                return;
            }

            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string sql = @"INSERT INTO utente (Nome, Cognome, Ruolo, Societa, Livello, Dipartimento, Telefono, Email, Pass)
                               SELECT @nome, @cognome, @ruolo, @societa, @livello, @dipartimento, @telefono, @email, @password
                               FROM DUAL
                               WHERE NOT EXISTS (SELECT 1 FROM utente WHERE Email = @email)";

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    SetParameters(cmd);
                    if (cmd.ExecuteNonQuery() > 0)
                        Response.Write("<script>alert('Utente creato con successo!')</script>");
                    else
                        Response.Write("<script>alert('Errore: Email già presente.')</script>");
                }
            }
        }

        private void ActionModifica()
        {
            if (ViewState["SelectedUserID"] == null) return;
            int userId = (int)ViewState["SelectedUserID"];

            utente creator = Session["CR"] as utente;
            int.TryParse(DRuolo.SelectedValue, out int targetRuolo);

            if (creator.Ruolo != Tec_Admin && (targetRuolo == Tec || targetRuolo == Tec_Admin))
            {
                Response.Write("<script>alert('Non hai i permessi per assegnare questo ruolo.');</script>");
                return;
            }

            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string sql = @"UPDATE utente SET Nome=@nome, Cognome=@cognome, Ruolo=@ruolo, 
                               Societa=@societa, Livello=@livello, Dipartimento=@dipartimento, 
                               Telefono=@telefono, Email=@email, Pass=@password WHERE ID=@id";

                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    SetParameters(cmd);
                    cmd.ExecuteNonQuery();
                    Response.Write("<script>alert('Dati aggiornati correttamente!')</script>");
                }
            }
        }

        public void clickModifica(object sender, EventArgs e)
        {
            BCancel.Visible = true;
            utente creator = Session["CR"] as utente;
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            int userId = Convert.ToInt32(rubricaUtenti.DataKeys[row.RowIndex].Value);

            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string sql = "SELECT * FROM utente WHERE ID = @id";
                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            int targetRuolo = Convert.ToInt32(rdr["Ruolo"]);

                            if (creator.Ruolo != Tec_Admin && (targetRuolo == Tec || targetRuolo == Tec_Admin))
                            {
                                Response.Write("<script>alert('Non hai i permessi per modificare questo utente.');</script>");
                                return;
                            }

                            ViewState["ButtonMode"] = 2;
                            ViewState["SelectedUserID"] = userId;
                            BCrea.Text = "Salva Modifiche";

                            TNome.Text = rdr["Nome"].ToString();
                            TCognome.Text = rdr["Cognome"].ToString();
                            TEmail.Text = rdr["Email"].ToString();
                            TTelefono.Text = rdr["Telefono"].ToString();
                            TPassword.Text = rdr["Pass"].ToString();

                            DRuolo.SelectedValue = rdr["Ruolo"].ToString();
                            DRuolo_SelectedIndexChanged(null, null);

                            if (DSocieta.Enabled) TrySetSelectedValue(DSocieta, rdr["Societa"].ToString());
                            if (DLivello.Enabled) TrySetSelectedValue(DLivello, rdr["Livello"].ToString());
                            if (DDipartimento.Enabled) TrySetSelectedValue(DDipartimento, rdr["Dipartimento"].ToString());
                        }
                    }
                }
            }
        }

        public void clickElimina(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            int userId = Convert.ToInt32(rubricaUtenti.DataKeys[row.RowIndex].Value);

            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string sql = "DELETE FROM utente WHERE ID = @id";
                using (MySqlCommand cmd = new MySqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    cmd.ExecuteNonQuery();
                }
            }
            BindRubricaUtenti();
            Response.Write("<script>alert('Utente eliminato.')</script>");
        }

        private void SetParameters(MySqlCommand cmd)
        {
            utente currentUser = Session["CR"] as utente;

            cmd.Parameters.AddWithValue("@nome", TNome.Text.Trim());
            cmd.Parameters.AddWithValue("@cognome", TCognome.Text.Trim());
            cmd.Parameters.AddWithValue("@email", TEmail.Text.Trim());
            cmd.Parameters.AddWithValue("@telefono", TTelefono.Text.Trim());
            cmd.Parameters.AddWithValue("@password", TPassword.Text.Trim());
            cmd.Parameters.AddWithValue("@ruolo", DRuolo.SelectedValue);

            if (currentUser.Ruolo == Client_Admin && (DRuolo.SelectedValue == Client.ToString() || DRuolo.SelectedValue == Client_Admin.ToString()))
            {
                cmd.Parameters.AddWithValue("@societa", currentUser.Societa);
            }
            else if (DSocieta.Enabled && !string.IsNullOrEmpty(DSocieta.SelectedValue))
            {
                cmd.Parameters.AddWithValue("@societa", DSocieta.SelectedValue);
            }
            else
            {
                cmd.Parameters.AddWithValue("@societa", DBNull.Value);
            }

            cmd.Parameters.AddWithValue("@livello", (DLivello.Enabled && !string.IsNullOrEmpty(DLivello.SelectedValue)) ? (object)DLivello.SelectedValue : DBNull.Value);
            cmd.Parameters.AddWithValue("@dipartimento", (DDipartimento.Enabled && !string.IsNullOrEmpty(DDipartimento.SelectedValue)) ? (object)DDipartimento.SelectedValue : DBNull.Value);
        }

        private void TrySetSelectedValue(DropDownList ddl, string val)
        {
            if (!string.IsNullOrEmpty(val) && ddl.Items.FindByValue(val) != null) ddl.SelectedValue = val;
        }

        private void LoadDropDownList(string query, DropDownList DDL, string dataTextField, string label)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
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
                    DDL.Items.Insert(0, new ListItem(label, "0"));
                }
            }
        }

        protected void BindRubricaUtenti()
        {
            utente currentUser = Session["CR"] as utente;
            if (currentUser == null) return;

            string query = "";

            if (currentUser.Ruolo == Tec)
            {
                query = "SELECT * FROM utenti WHERE Ruolo IN ('Cliente', 'client_admin')";
            }
            else if (currentUser.Ruolo == Tec_Admin)
            {
                query = "SELECT * FROM utenti";
            }
            else if (currentUser.Ruolo == Client_Admin)
            {
                
                query = @"SELECT * FROM utenti 
                  WHERE Societa = (SELECT Nome FROM societa WHERE ID = @mySocID) 
                  AND Ruolo IN ('Cliente', 'client_admin')";
            }

            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    
                    if (currentUser.Ruolo == Client_Admin)
                    {
                        cmd.Parameters.AddWithValue("@mySocID", currentUser.Societa);
                    }

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    rubricaUtenti.DataSource = table;
                    if (currentUser.Ruolo != Tec_Admin)
                    {
                        
                        rubricaUtenti.Columns[5].Visible = false; // Livello
                        rubricaUtenti.Columns[6].Visible = false; // Dipartimento
                    }
                    else
                    {
                       
                        rubricaUtenti.Columns[5].Visible = true;
                        rubricaUtenti.Columns[6].Visible = true;
                    }
                    rubricaUtenti.DataBind();
                }
            }
        }

        protected void ResetCampi()
        {
            utente user = Session["CR"] as utente;
            TNome.Text = TCognome.Text = TPassword.Text = TTelefono.Text = TEmail.Text = "";
            DRuolo.SelectedIndex = DSocieta.SelectedIndex = DLivello.SelectedIndex = DDipartimento.SelectedIndex = 0;

            if (user.Ruolo == Client_Admin) {DSocieta.SelectedValue = user.Societa.ToString(); }
            if (user.Ruolo == Tec_Admin) { DSocieta.Enabled = true; DRuolo.Enabled=true; DLivello.Enabled=true; DDipartimento.Enabled = true; }
        }

        protected void DRuolo_SelectedIndexChanged(object sender, EventArgs e)
        {
            utente user = Session["CR"] as utente;
            int.TryParse(DRuolo.SelectedValue, out int selectedRuolo);
            bool isClient = (selectedRuolo == Client || selectedRuolo == Client_Admin);

            DSocieta.Enabled = isClient && (user.Ruolo != Client_Admin);
            DLivello.Enabled = !isClient;
            DDipartimento.Enabled = !isClient;

            if (!isClient) DSocieta.SelectedIndex = 0;
            else { DLivello.SelectedIndex = 0; DDipartimento.SelectedIndex = 0; }
        }
        protected void clickAnnulla(object sender, EventArgs e)
        {
            
            ViewState["ButtonMode"] = 1;
            ViewState["SelectedUserID"] = null;

            
            BCrea.Text = "Crea";
            BCancel.Visible = false;

            
            ResetCampi();
            

        }
    }
}