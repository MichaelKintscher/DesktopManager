using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDesktopManager.Models
{
    /// <summary>
    /// Represents info about a webpage.
    /// </summary>
    internal class WebPageInfo
    {
        /// <summary>
        /// User-given name for the web page.
        /// </summary>
        internal string Name { get; set; }
        /// <summary>
        /// The URL of the web page.
        /// </summary>
        internal string Url { get; set; }
    }
}
