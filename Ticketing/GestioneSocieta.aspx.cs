using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ticketing.Models;


namespace Ticketing
{
    public partial class GestioneSocieta : System.Web.UI.Page
    {

        private string nome;
        private string indirizzo;
        private string citta;
        private string cap;
        private string telefono;
        private string email;
        private string piva;
        private string note;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CR"] != null)
            {
                utente user = Session["CR"] as utente;
                if (user != null)
                {
                    string welcomeMessage = $"Benvenuto,{user.Nome} {user.Cognome}!";
                    BModifica.Visible = false;
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }

            BindRubricaSocieta();
            
        }

        protected void BindRubricaSocieta()
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                MySqlCommand command = new MySqlCommand("SELECT ID,Nome,Email,Telefono FROM societa", con);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                var table = new DataTable();
                adapter.Fill(table);
                
                rubricaSocieta.DataSource = table;
                rubricaSocieta.DataBind();
                

            }
        }

        protected void clickMod(object sender, EventArgs e)
        {
            
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            int id = Convert.ToInt32(rubricaSocieta.DataKeys[row.RowIndex].Value);
            CaricaDatiNeiCampi(id);
          

        }

        protected void clickElim(object sender, EventArgs e)
        {

            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            int id = Convert.ToInt32(rubricaSocieta.DataKeys[row.RowIndex].Value);
            EliminaRiga(id);


        }

        private void EliminaRiga(int id)
        {
            Response.Write("<script>alert('Stai eliminando dal database la societa')</script>");
        }

        private void CaricaDatiNeiCampi(int id)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            string query = @"SELECT Nome, Indirizzo, Citta, Cap, Telefono, Email, PIva, Note
                     FROM societa
                     WHERE ID = @ID";

            using (MySqlConnection conn = new MySqlConnection(cs))
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.Add("@ID", MySqlDbType.Int32).Value = id;

                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        TNome.Text = reader["Nome"]?.ToString() ?? "";
                        TIndirizzo.Text = reader["Indirizzo"]?.ToString() ?? "";
                        TCitta.Text = reader["Citta"]?.ToString() ?? "";
                        TCap.Text = reader["Cap"]?.ToString() ?? "";
                        TTelefono.Text = reader["Telefono"]?.ToString() ?? "";
                        TeMail.Text = reader["Email"]?.ToString() ?? "";
                        TPIva.Text = reader["PIva"]?.ToString() ?? "";
                        TNote.Text = reader["Note"]?.ToString() ?? "";

                        ViewState["IdSocietaSelezionata"] = id;

                        BModifica.Visible = true;
                    }
                }
            }
        }

        private void SvuotaCampi()
        {
            TNome.Text = "";
            TIndirizzo.Text = "";
            TCitta.Text = "";
            TCap.Text = "";
            TTelefono.Text = "";
            TeMail.Text = "";
            TPIva.Text = "";
            TNote.Text = "";

            ViewState["IdSocietaSelezionata"] = null;
            
        }

        public void clickCrea(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            nome = TNome.Text;
            indirizzo = TIndirizzo.Text;
            citta = TCitta.Text;
            cap = TCap.Text.Trim();
            telefono = TTelefono.Text.Trim();
            email = TeMail.Text.Trim();
            piva = TPIva.Text.Trim();
            note = TNote.Text;


            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string newSocieta = "INSERT INTO societa (Nome, Indirizzo, Citta, Cap, Telefono, Email,PIva,Note) VALUES (@nome, @indirizzo, @citta, @cap, @telefono, @email,@piva,@note)";

                MySqlCommand cmd = new MySqlCommand(newSocieta, con);

                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = nome; //More secure, advised (To be researched)

                cmd.Parameters.Add("@indirizzo", MySqlDbType.VarChar).Value = indirizzo;
                cmd.Parameters.Add("@citta", MySqlDbType.VarChar).Value = citta;
                cmd.Parameters.Add("@cap", MySqlDbType.VarChar).Value = cap;
                cmd.Parameters.Add("@telefono", MySqlDbType.VarChar).Value = telefono;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                cmd.Parameters.Add("@piva", MySqlDbType.VarChar).Value = piva;
                cmd.Parameters.Add("@note", MySqlDbType.VarChar).Value = note;

                cmd.ExecuteNonQuery();

                SvuotaCampi();
                BindRubricaSocieta();


            }

        }

        public void clickModifica(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;


            nome = TNome.Text;
            indirizzo = TIndirizzo.Text;
            citta = TCitta.Text;
            cap = TCap.Text.Trim();
            telefono = TTelefono.Text.Trim();
            email = TeMail.Text.Trim();
            piva = TPIva.Text.Trim();
            note = TNote.Text;


            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string modificaSocieta =
                "UPDATE societa SET Nome= @nome, Indirizzo= @indirizzo, Citta= @citta, Cap= @cap, Telefono= @telefono, Email= @email, PIva=@piva WHERE PIva = @piva"; //change textbox to piva
               
                MySqlCommand cmd = new MySqlCommand(modificaSocieta, con);

                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = nome;
                cmd.Parameters.Add("@indirizzo", MySqlDbType.VarChar).Value = indirizzo;
                cmd.Parameters.Add("@citta", MySqlDbType.VarChar).Value = citta;
                cmd.Parameters.Add("@cap", MySqlDbType.VarChar).Value = cap;
                cmd.Parameters.Add("@telefono", MySqlDbType.VarChar).Value = telefono;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                cmd.Parameters.Add("@piva", MySqlDbType.VarChar).Value = piva;
                cmd.Parameters.Add("@note", MySqlDbType.VarChar).Value = note;

                int righeAggiornate = cmd.ExecuteNonQuery();

                if (righeAggiornate > 0)
                {
                    Response.Write("<script>alert('Modifica avvenuta con successo')</script>");
                    
                } else
                {
                    Response.Write("<script>alert('Nessuna Modifica Effettuata')</script>");
                }

            }

            SvuotaCampi();
            BindRubricaSocieta();

        }
        public void clickElimina(object sender, EventArgs e)
        {
            SvuotaCampi();
        }
    }
}