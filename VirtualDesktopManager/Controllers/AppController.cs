using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualDesktopManager.EventArguments;
using VirtualDesktopManager.Managers;
using VirtualDesktopManager.Models;
using Windows.System;

namespace VirtualDesktopManager.Controllers
{
    /// <summary>
    /// The main controller for the app - handles navigation between app-level processes.
    /// </summary>
    internal class AppController : SingletonController<AppController>
    {
        #region Properties
        /// <summary>
        /// The root page for app navigation.
        /// </summary>
        public MainWindow RootPage { get; private set; }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Handles when a URI is invoked on the main window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MainWindow_UriInvoked(object sender, UriInvokedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("You launched " + e.Uri);
            Launcher.LaunchUriAsync(new Uri(e.Uri));
        }
        #endregion

        #region Methods
        /// <summary>
        /// Intended to be called upon app launch. Initializes app navigation.
        /// </summary>
        /// <param name="rootPage">The root page for this initialization of the app.</param>
        public void StartApp(MainWindow rootPage)
        {
            // Set the given page as the root page.
            this.RootPage = rootPage;

            // Subscribe to the root page's events.
            this.RootPage.UriInvoked += this.MainWindow_UriInvoked;

            // Initialize the home page.
            this.InitializeHomePageAsync(this.RootPage);
        }

        /// <summary>
        /// Initializes the Home Page view.
        /// </summary>
        /// <returns></returns>
        public async Task InitializeHomePageAsync(MainWindow homePage)
        {
            // Get a list of the workspace names.
            List<Workspace> workspaces = await WorkspaceDataManager.GetWorkspaces();

            // Set the view to display the workspaecs.
            homePage.DisplayWorkspaces(workspaces);
        }
        #endregion
    }
}
