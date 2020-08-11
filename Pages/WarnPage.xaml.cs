using System;
using System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using YoutubeGameBarWidget.Pages.PageObjects;
using YoutubeGameBarWidget.Utilities;

namespace YoutubeGameBarWidget.Pages
{
    /// <summary>
    /// A page to show a warning message to the user.
    /// </summary>
    public partial class WarnPage : Page
    {
        private WarnPayload Payload;
        public WarnPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        /// <summary>
        /// Sets and warns the user by the given message.
        /// </summary>
        /// <param name="e">The navigation arguments containing a WarnPayload.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.Payload = (WarnPayload)e.Parameter;
            this.WarnMessage.Text = Payload.Message;

            Painter.RunUIUpdateByMethod(WarnAndReturn);
            
            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Presents the warnig message for 1.5 seconds then navigates back to WebPage.
        /// </summary>
        private async void WarnAndReturn()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                        {
                            EnterWarn.Begin();
                        }
                    );

            Thread.Sleep(1500);

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                        {
                            ExitWarn.Begin();
                            this.Frame.Navigate(typeof(Webpage)); ;
                        }
                    );
        }
    }
}
