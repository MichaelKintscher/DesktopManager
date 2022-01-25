using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace VirtualDesktopManager.Managers
{
    internal static class ScriptManager
    {
        /// <summary>
        /// Reads the script from the given file.
        /// </summary>
        /// <param name="scriptFileName">The file name to read the script from.</param>
        /// <returns>The full script as a string.</returns>
        internal static async Task<string> GetScriptAsync(string scriptFileName)
        {
            // Read the text from the file.
            string lines = "";
            try
            {
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                System.Diagnostics.Debug.WriteLine("Path: " + folder.Path);
                StorageFile tokenFile = await folder.GetFileAsync(scriptFileName);
                lines = await FileIO.ReadTextAsync(tokenFile);
            }
            catch (Exception ex)
            {
                // An IO exception occured, so return false.
                System.Diagnostics.Debug.WriteLine("Error accessing: " + ApplicationData.Current.LocalFolder.Path);
                return "";
            }

            return lines;
        }
    }
}
