using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.Components.Dialogs
{
    class PathsTab : TabPage, IComponent
    {
        ConfigurationLink hadesPath, pythonPath, defaultPath, resourcesPath;
        Button saveButton;
        public PathsTab() : base("Paths")
        {
            Initialize();
            Populate();
        }
        public void Initialize()
        {
            Dock = DockStyle.Fill;
            saveButton = new Button();
            saveButton.Image = Image.FromFile(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Res\save_button.png");
            saveButton.Dock = DockStyle.Bottom;
            hadesPath = new ConfigurationLink("Hades Path", saveButton);
            pythonPath = new ConfigurationLink("Python Path", saveButton);
            defaultPath = new ConfigurationLink("Default Project Path", saveButton);
            resourcesPath = new ConfigurationLink("Resources Path", saveButton);

            Controls.Add(saveButton);
            Controls.Add(hadesPath);
            Controls.Add(pythonPath);
            Controls.Add(defaultPath);
            Controls.Add(resourcesPath);
        }

        public void Populate()
        {
            ConfigManager configManager = ConfigManager.GetInstance();
            Dictionary<string, string> configs = configManager.configs.GetAllConfigs();
            hadesPath.SetNewPath(configs["HadesPath"]);
            pythonPath.SetNewPath(configs["PythonPath"]);
            defaultPath.SetNewPath(configs["DefaultPath"]);
            resourcesPath.SetNewPath(configs["ResourcesPath"]);

            saveButton.Click += (s, e) => Action_SaveButton(s, e);
            hadesPath.browseButton.Click += (s, e) => Action_HadesPath_BrowseButton(s, e);
            pythonPath.browseButton.Click += (s, e) => Action_PythonPath_BrowseButton(s, e);
            defaultPath.browseButton.Click += (s, e) => Action_DefaultPath_BrowseButton(s, e);
            resourcesPath.browseButton.Click += (s, e) => Action_ResourcesPath_BrowseButton(s, e);
            saveButton.Enabled = false;
        }
        private void Action_SaveButton(object sender, EventArgs e)
        {
            ConfigManager configManager = ConfigManager.GetInstance();
            configManager.SaveConfig();

            saveButton.Enabled = false;
        }
        private void Action_HadesPath_BrowseButton(object sender, EventArgs e)
        {
            ConfigManager configManager = ConfigManager.GetInstance();
            string newPath = "";
            try
            {
                newPath = configManager.GetHadesPath(true);
            }
            catch { }
            if (newPath != "")
            {
                hadesPath.SetNewPath(newPath);
                saveButton.Enabled = true;
            }
        }
        private void Action_PythonPath_BrowseButton(object sender, EventArgs e)
        {
            ConfigManager configManager = ConfigManager.GetInstance();
            string newPath = "";
            try
            {
                newPath = configManager.GetPythonPath(true);
            }
            catch { }
            if (newPath != "")
            {
                pythonPath.SetNewPath(newPath);
                saveButton.Enabled = true;
            }
        }
        private void Action_DefaultPath_BrowseButton(object sender, EventArgs e)
        {
            ConfigManager configManager = ConfigManager.GetInstance();
            string newPath = "";
            try
            {
                newPath = configManager.GetDefaultPath(true);
            }
            catch { }
            if (newPath != "")
            {
                defaultPath.SetNewPath(newPath);
                saveButton.Enabled = true;
            }
        }
        private void Action_ResourcesPath_BrowseButton(object sender, EventArgs e)
        {
            ConfigManager configManager = ConfigManager.GetInstance();
            string newPath = "";
            try
            {
                newPath = configManager.GetResourcesPath(true);
            }
            catch { }
            if (newPath != "")
            {
                resourcesPath.SetNewPath(newPath);
                saveButton.Enabled = true;
            }
        }
    }
}
