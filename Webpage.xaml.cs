using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace YoutubeGameBarWidget
{
    /// <summary>
    /// A webview page able to display the desired video.
    /// </summary>
    public sealed partial class Webpage : Page
    {
        public WebView VideoUIWebpage;
        public Webpage()
        {
            this.InitializeComponent(); 
            this.VideoUIWebpage = _videoUIWebpage;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.VideoUIWebpage.Navigate((Uri)e.Parameter);
            base.OnNavigatedTo(e);
        }
    }
}
