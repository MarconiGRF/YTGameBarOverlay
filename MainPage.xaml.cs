using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.SmartCards;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace YoutubeGameBarOverlay {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string mediaURL = "";   

        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles the click on Play Button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private async void HandlePlayButton(object sender, RoutedEventArgs eventArgs)
        {
            PrepareToPlay();
        }

        /// <summary>
        /// Handles the keypresses on inputUrl TextBox.
        /// In case Enter/Return is pressed, executes the same funciton as clicking the play button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="keyArgs"></param>
        private async void HandleEnterPress(object sender, KeyRoutedEventArgs keyArgs)
        {
            if (keyArgs.Key == Windows.System.VirtualKey.Enter)
            {
                PrepareToPlay();
            }
        }

        /// <summary>
        /// Performs the necessary checks for validating the media URL, setting the XAML View states depending on the code worflow.
        /// </summary>
        private void PrepareToPlay()
        {
            InLoadingState(true);
            setInputAsMediaURL();

            if (isMediaURLValid() == true)
            {
                PrepareVideoUI();
            }
            else
            {
                InLoadingState(false);
                ShowErrorMessage("Invalid URL!");
            }
        }

        /// <summary>
        /// Shows an Error Message in the ErrorMessage TextBox XAML Element.
        /// </summary>
        /// <param name="errorMessage"></param>
        public void ShowErrorMessage(string errorMessage)
        {
            ErrorMessage.Text = errorMessage;
            ErrorMessage.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Sets a "Loading State" for the objects in XAML View, enabling and disabling functionality based on the passed value.
        /// </summary>
        /// <param name="value"></param>
        private void InLoadingState(bool value)
        {
            if (value == true)
            {
                LoadingRing.IsActive = true;
                ErrorMessage.Visibility = Visibility.Collapsed;
                inputUrlTextBox.IsEnabled = false;
                PlayButton.IsEnabled = false;
            }
            else
            {
                LoadingRing.IsActive = false;
                inputUrlTextBox.IsEnabled = true;
                PlayButton.IsEnabled = true;
            }
            
        }

        /// <summary>
        /// Sets the MediaURL as the current string on the TextBox.
        /// </summary>
        private void setInputAsMediaURL()
        {
            mediaURL = inputUrlTextBox.Text;
        }

        /// <summary>
        /// Checks if the MediaURL is a valid Youtube URL.
        /// </summary>
        private bool isMediaURLValid()
        {
            if (mediaURL.Length <= 32)
            {
                return false;
            }
            else
            {
                string youtubeBaseURLInput = mediaURL.Substring(0, 24);
                string youtubeBaseURLExpected = "https://www.youtube.com/";
                
                if (youtubeBaseURLInput == youtubeBaseURLExpected)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Prepares the necessary elements to compose the video UI.
        /// These elements are>
        /// - The iFrame Element
        /// - The Webview frame.
        /// </summary>
        private void PrepareVideoUI()
        {
            string iFrameElement = buildIFrameElement(getMediaId());
        }

        /// <summary>
        /// Gets and returns the Video ID from the Media URL.
        /// </summary>
        /// <returns></returns>
        private string getMediaId()
        {
            char argumentSeparator = '&';
            string mediaId = "";
            string videoSeparator = "v=";
            string playlistSeparator = "list=";

            try
            {
                mediaId = mediaURL.Split(playlistSeparator)[1].Split(argumentSeparator)[0];
                return mediaId;
            } 
            catch (IndexOutOfRangeException)
            {   
                mediaId = mediaURL.Split(videoSeparator)[1].Substring(0,11);
                return mediaId;
            }
        }

        /// <summary>
        /// Builds and returns string representing the iFrame HTML Element.
        /// </summary>
        /// <param name="mediaId">The Youtube Media ID.</param>
        /// <returns></returns>
        private string buildIFrameElement(string videoId)
        {
            string iframe = "";

            if (videoId.Length != 0)
            {

            }

            return iframe;
        }
    }
}
