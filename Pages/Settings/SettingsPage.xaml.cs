using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using YoutubeGameBarWidget.Pages;
using YoutubeGameBarWidget.Utilities;
using YoutubeGameBarWidget;

namespace YoutubeGameBarOverlay
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private SettingsResources LangResources;
        private ThemeResources ColorResources;

        public SettingsPage()
        {
            LangResources = BabelTower.getTranslatedResources<SettingsResources>();
            //ColorResources = Painter.GetTheme();
            InitializeComponent();
        }

        /// <summary>
        /// Sets the page variables as soon as frame navigates to SettingsPage.
        /// </summary>
        /// <param name="e">The navigation arguments.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //AccentColorComboBox.ItemsSource = BuildAccentComboValues();
        }

        /// <summary>
        /// Builds a list of Setting Items based on the "Accent Color" constants.
        /// </summary>
        /// <returns>The list of Setting Items with the Accent Color names and values.</returns>
        

        /// <summary>
        /// Handles the app behavior when Save button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //SettingItem selectedAccent = (SettingItem)AccentColorComboBox.SelectedItem;
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
