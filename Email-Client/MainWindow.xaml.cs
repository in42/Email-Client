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
        private int cnt;
        private String Password;
        public MainWindow()
        {
            InitializeComponent();
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
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            List<Mail> items = new List<Mail>();
            items.Add(new Mail() { Sender = "John Doe", Subject = "Dummy Email", Date = "08-11-2017" });
            items.Add(new Mail() { Sender = "John Doe", Subject = "Dummy Email", Date = "08-11-2017" });
            items.Add(new Mail() { Sender = "John Doe", Subject = "Dummy Email", Date = "08-11-2017" });
            inbox.ItemsSource = items;
        }

        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            popPort.Text = "";
            popServer.Text = "";
            PopUsername.Text = "";
            PopPassword.Password = "";
            inbox.ItemsSource = null;
            selectedMessage.Visibility = Visibility.Hidden;
            MailInbox.IsSelected = true;

        }

        private void inbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedMessage.Visibility = Visibility.Visible;
            selectedMessage.IsSelected = true;
            Mail item = (sender as ListView).SelectedItem as Mail;
            InboxFrom.Text = item.Sender;
            InboxSubject.Text = item.Subject;



        }
    }

    public class Mail
    {
        public string Sender { get; set; }

        public string Subject { get; set; }

        public string Date { get; set; }
    }
}
