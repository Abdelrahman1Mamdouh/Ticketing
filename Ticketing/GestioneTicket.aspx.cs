using MySql.Data.MySqlClient;
using System;
using System.Configuration;

namespace Ticketing
{
    public partial class GestioneTicket : System.Web.UI.Page
    {
        
        private string prodotto;
        private string categoria;
        private string oggetto;
        private string messaggio;
        

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void clickCrea(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            
            prodotto = DProdotto.Text;
            categoria = DCategoria.Text;
            oggetto = TOggetto.Text;
            messaggio = TMessaggio.Text;
            

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string newSocieta = "INSERT INTO societa " +
                    "(Prodotto, Categoria, Oggetto, Messaggio) " +
                    "VALUES (@prodotto, @categoria, @oggetto, @messaggio)";

                MySqlCommand cmd = new MySqlCommand(newSocieta, con);

                
                cmd.Parameters.Add("@prodotto", MySqlDbType.VarChar).Value = prodotto;
                cmd.Parameters.Add("@categoria", MySqlDbType.VarChar).Value = categoria;
                cmd.Parameters.Add("@oggetto", MySqlDbType.VarChar).Value = oggetto;
                cmd.Parameters.Add("@messaggio", MySqlDbType.VarChar).Value = messaggio;
                
                cmd.ExecuteNonQuery();
            }
        }


        
        
    } 
}