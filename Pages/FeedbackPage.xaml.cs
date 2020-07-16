using System;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;
using Windows.Data.Json;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using YoutubeGameBarOverlay;

namespace YoutubeGameBarWidget
{
    /// <summary>
    /// A feedback page to share and improve app experiences using Youtube Game Bar Feedback Server's services.
    /// 
    /// For more API details, see: https://github.com/MarconiGRF/YoutubeGameBarFeedbackServer
    /// </summary>
    public sealed partial class FeedbackPage : Page
    {
        private Thread auxiliaryUIThread;
        private Uri ytgbfsUri;
        public FeedbackPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            this.ytgbfsUri = new Uri(
                "http://" + 
                Environment.GetEnvironmentVariable("YTGBFS_ADDRESS") + 
                ":" +
                Environment.GetEnvironmentVariable("YTGBFS_PORT") +
                "/feedback");
        }

        /// <summary>
        /// Cleans the fields as soon as frame navigates to Feedback Page.
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

        /// <summary>
        /// Handles the app behavior when Send button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                sendMessage();
            }

        }

        /// <summary>
        /// Composes a message using the given page information and sends it using the current environment variables configuration.
        /// </summary>
        /// <returns></returns>
        private void sendMessage()
        {
            StringBuilder feedbackContent = new StringBuilder();
            feedbackContent.Append(FeedbackTextBox.Text);
            feedbackContent.Append("\n\n");
            feedbackContent.Append("Author: " + FeedBackAuthor.Text);

            JsonObject json = new JsonObject();
            json.Add("message", JsonValue.CreateStringValue(feedbackContent.ToString()));

            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            client.UploadDataCompleted += new UploadDataCompletedEventHandler(EvaluateResult);
            client.UploadDataAsync(this.ytgbfsUri, "POST", Encoding.UTF8.GetBytes(json.Stringify()));
        }

        private void EvaluateResult(Object sender, UploadDataCompletedEventArgs e)
        {
            try
            {
                string result = Encoding.UTF8.GetString(e.Result);
                if (result == "OK")
                {
                    RunUIUpdateByMethod(DoneSending);
                }
            }
            catch
            {
                RunUIUpdateByMethod(ErrorSending);
            }
        }

        /// <summary>
        /// Auxiliary method to asynchronously update UI on a "Error Sending feedback" ocasion.
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
        /// Auxiliary method to asynchronously update UI on a "Sending feedback" ocasion.
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
        /// Auxiliary method to asynchronously update UI on a "Finished sending feedback" ocasion.
        /// </summary>
        private async void DoneSending()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        this.SendButtonText.Text = "Sent!";
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
