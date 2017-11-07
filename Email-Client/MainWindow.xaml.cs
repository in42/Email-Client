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
                EmailUtils.SendMail(username.Text, password.Password, smptServer.Text, smptPort.Text,
                    MailFrom.Text, MailTo.Text, MailSubject.Text, MailBody.Text);

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

        private void logoutLogin_Click(object sender, RoutedEventArgs e)
        {
            if (LogInOut.Content.ToString() == "Logout")
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
                Home.IsSelected = true;
                LogInOut.Content = "Login";
                password.Visibility = Visibility.Visible;
            }
            else if (LogInOut.Content.ToString() == "Login")
            {
                smptPort.IsReadOnly = true;
                smptServer.IsReadOnly = true;
                username.IsReadOnly = true;
                popPort.IsReadOnly = true;
                popServer.IsReadOnly = true;
                password.Visibility = Visibility.Hidden;
                passLabel.Visibility = Visibility.Hidden;
                LogInOut.Content = "Logout";
            }
        }

        private void SentRefresh_Click(object sender, RoutedEventArgs e)
        {
        }

        private void InboxRefresh_Click(object sender, RoutedEventArgs e)
        {
            List<RecievedMail> items = EmailUtils
                .GetUnreadMessages("pop.gmail.com", 995, true, username.Text, password.Password)
                .Select(message => new RecievedMail() {
                    Sender = message.Headers.From.MailAddress.Address,
                    Subject = message.Headers.Subject,
                    Body = message.FindFirstPlainTextVersion().GetBodyAsText(),
                    Date = message.Headers.Date
                }).ToList();
            // items.add(new recievedmail() { sender = "john doe", subject = "dummy email", date = "08-11-2017" });
            // items.add(new recievedmail() { sender = "john doe", subject = "dummy email", date = "08-11-2017" });
            // items.add(new recievedmail() { sender = "john doe", subject = "dummy email", date = "08-11-2017" });
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
            if (item != null)
                InboxFrom.Text = item.Sender;
            else
                InboxFrom.Text = "";
            if (item != null)
                InboxSubject.Text = item.Subject;
            else
                InboxSubject.Text = "";
            if (item != null)
                InboxBody.Text = item.Body;
            else
                InboxBody.Text = "";
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
