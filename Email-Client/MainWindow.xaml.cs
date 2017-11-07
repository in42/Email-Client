using OpenPop.Mime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Email_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<SentMail> OutboxList;
        public MainWindow()
        {
            InitializeComponent();
            OutboxList = new List<SentMail> ();
            outbox.ItemsSource = OutboxList;
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(smptServer.Text);

                mail.From = new MailAddress(MailFrom.Text);
                mail.To.Add(MailTo.Text);
                mail.Subject = MailSubject.Text;
                mail.Body = MailBody.Text;

                SmtpServer.Port = Int32.Parse(smptPort.Text);
                SmtpServer.Credentials = new System.Net.NetworkCredential(username.Text,password.Password);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                OutboxList.Add(new SentMail() { SendTo = MailTo.Text, Subject = MailSubject.Text, Date = "08-11-2017", Read = false, Body= MailBody.Text });

                smptPort.Text = "";
                smptServer.Text = "";
                username.Text = "";
                password.Password = "";
                MailBody.Text = "";
                MailFrom.Text = "";
                MailSubject.Text = "";
                MailTo.Text = "";

                MessageBox.Show("mail Send");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            smptPort.Text = "";
            smptServer.Text = "";
            username.Text = "";
            password.Password = "";
            MailBody.Text = "";
            MailFrom.Text = "";
            MailSubject.Text = "";
            MailTo.Text = "";
            popPort.Text = "";
            popServer.Text = "";
            // PopUsername.Text = "";
            //PopPassword.Password = "";
            inbox.ItemsSource = new List<RecievedMail>();
            selectedMessage.Visibility = Visibility.Hidden;
            MailInbox.IsSelected = true;
        }

        private void SentRefresh_Click(object sender, RoutedEventArgs e)
        {
        }

        private void InboxRefresh_Click(object sender, RoutedEventArgs e)
        {
            List<RecievedMail> items = new List<RecievedMail>();
            items.Add(new RecievedMail() { Sender = "John Doe", Subject = "Dummy Email", Date = "08-11-2017" });
            items.Add(new RecievedMail() { Sender = "John Doe", Subject = "Dummy Email", Date = "08-11-2017" });
            items.Add(new RecievedMail() { Sender = "John Doe", Subject = "Dummy Email", Date = "08-11-2017" });
            inbox.ItemsSource = items;
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            popPort.Text = "";
            popServer.Text = "";
           // PopUsername.Text = "";
            //PopPassword.Password = "";
            inbox.ItemsSource = null;
            selectedMessage.Visibility = Visibility.Hidden;
            Home.IsSelected = true;

        }

        private void inbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedMessage.Visibility = Visibility.Visible;
            selectedMessage.IsSelected = true;
            RecievedMail item = (sender as ListView).SelectedItem as RecievedMail;
            if (item!= null)
                InboxFrom.Text = item.Sender;
            else
                InboxFrom.Text = "";
            if (item != null)
                InboxSubject.Text = item.Subject;
            else
                InboxSubject.Text = "";



        }
    }

    public class RecievedMail
    {
        public string Sender { get; set; }

        public string Subject { get; set; }

        public string Date { get; set; }

        public string Body { get; set; }

        
    }

    public class SentMail
    {
        public string SendTo { get; set; }

        public string Subject { get; set; }

        public string Date { get; set; }

        public string Body { get; set; }

        public bool Read { get; set; }

    }
}
