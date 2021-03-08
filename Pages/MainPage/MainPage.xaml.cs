using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using YoutubeGameBarWidget;
using YoutubeGameBarWidget.Pages;
using YoutubeGameBarWidget.Pages.PageObjects;
using YoutubeGameBarWidget.Search;
using YoutubeGameBarWidget.Utilities;

namespace YoutubeGameBarOverlay
{
    /// <summary>
    /// The main page of the overlay.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Search Search;
        private MainPageResources LangResources;
        private ThemeResources ColorResources;
        private Cabinet Cabinet;
        private bool inLoadingState;
        public string MediaURL;

        public MainPage()
        {
            Search = new Search();
            Search.FinishedFetchingResults += PresentResults;
            Search.FailedFetchingResults += PresentSearchError;

            LangResources = BabelTower.getTranslatedResources<MainPageResources>();
            ColorResources = Painter.GetTheme();

            Cabinet = new Cabinet();
            Cabinet.Initialize();

            NavigationCacheMode = NavigationCacheMode.Enabled;
            InitializeComponent();
        }

        /// <summary>
        /// Cleans the page variables as soon as frame navigates to MainPage.
        /// </summary>
        /// <param name="e">The navigation arguments.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MediaURL = Constants.Common.EmptyString;
            inputBox.Text = Constants.Common.EmptyString;
            InLoadingState(false);

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
            ListItems list = (ListItems)inputBox.ItemsSource;

            if (list != null && list.Count > 0)
            {
                SetAsMediaURL(list.ElementAt(0).MediaUrl);
                PrepareToPlay();
            }
            else
            {
                ShowErrorMessage(LangResources.VideoNotSelectedError);
            }
        }

        /// <summary>
        /// Handles the click at the Feedback Page Button navigating to it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void HandleFeedbackButton(object sender, RoutedEventArgs eventArgs)
        {
            Frame.Navigate(typeof(FeedbackPage));
        }

        /// <summary>
        /// Handles the click at the Changelog Page Button navigating to it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void HandleChangelogButton(object sender, RoutedEventArgs eventArgs)
        {
            Frame.Navigate(typeof(ChangelogPage));
        }

        /// Handles the click at the SupportMe Page Button navigating to it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void HandleSupportMeButton(object sender, RoutedEventArgs eventArgs)
        {
            Frame.Navigate(typeof(SupportMePage));
        }

        /// Handles the click at the History Page Button navigating to it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void HandleHistoryButton(object sender, RoutedEventArgs eventArgs)
        {
            Frame.Navigate(typeof(HistoryPage));
        }

        /// <summary>
        /// Handles the click at the SettingsPage Button navigating to it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void HandleSettingsButton(object sender, RoutedEventArgs eventArgs)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        /// <summary>
        /// Prepares the App to play the video by validating the Media URL and setting the page elements states depending on the URL Validity.
        /// </summary>
        private void PrepareToPlay()
        {
            InLoadingState(true);

            if (Validator.IsMediaURLValid(MediaURL) == true)
            {
                StartPlayback();
            }
            else
            {
                InLoadingState(false);
                ShowErrorMessage(LangResources.URLNotValidError);
            }
        }

        /// <summary>
        /// Set the instance MediaURL attribute as the given string.
        /// </summary>
        /// <param name="input">The string to be set as MediaURL</param>
        private void SetAsMediaURL(string input)
        {
            MediaURL = input;
        }

        /// <summary>
        /// Prepares the necessary elements to start the playback on the WebPage.
        /// 
        /// These elements are:
        /// 1 - The Information payload to be passed to Webpage.
        /// 2 - Clean, if any, garbage left on memory.
        /// 3 - Navigate to VideoUI's Webpage.
        /// </summary>
        private void StartPlayback()
        {
            InformationPayload information = new InformationPayload(Utils.GetProperVideoUIUri(MediaURL));
            
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Frame.Navigate(typeof(Webpage), information);
        }

        /// <summary>
        /// Handles the text changes on the Search bar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void inputBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (inputBox.Text == Constants.Common.EmptyString)
            {
                inputBox.IsSuggestionListOpen = false;
                sender.ItemsSource = new ListItems();
                Painter.RunUIUpdateByMethod(FalseLoading);
            }
            else if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                if (inputBox.Text.Length >= 8)
                {
                    string inputStart = inputBox.Text.Substring(0, 8);

                    if (Regex.IsMatch(inputStart, Validator.RegexPatterns.HTTPBaseExpected))
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
                SaveItem(chosenItem);
                SetAsMediaURL(chosenItem.MediaUrl);

                StartPlayback();
            }
        }

        private async void SaveItem(ListItem chosenItem)
        {
            HistoryEntry historyEntry = new HistoryEntry(
                chosenItem.MediaTitle,
                chosenItem.ChannelTitle,
                chosenItem.MediaUrl,
                chosenItem.Thumbnail,
                Enum.Parse<Constants.MediaTypes>(chosenItem.MediaTypeLiteral),
                DateTime.Now.ToShortDateString()
            );
            bool successfullySaved = await Cabinet.SaveEntry(historyEntry);

            if (!successfullySaved)
            {
                Frame.Navigate(typeof(WarnPage), new WarnPayload(LangResources.HistorySaveError, typeof(MainPage), 2000));
            }
        }

        /// <summary>
        /// Executes the search using the text available on inputBox.
        /// In case of a unkown exception is raised, shows a error message accordingly.
        /// </summary>
        private async Task DoSearch()
        {
            if (inLoadingState == false)
            {
                inLoadingState = true;
                Painter.RunUIUpdateByMethod(WeakLoading);
            }

            try
            {
                await Search.ByTerm(inputBox.Text);
            }
            catch (Exception ex)
            {
                if (!(ex is NotSupportedException))
                {
                    PresentSearchError(null, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Presents the results on AutoSuggestBox when the results are ready.
        /// This function is triggered by the Search's FinishedFetchingResults event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PresentResults(Object sender, EventArgs e)
        {
            if (inLoadingState == true)
            {
                InLoadingState(false);
            }
            inputBox.ItemsSource = null;
            inputBox.ItemsSource = Search.Retreive();
            inputBox.IsSuggestionListOpen = true;
        }

        /// <summary>
        /// Presents a seach error when something went wrong with the request to server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PresentSearchError(Object sender, EventArgs e)
        {
            InLoadingState(false);
            ShowErrorMessage(LangResources.SearchNotAvailableError);
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
                inLoadingState = true;
                Painter.RunUIUpdateByMethod(TrueLoading);
            }
            else
            {
                inLoadingState = false;
                Painter.RunUIUpdateByMethod(FalseLoading);
            }
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

                        if (ErrorMessage.Text != Constants.Common.EmptyString)
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
                        ErrorMessage.Text = Constants.Common.EmptyString;
                        ErrorMessage.Visibility = Visibility.Collapsed;
                    }
                );
        }
    }
}
