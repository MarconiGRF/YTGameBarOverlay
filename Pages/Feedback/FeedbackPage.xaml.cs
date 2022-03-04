using System;
using System.Net;
using System.Text;
using Windows.Data.Json;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using YTGameBarOverlay;
using YTGameBarWidget.Utilities;

namespace YTGameBarWidget
{
    /// <summary>
    /// A feedback page to share and improve app experiences using YTGameBar Feedback Server's services.
    /// 
    /// For more API details, see: https://github.com/MarconiGRF/YTGameBarFeedbackServer
    /// </summary>
    public sealed partial class FeedbackPage : Page
    {
        private Uri ytgbfsUri;
        private FeedbackResources LangResources;
        private ThemeResources ColorResources;
        public FeedbackPage()
        {
            ytgbfsUri = new Uri(String.Format(Constants.Endpoints.FSBase, Utils.GetVar(Constants.Vars.FSAddress), Utils.GetVar(Constants.Vars.FSPort)));
            LangResources = BabelTower.getTranslatedResources<FeedbackResources>();
            ColorResources = Painter.GetTheme();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            InitializeComponent();
        }

        /// <summary>
        /// Cleans the fields as soon as frame navigates to Feedback Page.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            FeedbackTextBox.Text = Constants.Common.EmptyString;
            FeedbackTextBox.IsEnabled = true;

            FeedBackAuthor.Text = Constants.Common.EmptyString;
            FeedBackAuthor.IsEnabled = true;

            SendButtonText.Text = this.LangResources.Send;
            SendButton.IsEnabled = true;

            ErrorMessage.Visibility = Visibility.Collapsed;

            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Handles the app behavior when Send button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Visibility = Visibility.Collapsed;

            if (FeedbackTextBox.Text.Length == 0)
            {
                ErrorMessage.Text = this.LangResources.NoFeedbackMessageError;
                ErrorMessage.Visibility = Visibility.Visible;
            }
            else if (FeedBackAuthor.Text.Length == 0)
            {
                ErrorMessage.Text = this.LangResources.NoFeedbackAuthorError;
                ErrorMessage.Visibility = Visibility.Visible;
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
            client.UploadDataAsync(ytgbfsUri, Constants.WebServer.POSTMethod, Encoding.UTF8.GetBytes(json.Stringify()));
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
                        SendButtonText.Text = LangResources.FailedError;
                        ErrorMessage.Text = LangResources.TryAgainWarn;
                        ErrorMessage.Visibility = Visibility.Visible;
                        LoadingRing.IsActive = false;
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
                        SendButton.IsEnabled = false;
                        FeedbackTextBox.IsEnabled = false;
                        FeedBackAuthor.IsEnabled = false;
                        LoadingRing.IsActive = true;
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
                        SendButtonText.Text = LangResources.Sent;
                        LoadingRing.IsActive = false;
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
            Frame.Navigate(typeof(MainPage));
        }
    }
}
