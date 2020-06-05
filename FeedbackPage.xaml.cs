using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Text;
using System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using YoutubeGameBarOverlay;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace YoutubeGameBarWidget
{
    /// <summary>
    /// A feedback page to share and improve app experiences.
    /// </summary>
    public sealed partial class FeedbackPage : Page
    {
        private Thread auxiliaryUIThread;
        public FeedbackPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Cleans the fields as soon as frame navigates to MainPage.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.FeedbackTextBox.Text = "";
            this.FeedbackTextBox.IsEnabled = true;

            this.FeedBackAuthor.Text = "";
            this.FeedBackAuthor.IsEnabled = true;

            this.SendButtonText.Text = "Send";
            this.SendButton.IsEnabled = true;

            base.OnNavigatedTo(e);
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            this.ErrorMessage.Visibility = Visibility.Collapsed;

            if (FeedbackTextBox.Text.Length == 0)
            {
                this.ErrorMessage.Text = "Please say something.";
                this.ErrorMessage.Visibility = Visibility.Visible;
            }
            else if (FeedBackAuthor.Text.Length == 0)
            {
                this.ErrorMessage.Text = "Please say your name.";
                this.ErrorMessage.Visibility = Visibility.Visible;
            }
            else
            {
                RunUIUpdateByMethod(SendingState);

                if (sendMessage() == true)
                {
                    this.SendButtonText.Text = "Sent!";
                    RunUIUpdateByMethod(DoneSending);
                }
                else
                {
                    RunUIUpdateByMethod(ErrorSending);
                }
            }

        }

        /// <summary>
        /// Auxiliary method to asynchronously update UI on a InLoadingState(true) ocasion.
        /// </summary>
        private async void ErrorSending()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        this.SendButtonText.Text = "Failed.";
                        this.ErrorMessage.Text = "Try again later.";
                        this.ErrorMessage.Visibility = Visibility.Visible;
                        this.LoadingRing.IsEnabled = false;
                        this.LoadingRing.Visibility = Visibility.Collapsed;
                    }
                );
        }

        /// <summary>
        /// Auxiliary method to asynchronously update UI on a InLoadingState(true) ocasion.
        /// </summary>
        private async void SendingState()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        this.SendButton.IsEnabled = false;
                        this.FeedbackTextBox.IsEnabled = false;
                        this.FeedBackAuthor.IsEnabled = false;
                        this.LoadingRing.IsEnabled = true;
                        this.LoadingRing.Visibility = Visibility.Visible;
                    }
                );
        }

        /// <summary>
        /// Auxiliary method to asynchronously update UI on a InLoadingState(true) ocasion.
        /// </summary>
        private async void DoneSending()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        this.LoadingRing.IsEnabled = false;
                        this.LoadingRing.Visibility = Visibility.Collapsed;
                    }
                );
        }

        /// <summary>
        /// Asynchronously runs an UI updated defined by the given method using the auxiliary thread.
        /// </summary>
        /// <param name="uiMethod"></param>
        private void RunUIUpdateByMethod(Action uiMethod)
        {
            this.auxiliaryUIThread = new Thread(new ThreadStart(uiMethod));
            this.auxiliaryUIThread.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool sendMessage()
        {
            var message = new MimeMessage();
            string sourceMail = System.Environment.GetEnvironmentVariable("YTGBO_SOURCE_MAIL_ADDRESS");
            string destinationMail = System.Environment.GetEnvironmentVariable("YTGBO_DESTINATION_MAIL_ADDRESS");
            string smtpServerAddress = System.Environment.GetEnvironmentVariable("YTGBO_SMTP_SERVER_ADDRESS");
            string smtpUser = System.Environment.GetEnvironmentVariable("YTGBO_SMTP_USER");
            string smtpPassword = System.Environment.GetEnvironmentVariable("YTGBO_SMTP_PASSWORD");

            message.From.Add(new MailboxAddress("YTGBO Feedbacker", sourceMail));
            message.To.Add(new MailboxAddress("Marconi Gomes", destinationMail));
            message.Subject = "Feedback";

            StringBuilder sb = new StringBuilder();
            sb.Append(FeedbackTextBox.Text);
            sb.Append("\n\n");
            sb.Append("Author: " + FeedBackAuthor.Text);

            message.Body = new TextPart("plain") { Text = sb.ToString() };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(smtpServerAddress, 25, false);
                    client.Authenticate(smtpUser, smtpPassword);

                    client.Send(message);
                    client.Disconnect(true);
                }
                catch (Exception)
                {
                    //Mailing error.
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Event Handler for clicks on the Back Button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
