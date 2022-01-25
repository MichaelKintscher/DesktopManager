using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualDesktopManager.Models;

namespace VirtualDesktopManager.EventArguments
{
    /// <summary>
    /// Contains event info for when a workspace launch is requested.
    /// </summary>
    internal class WorkspaceLaunchRequestedEventArgs : EventArgs
    {
        /// <summary>
        /// The workspace requested to be launched.
        /// </summary>
        internal Workspace Workspace { get; private set; }

        internal WorkspaceLaunchRequestedEventArgs(Workspace workspace)
        {
            this.Workspace = workspace;
        }
    }
}
