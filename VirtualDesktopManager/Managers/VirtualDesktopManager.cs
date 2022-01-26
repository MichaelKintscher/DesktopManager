using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualDesktopManager.Managers.VirtualDesktops;

namespace VirtualDesktopManager.Managers
{
    /// <summary>
    /// Manages virtual desktops. Encapsulates the code taken from the Virtual Desktop project on GitHub: https://github.com/MScholtes/VirtualDesktop/blob/master/VirtualDesktop11.cs
    /// </summary>
    internal static class VirtualDesktopManager
    {
        /// <summary>
        /// Creates a new virtual desktop.
        /// </summary>
        /// <param name="name">The name to give the new virtual desktop.</param>
        /// <returns>The index of the new virtual desktop.</returns>
        internal static int CreateVirtualDesktop(string name)
        {
            // Create a new desktop and assign the desktop the given name.
            Desktop desktop = Desktop.Create();
            //desktop.SetName(name);

            // Get and return the index the desktop is at. The newly created
            //      desktop will be at the highest index (furthest right).
            return Desktop.Count - 1;
        }
    }
}
