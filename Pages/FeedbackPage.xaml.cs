using System;
using System.Net;
using System.Text;
using Windows.Data.Json;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using YoutubeGameBarOverlay;
using YoutubeGameBarWidget.Utilities;

namespace YoutubeGameBarWidget
{
    /// <summary>
    /// A feedback page to share and improve app experiences using Youtube Game Bar Feedback Server's services.
    /// 
    /// For more API details, see: https://github.com/MarconiGRF/YoutubeGameBarFeedbackServer
    /// </summary>
    public sealed partial class FeedbackPage : Page
    {
        private Uri ytgbfsUri;
        public FeedbackPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            this.ytgbfsUri = new Uri(String.Format(Constants.Endpoints.FSBase, Utils.GetVar(Constants.Vars.FSAddress), Utils.GetVar(Constants.Vars.FSPort)));
        }

        /// <summary>
        /// Cleans the fields as soon as frame navigates to Feedback Page.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.FeedbackTextBox.Text = Constants.Common.EmptyString;
            this.FeedbackTextBox.IsEnabled = true;

            this.FeedBackAuthor.Text = Constants.Common.EmptyString;
            this.FeedBackAuthor.IsEnabled = true;

            this.SendButtonText.Text = Constants.Common.Send;
            this.SendButton.IsEnabled = true;

            this.ErrorMessage.Visibility = Visibility.Collapsed;

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
                this.ErrorMessage.Text = Constants.Error.NoFeedbackMessageError;
                this.ErrorMessage.Visibility = Visibility.Visible;
            }
            else if (FeedBackAuthor.Text.Length == 0)
            {
                this.ErrorMessage.Text = Constants.Error.NoFeedbackAuthorError;
                this.ErrorMessage.Visibility = Visibility.Visible;
            }
            else
            {
                Painter.RunUIUpdateByMethod(SendingState);
                sendMessage();
            }

        }

        /// <summary>
        /// Composes a feedback message using the given page information and sends it using the current Feedback Page's attributes instance.
        /// </summary>
        private void sendMessage()
        {
            StringBuilder feedbackContent = new StringBuilder();
            feedbackContent.Append(FeedbackTextBox.Text);
            feedbackContent.Append("\n\n");
            feedbackContent.Append(Constants.Common.AuthorSignature + FeedBackAuthor.Text);

            JsonObject json = new JsonObject();
            json.Add("message", JsonValue.CreateStringValue(feedbackContent.ToString()));

            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.ContentType, Constants.Headers.Json);
            client.UploadDataCompleted += new UploadDataCompletedEventHandler(EvaluateResult);
            client.UploadDataAsync(this.ytgbfsUri, Constants.WebServer.POSTMethod, Encoding.UTF8.GetBytes(json.Stringify()));
        }

        /// <summary>
        /// Evaluates the result given by the response and acts accordingly to it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EvaluateResult(Object sender, UploadDataCompletedEventArgs e)
        {
            try
            {
                string result = Encoding.UTF8.GetString(e.Result);
                if (result == "OK")
                {
                    Painter.RunUIUpdateByMethod(DoneSending);
                }
            }
            catch
            {
                Painter.RunUIUpdateByMethod(ErrorSending);
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
                        this.SendButtonText.Text = Constants.Error.FailedError;
                        this.ErrorMessage.Text = Constants.Warn.TryAgainLater;
                        this.ErrorMessage.Visibility = Visibility.Visible;
                        this.LoadingRing.IsActive = false;
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
                        this.LoadingRing.IsActive = true;
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
                        this.SendButtonText.Text = Constants.Common.Sent;
                        this.LoadingRing.IsActive = false;
                    }
                );
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
