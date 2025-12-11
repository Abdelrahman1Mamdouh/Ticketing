using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace Ticketing
{
    public partial class GestioneTicket : System.Web.UI.Page

        private int id;
        private int cliente;
        private int tecnico;
        private int livello;
        private int stato;
        private int prodotto
        private int categoria;
        private int priorita;
        private String oggetto;
        private String messaggio;
        private String comunicazione;
        
        
       
         protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void clickCrea(object sender, EventArgs e)
    {
        String cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            cliente = TCliente.Text;
            tecnico = TTecnico.Text;
            livello = TLivello.Text;
            stato = DStato.Text;
            prodotto = DProdotto.Text;
            categoria = DCategoria.Text;
            priorita = DPriorita.Text;
            oggetto = TOggetto.Text;
            messaggio = TMessaggio.Text;
            comunicazione = TComunicazione.Text;

            using (MySqlComand con = new MySqlComand(cs))
            {
                con.Open();
                String newSocieta = "INSERT INTO societa (Cliente, Tecnico, Livello, Stato, Prodotto, Categoria, Priorita, Oggetto, Messaggio, Comunicazione) VALUES (@cliente, @tecnico, @livello, @stato, @prodotto, @categoria, @priorita, @oggetto, @messaggio, @comunicazione)";

                MySqlComand cmd = new MySqlComand(newSocieta, con);

                cmd.Parameters.AddWithValue("@cliente", cliente);
                cmd.Parameters.AddWithValue("@tecnico", tecnico);
                cmd.Parameters.AddWithValue("@livello", livello);
                cmd.Parameters.AddWithValue("@stato", stato);
                cmd.Parameters.AddWithValue("@prodotto", prodotto);
                cmd.Parameters.AddWithValue("@categoria", categoria);
                cmd.Parameters.AddWithValue("@priorita", priorita);
                cmd.Parameters.AddWithValue("@oggetto", oggetto);
                cmd.Parameters.AddWithValue("@messaggio", messaggio);
                cmd.Parameters.AddWithValue("@comunicazione", comunicazione);
        }
        }
        
        public void clickModifica(object sender, EventArgs e)
        {
            String cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            ID = TId.Text;
            cliente = TCliente.Text;
            tecnico = TTecnico.Text;
            livello = TLivello.Text;
            stato = DStato.Text;
            prodotto = DProdotto.Text;
            categoria = DCategoria.Text;
            priorita = DPriorita.Text;
            oggetto = TOggetto.Text;
            messaggio = TMessaggio.Text;
            comunicazione = TComunicazione.Text;

        using (MySqlComand con = new MySqlComand(cs))

                {
                con.Open();
                public String ModificaSocieta =
                "UPDATE societa SET Cliente= @cliente, Tecnico= @tecnico, Livello= @livello, Stato= @stato, Prodotto= @prodotto, Categoria= @categoria, Priorita= @priorita, Oggetto= @oggetto, Messaggio= @messaggio, Comunicazione= @comunicazione WHERE (ID => @ID)";
                
                MySqlComand cmd = new MySqlComand(ModificaSocieta, con);

                cmd.Parameters.AddWithValue("@cliente", cliente);
                cmd.Parameters.AddWithValue("@tecnico", tecnico);
                cmd.Parameters.AddWithValue("@livello", livello);
                cmd.Parameters.AddWithValue("@stato", stato);
                cmd.Parameters.AddWithValue("@prodotto", prodotto);
                cmd.Parameters.AddWithValue("@categoria", categoria);
                cmd.Parameters.AddWithValue("@priorita", priorita);
                cmd.Parameters.AddWithValue("@oggetto", oggetto);
                cmd.Parameters.AddWithValue("@messaggio", messaggio);
                cmd.Parameters.AddWithValue("@comunicazione", comunicazione);

                }
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
}