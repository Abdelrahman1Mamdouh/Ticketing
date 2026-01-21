using MySqlConnector;
using System;
using System.Configuration;
using System.Data;

/// <summary>
/// Summary description for Class1
/// </summary>

namespace Ticketing
{
        public class filtri
    {
        public static DataTable Filtro(string tabella)
	    {
            var table = new DataTable();
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                MySqlCommand command = new MySqlCommand($"SELECT * FROM {tabella} ORDER BY ID DESC", con);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            
                adapter.Fill(table);
            }

            return table;
        }

        public static DataTable Filtro(string stringa, string tabella)
        {
            var table = new DataTable();

            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                MySqlCommand command = new MySqlCommand($"SELECT * FROM {tabella} where {stringa} ORDER BY  Creata_a DESC", con);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);

                adapter.Fill(table);

            }

            return table;
        }
        public static DataTable Filtroo(string stringa, string stringa2, string tabella)
        {
            var table = new DataTable();

            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();

                MySqlCommand command = new MySqlCommand($"SELECT * FROM {tabella} WHERE {stringa} OR {stringa2} ORDER BY Creata_a DESC;", con);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);

                adapter.Fill(table);

            }

            return table;
        }

        
    }

}
