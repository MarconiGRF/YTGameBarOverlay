using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using YoutubeGameBarWidget.Pages;
using YoutubeGameBarWidget.Utilities;
using YoutubeGameBarWidget;
using System.Collections.Generic;
using YoutubeGameBarWidget.Pages.PageObjects;
using System;

namespace YoutubeGameBarOverlay
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private SettingsResources LangResources;
        private ThemeResources ColorResources;
        private List<SettingItem> AccentComboValues;
        private List<SettingItem> SecondaryComboValues;
        private List<SettingItem> AuxiliaryComboValues;
        private List<SettingItem> LanguageComboValues;

        public SettingsPage()
        {
            LangResources = BabelTower.getTranslatedResources<SettingsResources>();
            ColorResources = Painter.GetTheme();

            NavigationCacheMode = NavigationCacheMode.Enabled;
            InitializeComponent();
        }

        /// <summary>
        /// Sets the page variables as soon as frame navigates to SettingsPage.
        /// </summary>
        /// <param name="e">The navigation arguments.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BuildAccentComboValues();
            BuildSecondaryComboValues();
            BuildAuxiliaryComboValues();
            BuildLanguageComboValues();
        }

        /// <summary>
        /// Builds a list of Setting Items based on the "Accent Color" constants.
        /// </summary>
        /// <returns>The list of Setting Items with the Accent Color names and values.</returns>
        private void BuildAccentComboValues()
        {
            string currentAccentValue = (string)Utils.GetSettingValue(Constants.Settings.AccentColors["varname"]);
            SettingItem currentOption = new SettingItem(null, currentAccentValue);

            AccentComboValues = BuildColorSettingList(ref currentOption, Constants.Settings.AccentColors, Constants.Settings.ColorNames);
            AccentColorComboBox.ItemsSource = AccentComboValues;
            AccentColorComboBox.SelectedItem = currentOption;
        }

        /// <summary>
        /// Builds a list of Setting Items based on the "Secondary Color" constants.
        /// </summary>
        /// <returns>The list of Setting Items with the Secondary Color names and values.</returns>
        private void BuildSecondaryComboValues()
        {
            string currentSecondaryValue = (string)Utils.GetSettingValue(Constants.Settings.SecondaryColors["varname"]);
            SettingItem currentOption = new SettingItem(null, currentSecondaryValue);

            SecondaryComboValues = BuildColorSettingList(ref currentOption, Constants.Settings.SecondaryColors, Constants.Settings.ColorNames);
            SecondaryColorComboBox.ItemsSource = SecondaryComboValues;
            SecondaryColorComboBox.SelectedItem = currentOption;
        }

        /// <summary>
        /// Builds a list of Setting Items based on the "Secondary Color" constants.
        /// </summary>
        /// <returns>The list of Setting Items with the Secondary Color names and values.</returns>
        private void BuildAuxiliaryComboValues()
        {
            string currentAuxiliaryValue = (string)Utils.GetSettingValue(Constants.Settings.AuxiliaryColors["varname"]);
            SettingItem currentOption = new SettingItem(null, currentAuxiliaryValue);

            AuxiliaryComboValues = BuildColorSettingList(ref currentOption, Constants.Settings.AuxiliaryColors, Constants.Settings.AuxiliaryNames);
            AuxiliaryColorComboBox.ItemsSource = AuxiliaryComboValues;
            AuxiliaryColorComboBox.SelectedItem = currentOption;
        }

        /// <summary>
        /// Builds a list of Setting Items based on the "Secondary Color" constants.
        /// </summary>
        /// <returns>The list of Setting Items with the Secondary Color names and values.</returns>
        private void BuildLanguageComboValues()
        {
            string currentLanguageValue = (string)Utils.GetSettingValue(Constants.Settings.Languages["varname"]);
            SettingItem currentOption = new SettingItem(null, currentLanguageValue);

            LanguageComboValues = new List<SettingItem>();
            foreach (string languageName in Constants.Settings.LanguagesNames)
            {
                SettingItem comboOption = new SettingItem(languageName, Constants.Settings.Languages[languageName]);
                if (comboOption.RawValue == currentOption.RawValue)
                {
                    currentOption = comboOption;
                }
                LanguageComboValues.Add(comboOption);
            }

            LanguageComboBox.ItemsSource = LanguageComboValues;
            LanguageComboBox.SelectedItem = currentOption;
        }

        /// <summary>
        /// Builds and returns a List of SettingItem of colors, its values will be based on the given parameters so it can fit any color type.
        /// </summary>
        /// <param name="current">The current value of the color, to be filled with name.</param>
        /// <param name="settingsValues">The values of color for filling the list.</param>
        /// <returns></returns>
        private List<SettingItem> BuildColorSettingList(ref SettingItem current, Dictionary<string, string> settingsValues, string[] names)
        {
            List <SettingItem> values = new List<SettingItem>();
            foreach (string colorName in names)
            {
                SettingItem comboOption = new SettingItem(colorName, settingsValues[colorName]);
                if (comboOption.RawValue == current.RawValue)
                {
                    current = comboOption;
                }
                values.Add(comboOption);
            }

            return values;
        }

        /// <summary>
        /// Handles the app behavior when Save button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SettingItem selectedAccent = (SettingItem)AccentColorComboBox.SelectedItem;
            SettingItem selectedSecondary = (SettingItem)SecondaryColorComboBox.SelectedItem;
            SettingItem selectedAuxiliary = (SettingItem)AuxiliaryColorComboBox.SelectedItem;
            SettingItem selectedLanguage= (SettingItem)LanguageComboBox.SelectedItem;

            if (selectedAccent.RawValue == Constants.Settings.AccentColors["Black"])
            {
                if (selectedAuxiliary.RawValue == Constants.Settings.AuxiliaryColors["Black"])
                {
                    //Unreadable color choice, falls back to white.
                    string white = Constants.Settings.AuxiliaryNames[1];
                    selectedAuxiliary = new SettingItem(white, Constants.Settings.AuxiliaryColors[white]);
                }
            }

            Utils.setSettingValue(Constants.Settings.AccentColors["varname"], selectedAccent.RawValue);
            Utils.setSettingValue(Constants.Settings.SecondaryColors["varname"], selectedSecondary.RawValue);
            Utils.setSettingValue(Constants.Settings.AuxiliaryColors["varname"], selectedAuxiliary.RawValue);
            Utils.setSettingValue(Constants.Settings.Languages["varname"], selectedLanguage.RawValue);

            CleanAndNavigateBack();
        }

        /// <summary>
        /// Cleans the UI objects and calls a GC collect to prevent memory leaks, then navigates to MainPage.
        /// </summary>
        private void CleanAndNavigateBack()
        {
            AccentColorComboBox.ItemsSource = null;
            AccentComboValues = null;
            AccentColorComboBox.ItemsSource = AccentComboValues;

            SecondaryColorComboBox.ItemsSource = null;
            SecondaryComboValues = null;
            SecondaryColorComboBox.ItemsSource = SecondaryComboValues;

            AuxiliaryColorComboBox.ItemsSource = null;
            AuxiliaryComboValues = null;
            AuxiliaryColorComboBox.ItemsSource = AuxiliaryComboValues;

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Frame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// Event Handler for clicks on the Back Button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            CleanAndNavigateBack();
        }
    }
}
