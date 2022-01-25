using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VirtualDesktopManager.EventArguments;
using VirtualDesktopManager.Models;
using VirtualDesktopManager.Pages;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VirtualDesktopManager
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        #region Properties
        /// <summary>
        /// The collection of workspaces the user has defined.
        /// </summary>
        internal ObservableCollection<Workspace> Workspaces { get; set; }

        private List<WorkspaceDetailPage.UriInvokedHandler> uriInvokedHandlers { get; set; }
        #endregion

        #region Events
        /// <summary>
        /// Bubbles up the Uri Invoked event from the dialog.
        /// </summary>
        internal event WorkspaceDetailPage.UriInvokedHandler UriInvoked
        {
            // This is based on the answer here: https://stackoverflow.com/questions/217233/bubbling-up-events
            //      Events are saved to a collection, to be added and removed when the page (publisher) instance
            //      is actually created.
            add { this.uriInvokedHandlers.Add(value); }
            remove { this.uriInvokedHandlers.Remove(value); }
        }

        internal delegate void WorkspaceLaunchRequestedHandler(object sender, WorkspaceLaunchRequestedEventArgs e);
        /// <summary>
        /// Raised when a workspace launch is requested.
        /// </summary>
        internal event WorkspaceLaunchRequestedHandler WorkspaceLaunchRequested;
        private void RaiseWorkspaceLaunchRequested(Workspace workspace)
        {
            // Create the args and call the listening event handlers, if there are any.
            WorkspaceLaunchRequestedEventArgs args = new WorkspaceLaunchRequestedEventArgs(workspace);
            this.WorkspaceLaunchRequested?.Invoke(this, args);
        }
        #endregion

        #region Constructors
        public MainWindow()
        {
            this.InitializeComponent();

            // Initialize the collection.
            this.Workspaces = new ObservableCollection<Workspace>();
            this.uriInvokedHandlers = new List<WorkspaceDetailPage.UriInvokedHandler>();
        }
        #endregion

        #region EventHandlers
        /// <summary>
        /// Handles when the Workspace Details button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void WorkspaceDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                // Create the workspace detail page and bind the model to it.
                WorkspaceDetailPage workspaceDetailPage = new WorkspaceDetailPage();
                workspaceDetailPage.WorkspaceModel = button.Tag as Workspace;

                // Subscribe to the events.
                this.uriInvokedHandlers.ForEach(handler => workspaceDetailPage.UriInvoked += handler);

                ContentDialog dialog = new ContentDialog()
                {
                    Content = workspaceDetailPage,
                    XamlRoot = this.WorkspaceGridView.XamlRoot,
                    CloseButtonText = "Back"
                };

                var result = await dialog.ShowAsync();

                // Unsubscribe from the events.
                this.uriInvokedHandlers.ForEach(handler => workspaceDetailPage.UriInvoked -= handler);
            }
        }

        /// <summary>
        /// Handles when the Launch Workspace button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LaunchWorkspaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                // Get the workspace that was selected.
                Workspace workspace = button.Tag as Workspace;

                // Raise the workspace launch requested event.
                this.RaiseWorkspaceLaunchRequested(workspace);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Displays the given list of workspaces in the view.
        /// </summary>
        /// <param name="workspaces">The list of workspae names to display</param>
        internal void DisplayWorkspaces(List<Workspace> workspaces)
        {
            // Clear the collection, then refill it with the given workspace list.
            this.Workspaces.Clear();
            workspaces.ForEach(workspace => this.Workspaces.Add(workspace));
        }
        #endregion
    }
}
