using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Email_Client
{
    class EmailUtils
    {
        public static List<Message> GetUnreadMessages(string hostname, int port, bool useSsl, string username, string password)
        {
            using (Pop3Client client = new Pop3Client())
            {
                // Connect to the server
                client.Connect(hostname, port, useSsl);

                // Authenticate ourselves towards the server
                client.Authenticate(username, password);

                // Get the number of messages in the inbox
                int messageCount = client.GetMessageCount();

                // We want to download all messages
                List<Message> allMessages = new List<Message>(messageCount);

                // Messages are numbered in the interval: [1, messageCount]
                // Ergo: message numbers are 1-based.
                // Most servers give the latest message the highest number
                for (int i = messageCount; i > 0; i--)
                {
                    allMessages.Add(client.GetMessage(i));
                }

                // Now return the fetched messages
                return allMessages;
            }
        }
        
        public static void SendMail(string username, string password,
            string smtpServer, string smtpPort, string to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtpClient = new SmtpClient(smtpServer);

            mail.From = new MailAddress(username);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;

            smtpClient.Port = Int32.Parse(smtpPort);
            smtpClient.Credentials = new System.Net.NetworkCredential(username, password);
            smtpClient.EnableSsl = true;

            smtpClient.Send(mail);
        }

        public static void SendReadReceipt(string username, string password,
            string smtpServer, string smtpPort, string from, string to, string subject, string body)
        {
            SendMail(username, password, smtpServer, smtpPort, from, "READ RECEIPT: " +
                subject, body);
        }
    }
}
