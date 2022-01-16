using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualDesktopManager.Models;
using Windows.Data.Json;
using Windows.Storage;

namespace VirtualDesktopManager.Managers
{
    /// <summary>
    /// Serves as the interface for manipulating the settings.json
    /// file containing all of the workspace data.
    /// </summary>
    internal static class WorkspaceDataManager
    {
        /// <summary>
        /// Gets the workspaces the user has defined.
        /// </summary>
        /// <returns></returns>
        internal static async Task<List<Workspace>> GetWorkspaces()
        {
            List<Workspace> workspaces = new List<Workspace>();

            string fileContent = "";
            try
            {
                // Get a reference to the local app data folder.
                StorageFolder folder = ApplicationData.Current.LocalFolder;
                System.Diagnostics.Debug.WriteLine(folder.Path);
                StorageFile jsonFile = await folder.CreateFileAsync("settings.json", CreationCollisionOption.OpenIfExists);
                fileContent = await FileIO.ReadTextAsync(jsonFile);
            }
            catch (Exception ex)
            {
                // An IO exception occured.
                System.Diagnostics.Debug.WriteLine("Error accessing: " + ApplicationData.Current.LocalFolder.Path);
            }

            // Parse the JSON to get each workspace name.
            JsonObject jsonObject = JsonObject.Parse(fileContent);
            JsonArray workspacesArray = jsonObject["workspaces"].GetArray();
            foreach (var workspaceJson in workspacesArray)
            {
                // Create the workspace object.
                Workspace workspace = new Workspace();

                JsonObject workspaceObject = workspaceJson.GetObject();
                workspace.Name = workspaceObject["name"].GetString();

                // Add the workspace name to the list.
                workspaces.Add(workspace);
            }

            return workspaces;
        }
    }
}
