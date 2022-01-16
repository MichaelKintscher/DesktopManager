using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDesktopManager.Models
{
    /// <summary>
    /// Represents info about an application.
    /// </summary>
    internal class AppInfo
    {
        /// <summary>
        /// The user-given name of the app.
        /// </summary>
        internal string Name { get; set; }
        /// <summary>
        /// The install path of the app.
        /// </summary>
        internal string InstallPath { get; set; }
    }
}
