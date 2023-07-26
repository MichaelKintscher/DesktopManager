using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VirtualDesktopManager.EventArguments;
using VirtualDesktopManager.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VirtualDesktopManager.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WorkspaceDetailPage : Page
    {
        #region Properties
        /// <summary>
        /// The workspace model to display.
        /// </summary>
        internal Workspace WorkspaceModel { get; set; }
        #endregion

        #region Events
        internal delegate void UriInvokedHandler(object sender, UriInvokedEventArgs e);
        /// <summary>
        /// Raised when a Uri is invoked.
        /// </summary>
        internal event UriInvokedHandler UriInvoked;
        private void RaiseUriInvoked(string type, string uri, Dictionary<string, string> parameters = null)
        {
            // Create the args and call the listening event handlers, if there are any.
            UriInvokedEventArgs args = new UriInvokedEventArgs(type, uri, parameters);
            this.UriInvoked?.Invoke(this, args);
        }
        #endregion

        #region Constructors
        public WorkspaceDetailPage()
        {
            this.InitializeComponent();
        }
        #endregion

        #region Event Handlers
        private void WebPageListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Confirm the clicked item is a web page.
            if (e.ClickedItem is WebPageInfo webPageInfo)
            {
                string type = "Webpage";
                string uri = webPageInfo.Url;

                // Raise the uri invoked event.
                this.RaiseUriInvoked(type, uri);
            }
        }
        #endregion
    }
}
