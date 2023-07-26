using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualDesktopManager.EventArguments
{
    /// <summary>
    /// Contains event info for when a URI is invoked.
    /// </summary>
    internal class UriInvokedEventArgs : EventArgs
    {
        /// <summary>
        /// The type of URI invoked (specific app, or a web url).
        /// </summary>
        internal string Type { get; private set; }
        /// <summary>
        /// The invoked URI.
        /// </summary>
        internal string Uri { get; private set; }
        /// <summary>
        /// The parameters of the invoked URI.
        /// </summary>
        internal IReadOnlyDictionary<string, string> Parameters { get; private set; }

        internal UriInvokedEventArgs(string type, string uri, Dictionary<string, string> parameters = null)
        {
            this.Type = type;
            this.Uri = uri;
            this.Parameters = parameters == null ? new Dictionary<string, string>() : parameters;
        }
    }
}
