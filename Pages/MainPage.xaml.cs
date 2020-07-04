using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using YoutubeGameBarWidget;
using YoutubeGameBarWidget.Pages;
using YoutubeGameBarWidget.Pages.PageObjects;
using YoutubeGameBarWidget.Search;

namespace YoutubeGameBarOverlay
{
    /// <summary>
    /// The main page of the overlay. It exists to the user input and validate the URL, invoke webserver and redirect it to the webpage.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Search search;
        private Thread UIUpdateThread;
        private bool inLoadingState;
        public string mediaURL;

        public MainPage()
        {
            this.search = new Search();
            this.search.FinishedFetchingResults += PresentResults;
            this.search.FailedFetchingResults += PresentSearchError;
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        /// <summary>
        /// Cleans the page variables as soon as frame navigates to MainPage.
        /// </summary>
        /// <param name="e">The navigation arguments.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.mediaURL = "";
            this.inputBox.Text = "";
            this.InLoadingState(false);

            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Handles the click on Play Button.
        /// 
        /// In case of no video has been searched, show an error.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void HandlePlayButton(object sender, RoutedEventArgs eventArgs)
        {
            ListItems list = (ListItems)this.inputBox.ItemsSource;

            if (list != null && list.Count > 0)
            {
                SetAsMediaURL(list.ElementAt(0).MediaUrl);
                PrepareToPlay();
            }
            else
            {
                ShowErrorMessage("Please select a video.");
            }
        }

        /// <summary>
        /// Handles the click at the Feedback Button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void HandleFeedbackButton(object sender, RoutedEventArgs eventArgs)
        {
            this.Frame.Navigate(typeof(FeedbackPage));
        }

        /// <summary>
        /// Handles the click at the Changelog Button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void HandleChangelogButton(object sender, RoutedEventArgs eventArgs)
        {
            this.Frame.Navigate(typeof(ChangelogPage));
        }

        /// <summary>
        /// Prepares the App to play the video by validating the Media URL and setting the page elements states depending on the URL Validity.
        /// </summary>
        private void PrepareToPlay()
        {
            InLoadingState(true);

            if (IsMediaURLValid() == true)
            {
                PrepareVideoUI();
            }
            else
            {
                InLoadingState(false);
                ShowErrorMessage("The URL is not valid!");
            }
        }

        /// <summary>
        /// Shows an Error Message in the ErrorMessage element.
        /// </summary>
        /// <param name="errorMessage">The error message to be displayed.</param>
        private async void ShowErrorMessage(string errorMessage)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {

                        ErrorMessage.Text = errorMessage;
                        ErrorMessage.Visibility = Visibility.Visible;
                    }
                );
        }

        /// <summary>
        /// Sets a "Loading State" for the objects in the page, enabling and disabling functionality based on the given value.
        /// </summary>
        /// <param name="value">The value indicating if it is in a loading state or not.</param>
        private void InLoadingState(bool value)
        {
            if (value == true)
            {
                this.inLoadingState = true;
                RunUIUpdateByMethod(TrueLoading);
            }
            else
            {
                this.inLoadingState = false;
                RunUIUpdateByMethod(FalseLoading);
            }
        }

        /// <summary>
        /// Set the instance MediaURL attribute as the given string.
        /// </summary>
        /// <param name="input">The string to be set as MediaURL</param>
        private void SetAsMediaURL(string input)
        {
            this.mediaURL = input;
        }

        /// <summary>
        /// Checks if the MediaURL is a valid Youtube video, live  or playlist URL.
        /// </summary>
        public bool IsMediaURLValid()
        {
            string userRoute = "/user/";
            string channelRoute = "/channel/";
            string redirKW = "redirect";
            string eventQuery = "event=";

            if (mediaURL.Contains(userRoute) || mediaURL.Contains(channelRoute) || mediaURL.Contains(redirKW) || mediaURL.Contains(eventQuery))
            {
                return false;
            }
            else
            {
                if (mediaURL.Length <= 32)
                {
                    Regex ytCompressedUrlRegex = new Regex(@"https?:\/\/(www\.)?[-a-zA-Z]{1,10}\.[a-zA-Z]{1,6}\b(\/)");
                    if (ytCompressedUrlRegex.IsMatch(mediaURL))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    string ytBaseURLInput = mediaURL.Substring(0, 24);
                    Regex ytBaseExpectedRegex = new Regex(@"https?:\/\/(www\.)?[-a-zA-Z]{1,10}\.[a-zA-Z]{1,6}\b([-a-zA-Z.\/]*)");


                    if (ytBaseExpectedRegex.IsMatch(ytBaseURLInput))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Prepares the necessary elements to compose the video UI.
        /// 
        /// These elements are:
        /// 1 - The Information payload to be passed to Webpage.
        /// 2 - Navigate to Webpage.
        /// </summary>
        private void PrepareVideoUI()
        {
            InformationPayload information = new InformationPayload(GetProperVideoUIUrl(), this);

            this.Frame.Navigate(typeof(Webpage), information);
        }

        public Uri GetProperVideoUIUrl()
        {
            Uri properVideoUIURI;
            string baseAddress = "http://localhost:54523/?";
            string videoQuerystring = "videoId=";
            string playlistQuerystring = "listId=";

            string mediaId = GetMediaId(this.mediaURL);
            if (mediaId.Length > 11)
            {
                properVideoUIURI = new Uri(baseAddress + playlistQuerystring + mediaId);
            }
            else
            {
                properVideoUIURI = new Uri(baseAddress + videoQuerystring + mediaId);
            }

            return properVideoUIURI;
        }

        /// <summary>
        /// Gets the proper URI following YTGBO's VideoUI format.
        /// Supported formats are described at https://github.com/MarconiGRF/YoutubeGameBarVideoUI
        /// </summary>
        /// <returns>A compatible VideoUI URI.</returns>
        public Uri GetProperVideoUIUri()
        {
            Uri properVideoUIURI;
            string baseAddress = "http://localhost:54523/?";
            string videoQuerystring = "videoId=";
            string playlistQuerystring = "listId=";

            string mediaId = GetMediaId(this.mediaURL);
            if (mediaId.Length > 11)
            {
                properVideoUIURI = new Uri(baseAddress + playlistQuerystring + mediaId);
            }
            else
            {
                properVideoUIURI = new Uri(baseAddress + videoQuerystring + mediaId);
            }

            return properVideoUIURI;
        }

        /// <summary>
        /// Parses the Video/Live ID from the given Media URL.
        /// </summary>
        /// <returns></returns>
        public string GetMediaId(string url)
        {
            char argumentSeparator = '&';
            char dashSeparator = '/';
            string videoSeparator = "v=";
            string listSeparator = "list=";

            try
            {
                return mediaURL.Split(listSeparator)[1].Split(argumentSeparator).First();
            }
            catch (IndexOutOfRangeException)
            {
                try
                {
                    return url.Split(videoSeparator)[1].Substring(0, 11);
                }
                catch (IndexOutOfRangeException)
                {
                    return url.Split(dashSeparator).Last();
                }
            }
        }

        /// <summary>
        /// Asynchronously runs an UI updated defined by the given method using the UIUpdate thread.
        /// </summary>
        /// <param name="uiMethod">The UI update method to be executed.</param>
        private void RunUIUpdateByMethod(Action uiMethod)
        {
            this.UIUpdateThread = new Thread(new ThreadStart(uiMethod));
            this.UIUpdateThread.Start();
        }

        /// <summary>
        /// Auxiliary method to asynchronously update UI on a InLoadingState(true) ocasion.
        /// </summary>
        private async void TrueLoading()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        LoadingRing.IsActive = true;
                        ErrorMessage.Visibility = Visibility.Collapsed;
                        inputBox.IsEnabled = false;
                        PlayButton.IsEnabled = false;
                    }
                );
        }

        /// <summary>
        /// Auxiliary method to asynchronously update UI on a InLoadingState(false) ocasion.
        /// </summary>
        private async void FalseLoading()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        LoadingRing.IsActive = false;
                        inputBox.IsEnabled = true;
                        PlayButton.IsEnabled = true;

                        if (ErrorMessage.Text != "")
                        {
                            ErrorMessage.Visibility = Visibility.Visible;
                        }
                    }
                );
        }

        /// <summary>
        /// Auxiliary method to asynchronously update UI on to a weak loading state, without locking all page elements.
        /// </summary>
        private async void WeakLoading()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        LoadingRing.IsActive = true;
                        inputBox.IsSuggestionListOpen = false;
                        ErrorMessage.Visibility = Visibility.Collapsed;
                    }
                );
        }

        /// <summary>
        /// Handles the text changes on the Search bar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void inputBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (inputBox.Text == "")
            {
                inputBox.IsSuggestionListOpen = false;
                sender.ItemsSource = new ListItems();
                RunUIUpdateByMethod(FalseLoading);
            }
            else if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (inputBox.Text.Length >= 8)
                {
                    string inputStart = inputBox.Text.Substring(0, 8);
                    Regex urlBaseRegex = new Regex(@"https?:\/\/");

                    if (urlBaseRegex.IsMatch(inputStart))
                    {
                        SetAsMediaURL(inputBox.Text);
                        PrepareToPlay();
                    }
                    else
                    {
                        await DoSearch();
                    }
                }
                else
                {
                    await DoSearch();
                }
            }
        }

        /// <summary>
        /// Executes the search using the text available on inputBox.
        /// In case of a unkown exception is raised, shows a error message accordingly.
        /// </summary>
        private async Task DoSearch()
        {
            if (this.inLoadingState == false)
            {
                this.inLoadingState = true;
                RunUIUpdateByMethod(WeakLoading);
            }

            await this.search.ByTerm(inputBox.Text);
        }

        /// <summary>
        /// Presents the results on AutoSuggestBox when the results are ready.
        /// This function is triggered by the Search's FinishedFetchingResults event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PresentResults(Object sender, EventArgs e)
        {
            if (this.inLoadingState == true)
            {
                InLoadingState(false);
            }

            inputBox.ItemsSource = this.search.Retreive();
            inputBox.IsSuggestionListOpen = true;
        }

        public void PresentSearchError(Object sender, EventArgs e)
        {
            InLoadingState(false);
            ShowErrorMessage("Search is not available now, please use links.");
        }

        /// <summary>
        /// Handles the app behavior based on the selected suggestion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void inputBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                InLoadingState(true);
                ListItem chosenItem = (ListItem)args.ChosenSuggestion;
                SetAsMediaURL(chosenItem.MediaUrl);

                PrepareVideoUI();
            }
        }
    }
}
