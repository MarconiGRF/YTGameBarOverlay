using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YoutubeGameBarWidget.Utilities
{
    public class Painter
    {
        // private Thread UIThread;

        /// <summary>
        /// Asynchronously runs an UI updated defined by the given method using the UIUpdate thread.
        /// </summary>
        /// <param name="uiMethod">The UI update method to be executed.</param>
        public static void RunUIUpdateByMethod(Action uiMethod)
        {
            Thread UIThread = new Thread(new ThreadStart(uiMethod));
            UIThread.Start();
        }
    }
}
