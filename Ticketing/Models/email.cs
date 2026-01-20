using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;



namespace Ticketing
{
    public class email
    {
        public string receiver { get; set; }
        public string mailer { get; set; }
        public string subject { get; set; }
        public string body { get; set; }



        public static async Task sendMail(string receiver, string mailer, string subject, string body, int idTicket)
        {


            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress("mittente", mailer));
            mail.To.Add(new MailboxAddress("destinatario", receiver));
            mail.Subject = subject;
            mail.Body = new TextPart("plain")
            {
                Text = body
            };




            using (var client = new SmtpClient())
            {
                // Use smtp.gmail.com and StartTls on 587
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                // Use an app-specific password (do NOT use the account's primary password)
                await client.AuthenticateAsync("ticketdevtest@gmail.com", "devfg2426a1");
                await client.SendAsync(mail);
                await client.DisconnectAsync(true);
            }


            string cs = ConfigurationManager.ConnectionStrings["TicketingDb"].ConnectionString;

            using (MySqlConnection con = new MySqlConnection(cs))
            {
                con.Open();
                string nuovaTicket = @"INSERT INTO storico (Mittente, Destinatario, Oggetto, Messaggio, Ticket) 
VALUES (@mittente, @destinatario, @oggetto, @messaggio, @idTicket);
";

                MySqlCommand cmd = new MySqlCommand(nuovaTicket, con);

                cmd.Parameters.Add("@mittente", MySqlDbType.VarChar).Value = mailer;
                cmd.Parameters.Add("@destinatario", MySqlDbType.VarChar).Value = receiver;
                cmd.Parameters.Add("@messaggio", MySqlDbType.VarChar).Value = body;
                cmd.Parameters.Add("@oggetto", MySqlDbType.VarChar).Value = subject;
                cmd.Parameters.Add("@idTicket", MySqlDbType.Int32).Value = idTicket;


                cmd.ExecuteNonQuery();

            }
        }
    }
}