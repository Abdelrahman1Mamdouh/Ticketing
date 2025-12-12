using MySql.Data.MySqlClient;
using System;
using System.Configuration;

namespace Ticketing
{
    public partial class Login : System.Web.UI.Page
    {
        string userName;
        string password;
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;
            userName = TUser.Text.Trim();
            password = TPass.Text.Trim();

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string query = "SELECT COUNT(1) FROM Utente WHERE Email=@Email AND Pass=@Pass";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = userName;
                cmd.Parameters.Add("@Pass", MySqlDbType.VarChar).Value = password;


                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count == 1)
                {
                    Response.Redirect("Dashboard.aspx");
                }
                else
                {
                    Response.Write("Username o password sono invalidi");
                }
            }
        }
        protected void BtnAnnulla_Click(object sender, EventArgs e)
        {
            userName = TUser.Text = "";
            password = TPass.Text = "";
        }
    }
}