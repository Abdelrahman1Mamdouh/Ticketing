using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Management;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace Ticketing
{
    public partial class GestioneSocieta : System.Web.UI.Page
    {
        private int id;
        private String nome;
        private String indirizzo;
        private String citta;
        private String cap;
        private String telefono;
        private String email;
        private String piva;
        private String comunicazione;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void clickCrea(object sender, EventArgs e)
        {
            String cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            nome = TNome.Text;
            indirizzo = TIndirizzo.Text;
            citta = TCitta.Text;
            cap = TCap.Text;
            telefono = TTelefono.Text;
            email = TeMail.Text;
            piva = TPIva.Text;
            comunicazione = TComunicazione.Text;

            using (MySqlComand con=new MySqlComand(cs))
            {
                con.Open();
                String newSocieta = "INSERT INTO societa (Nome, Indirizzo, Citta, Cap, Telefono, Email) VALUES (@nome, @indirizzo, @citta, @cap, @telefono, @email)";
                
                MySqlComand cmd = new MySqlComand(newSocieta, con);

                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@indirizzo", indirizzo);
                cmd.Parameters.AddWithValue("@citta", citta);
                cmd.Parameters.AddWithValue("@cap", cap);
                cmd.Parameters.AddWithValue("@livelloo", livello);
                cmd.Parameters.AddWithValue("@telefono", telefono);
                cmd.Parameters.AddWithValue("@email", email);
            }
             
        }
        
        public void clickModifica(object sender, EventArgs e)
        {
            String cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            ID=TId.Text;
            nome = TNome.Text;
            indirizzo = TIndirizzo.Text;
            citta = TCitta.Text;
            cap = TCap.Text;
            telefono = TTelefono.Text;
            email = TeMail.Text;
            piva = TPIva.Text;
            comunicazione = TComunicazione.Text;

            using (MySqlComand con = new MySqlComand(cs))
            {
                con.Open();
                public String ModificaSocieta =
                "UPDATE societa SET Nome= @nome, Indirizzo= @indirizzo, Citta= @citta, Cap= @cap, Telefono= @telefono, Email= @email WHERE (ID => @ID)";
                MySqlComand cmd = new MySqlComand(ModificaSocieta, con);

                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@indirizzo", indirizzo);
                cmd.Parameters.AddWithValue("@citta", citta);
                cmd.Parameters.AddWithValue("@cap", cap);
                cmd.Parameters.AddWithValue("@livelloo", livello);
                cmd.Parameters.AddWithValue("@telefono", telefono);
                cmd.Parameters.AddWithValue("@email", email);
            }
        
        public void clickElimina(object sender, EventArgs e)
        {
             ID = TId.Text;
             using (MySqlComand con = new MySqlComand(cs))
             {
                con.Open();
                public String deleteSocieta = "DELETE FROM `societa` [WHERE id== @ID]"
                MySqlComand cmd = new MySqlComand(deleteSocieta, con);

                cmd.Parameters.AddWithValue("@ID", ID);
             }
        }
       
    }