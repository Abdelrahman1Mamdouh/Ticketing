using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace Ticketing
{
    public partial class GestioneUtenti : System.Web.UI.Page
    {
        private int id;
        private String nome;
        private String cognome;
        private int ruolo;
        private int societa;
        private int livello;
        private int dipartimento;
        private String telefono;
        private String email;
        private String comunicaqzione;
        private String pass;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void clickCrea(object sender, EventArgs e)
        {

            String cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            nome = TNome.Text;
            cognome = TCognome.Text;
            ruolo = DRuolo.Text;
            societa = TSocieta.Text;
            livello = DLivello.Text;
            dipartimento = DDipartimento.Text;
            pass = TPassword.Text;
            telefono = TTelefono.Text;
            email = TeMail.Text;
            comunicazione = TComunicazione;

            using (MySqlComand con = new MySqlComand(cs))


            {
                con.Open();
                String newUtenti = "INSERT INTO societa (Nome, Cognome, Ruolo, Societa, Livello, Dipartimento, Password, Telefono, Email, Comunicazione) VALUES (@nome, @cognome, @ruolo, @societa, @livello, @dipartimento, @password, @telefono, @email, @comunicazione)";

                MySqlComand cmd = new MySqlComand(newUtenti, con);

                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@cognome", cognome);
                cmd.Parameters.AddWithValue("@ruolo", ruolo);
                cmd.Parameters.AddWithValue("@societa", societa);
                cmd.Parameters.AddWithValue("@livelloo", livello);
                cmd.Parameters.AddWithValue("@dipartimento", dipartimento);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@telefono", telefono);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@comunicazione", comunicazione);
            }

        }

        public void clickModifica(object sender, EventArgs e)
        {
            String cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            ID = TId.Text;
            nome = TNome.Text;
            cognome = TCognome.Text;
            ruolo = DRuolo.Text;
            societa = TSocieta.Text;
            livello = DLivello.Text;
            dipartimento = DDipartimento.Text;
            pass = TPassword.Text;
            telefono = TTelefono.Text;
            email = TeMail.Text;
            comunicazione = TComunicazione;

            using (MySqlComand con = new MySqlComand(cs))
            {
                con.Open();
                public String ModificaUtenti =
                $"UPDATE utenti SET Nome= @nome, Cognome= @cognome, Ruolo= @ruolo, Societa= @societa, Livello= @livello, Dipartimento= @dipartimento, Telefono= @telefono, Email= @email, Comunicazione= @comunicazione WHERE (ID=> @ID)";
                MySqlComand cmd = new MySqlComand(ModificaUtenti, con);

                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@cognome", cognome);
                cmd.Parameters.AddWithValue("@ruolo", ruolo);
                cmd.Parameters.AddWithValue("@societa", societa);
                cmd.Parameters.AddWithValue("@livelloo", livello);
                cmd.Parameters.AddWithValue("@dipartimento", dipartimento);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@telefono", telefono);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@comunicazione", comunicazione);
    }
            public void clickElimina(object sender, EventArgs e)
    {
            ID = TId.Text;
            using (MySqlComand con = new MySqlComand(cs))
            {
                con.Open();
                public String deleteUtenti = "DELETE FROM `utenti` [WHERE id== @ID]"
                MySqlComand cmd = new MySqlComand(deleteUtenti, con);

                cmd.Parameters.AddWithValue("@ID", ID);
            }
    }
}