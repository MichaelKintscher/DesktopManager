using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDesktopManager.Models
{
    /// <summary>
    /// Represents a virtual desktop and the applications and webpages on it.
    /// </summary>
    internal class Workspace
    {
        #region Properties
        internal string Name { get; set; }
        internal bool[] Days { get; set; }
        internal List<AppInfo> AppInfo { get; set; }
        internal List<WebPageInfo> WebPageInfo { get; set; }
        #endregion

        #region Constructors
        internal Workspace()
        {
            // Initialize the properties to defaults.
            this.Name = "";
            this.Days = new bool[7];  // 7 boolean values, each represents a day of the week.
            this.AppInfo = new List<AppInfo>();
            this.WebPageInfo = new List<WebPageInfo>();
        }
        #endregion
    }
}
