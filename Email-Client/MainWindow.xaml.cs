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
        private List<SentMail> outboxList;
        private List<ReceivedMail> inboxList;
        public MainWindow()
        {
            InitializeComponent();
            outboxList = new List<SentMail> ();
            outbox.ItemsSource = outboxList;
            inboxList = new List<ReceivedMail>();
            inbox.ItemsSource = inboxList;
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EmailUtils.SendMail(usernameField.Text, passwordField.Password, smtpServer.Text, smtpPort.Text,
                    MailTo.Text, MailSubject.Text, MailBody.Text);

                outboxList.Add(new SentMail() { SendTo = MailTo.Text, Subject = MailSubject.Text, Date = "08-11-2017", Read = false, Body= MailBody.Text });

                // smtpPort.Text = "";
                // smtpServer.Text = "";
                // usernameField.Text = "";
                // passwordField.Password = "";
                MailBody.Text = "";
               // MailFrom.Text = "";
                MailSubject.Text = "";
                MailTo.Text = "";

                MessageBox.Show("Mail Sent!");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void logoutLogin_Click(object sender, RoutedEventArgs e)
        {
            if (LogInOut.Content.ToString() == "Log Out")
            {
                smtpPort.Text = "";
                smtpServer.Text = "";
                usernameField.Text = "";
                passwordField.Password = "";
                MailBody.Text = "";
                //MailFrom.Text = "";
                MailSubject.Text = "";
                MailTo.Text = "";
                popPort.Text = "";
                popServer.Text = "";

                inboxList.Clear();
                inbox.Items.Refresh();
                outboxList.Clear();
                outbox.Items.Refresh();

                selectedMessage.Visibility = Visibility.Hidden;
                Home.IsSelected = true;
                passwordField.Visibility = Visibility.Visible;
                passLabel.Visibility = Visibility.Visible;

                smtpPort.IsReadOnly = false;
                smtpServer.IsReadOnly = false;
                usernameField.IsReadOnly = false;
                popPort.IsReadOnly = false;
                popServer.IsReadOnly = false;

                LogInOut.Content = "Log In";
            }
            else if (LogInOut.Content.ToString() == "Log In")
            {
                smtpPort.IsReadOnly = true;
                smtpServer.IsReadOnly = true;
                usernameField.IsReadOnly = true;
                popPort.IsReadOnly = true;
                popServer.IsReadOnly = true;

                passwordField.Visibility = Visibility.Hidden;
                passLabel.Visibility = Visibility.Hidden;

                LogInOut.Content = "Log Out";
            }
        }

        private void SentRefresh_Click(object sender, RoutedEventArgs e)
        {
        }

        private void InboxRefresh_Click(object sender, RoutedEventArgs e)
        {
            List<ReceivedMail> newItems = EmailUtils
                .GetUnreadMessages("pop.gmail.com", 995, true, usernameField.Text, passwordField.Password)
                .Select(message => new ReceivedMail() {
                    Sender = message.Headers.From.MailAddress.Address,
                    Subject = message.Headers.Subject,
                    Body = message.FindFirstPlainTextVersion().GetBodyAsText(),
                    Date = message.Headers.Date,
                    Read = false
                }).ToList();
            // items.add(new recievedmail() { sender = "john doe", subject = "dummy email", date = "08-11-2017" });
            // items.add(new recievedmail() { sender = "john doe", subject = "dummy email", date = "08-11-2017" });
            // items.add(new recievedmail() { sender = "john doe", subject = "dummy email", date = "08-11-2017" });
            inboxList.InsertRange(0, newItems);
            inbox.Items.Refresh();
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
            ReceivedMail item = (sender as ListView).SelectedItem as ReceivedMail;
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

    public class ReceivedMail
    {
        public string Sender { get; set; }

        public string Subject { get; set; }

        public string Date { get; set; }

        public string Body { get; set; }

        public bool Read { get; set; }
       
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
