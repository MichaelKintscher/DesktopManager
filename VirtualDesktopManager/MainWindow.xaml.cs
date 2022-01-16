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
        public ObservableCollection<string> Workspaces { get; set; }
        #endregion

        #region Constructors
        public MainWindow()
        {
            this.InitializeComponent();

            // Initialize the collection.
            this.Workspaces = new ObservableCollection<string>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Displays the given list of workspaces in the view.
        /// </summary>
        /// <param name="workspaces">The list of workspae names to display</param>
        public void DisplayWorkspaces(List<string> workspaces)
        {
            // Clear the collection, then refill it with the given workspace list.
            this.Workspaces.Clear();
            workspaces.ForEach(workspace => this.Workspaces.Add(workspace));
        }
        #endregion
    }
}
