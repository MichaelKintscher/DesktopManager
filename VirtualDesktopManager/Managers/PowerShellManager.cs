using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using VirtualDesktopManager.Models;

namespace VirtualDesktopManager.Managers
{
    /// <summary>
    /// Wrapper class for functions interacting with PowerShell.
    /// </summary>
    internal static class PowerShellManager
    {
        /// <summary>
        /// Launches the given workspace using the given powershell script.
        /// </summary>
        /// <param name="script"></param>
        /// <param name="workspace"></param>
        internal static void LaunchWorkspace(string script, Workspace workspace)
        {
            using (PowerShell powerShell = PowerShell.Create())
            {
                // Add the script to run from.
                powerShell.AddScript(script, false);

                powerShell.Invoke();

                powerShell.Commands.Clear();

                // Add the command in the script to run.
                powerShell.AddCommand("Setup-Workspaces").AddParameter("WorkspaceName", workspace.Name);

                // Execute the command from the script.
                var results = powerShell.Invoke();
                //System.Diagnostics.Debug.WriteLine("Stuff happened");
                //System.Diagnostics.Debug.WriteLine("THE SCRIPT\n\n\n");
            }
        }
    }
}
