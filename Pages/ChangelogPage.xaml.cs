using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Windows.ApplicationModel;
using Windows.Data.Json;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YoutubeGameBarOverlay;
using YoutubeGameBarWidget.Utilities;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace YoutubeGameBarWidget.Pages
{
    /// <summary>
    /// A changelog page aimed to show changes between YTGBO's versions using Github API.
    /// </summary>
    public sealed partial class ChangelogPage : Page
    {
        List<String> changelogContent;
        string currentVersion;
        string githubReleasesEndpoint;

        public ChangelogPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            this.githubReleasesEndpoint = Constants.Endpoints.Github;
        }

        /// <summary>
        /// Checks if contents have already been loaded then presents the page as soon as frame navigates to Changelog Page.
        /// </summary>
        /// <param name="e">The navigation arguments.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (this.currentVersion == null)
            {
                GetFormattedAppVersion();
                this.VersionDisplayer.Text += this.currentVersion;
            }

            if (this.changelogContent == null)
            {
                new Thread(new ThreadStart(StartLoading)).Start();
                GetGithubContent();
            }

            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Auxiliary method to asynchronously update UI on a Started Loading ocasion.
        /// </summary>
        public async void StartLoading()
        {

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        this.ScrollableContent.Visibility = Visibility.Collapsed;
                        this.LoadingRing.IsEnabled = true;
                        this.LoadingRing.IsActive = true;
                    }
                );
            
        }

        /// <summary>
        /// Auxiliary method to asynchronously update UI on a Finished Loading ocasion.
        /// </summary>
        public async void FinishLoading()
        {

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        this.LoadingRing.IsEnabled = false;
                        this.LoadingRing.IsActive = false;
                        this.ScrollableContent.Visibility = Visibility.Visible;
                    }
                );
            
        }

        /// <summary>
        /// Asynchronously fetchs YTGBO's Releases information using Github API.
        /// </summary>
        public void GetGithubContent()
        {
            WebClient client = new WebClient();
            client.DownloadDataCompleted += new DownloadDataCompletedEventHandler(ParseResults);
            client.Headers.Add(HttpRequestHeader.Accept, Constants.Headers.GithubJson);
            client.Headers.Add(HttpRequestHeader.UserAgent, Constants.Headers.CommonUserAgent);
            
            client.DownloadDataAsync(new Uri(this.githubReleasesEndpoint));
        }

        /// <summary>
        /// Parses and stores the received releases' information in changelogContent list to be displayed. 
        ///
        /// In case of error, stores an error message instead.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ParseResults(Object sender, DownloadDataCompletedEventArgs e)
        {
            this.changelogContent = new List<string>();

            try { 
                JsonArray jArray = JsonArray.Parse(Encoding.UTF8.GetString(e.Result));
                foreach(JsonValue jValue in jArray)
                {
                    JsonObject jObject = jValue.GetObject();
                    this.changelogContent.Add(jObject.GetNamedString("name"));
                    this.changelogContent.Add(jObject.GetNamedString("body"));
                }
            }
            catch
            {
                this.changelogContent.Add(Constants.Error.AmazingError);
                this.changelogContent.Add(Constants.Error.GithubCommunicationError);
            }

            DisplayResults();
        }

        /// <summary>
        /// Inserts and updates the UI with stylished elements based on the results stored on changelogContent.
        /// 
        /// Each element could be a Version Title or Version Description.
        /// </summary>
        public void DisplayResults()
        {
            for(int i = 0; i < this.changelogContent.Count; i++)
            {
                if (i%2 == 0)
                {
                    ScrollableContent.Children.Add(GetStyledTextBlock(this.changelogContent.ElementAt(i), "version"));
                }
                else
                {
                    ScrollableContent.Children.Add(GetStyledTextBlock(GetFormattedDescription(this.changelogContent.ElementAt(i)), "description"));
                }
            }

            new Thread(new ThreadStart(FinishLoading)).Start();
        }

        /// <summary>
        /// Parses the string received from github, keeping only non-markdown strings.
        /// </summary>
        /// <param name="rawstring">The RAW string received from github.</param>
        /// <returns></returns>
        public string GetFormattedDescription(string rawstring)
        {
            StringBuilder builder = new StringBuilder();
            string[] temp = rawstring.Split("\r\n");
            
            foreach(string descriptionPart in temp)
            {
                if (!descriptionPart.Equals("") && !descriptionPart.StartsWith("#") && !descriptionPart.StartsWith("**"))
                {
                    builder.AppendLine(descriptionPart);
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Creates and returns a Stylished TextBlock elements with the given message and style they should have.
        /// </summary>
        /// <param name="message">The messsage the element should have.</param>
        /// <param name="styling">The styling the element should have.</param>
        /// <returns>The correct TextBlock to compose the UI.</returns>
        public TextBlock GetStyledTextBlock(string message, string styling)
        {
            TextBlock styledTextBlock = new TextBlock();
            styledTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(Byte.MaxValue, Byte.MaxValue, Byte.MaxValue, Byte.MaxValue));
            styledTextBlock.Text = message;

            if (styling == "version")
            {
                styledTextBlock.FontSize = 20;
            }
            else if (styling == "description")
            {
                styledTextBlock.FontSize = 12;
                styledTextBlock.Margin = new Thickness(12, 0, 0 ,0);
                styledTextBlock.TextWrapping = TextWrapping.Wrap;
            }

            return styledTextBlock;
        }

        /// <summary>
        /// Gets and sets the formatted string for the App Version based on the package.
        /// </summary>
        /// <returns>String indicating the version in the following form: "X.x.x".</returns>
        private void GetFormattedAppVersion()
        {
            PackageVersion version = Package.Current.Id.Version;
            this.currentVersion = version.Major + "." + version.Minor + "." + version.Build;
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
