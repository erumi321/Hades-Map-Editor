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
        ConfigurationLink hadesPath, pythonPath, projectPath, resourcesPath;
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
            projectPath = new ConfigurationLink("Default Project Path", saveButton);
            resourcesPath = new ConfigurationLink("Resources Path", saveButton);

            Controls.Add(saveButton);
            Controls.Add(hadesPath);
            Controls.Add(pythonPath);
            Controls.Add(projectPath);
            Controls.Add(resourcesPath);
        }

        public void Populate()
        {
            ConfigManager configManager = ConfigManager.GetInstance();
            hadesPath.SetText(configManager.GetPath(ConfigType.HadesPath));
            pythonPath.SetText(configManager.GetPath(ConfigType.PythonPath));
            projectPath.SetText(configManager.GetPath(ConfigType.ProjectPath));
            resourcesPath.SetText(configManager.GetPath(ConfigType.ResourcesPath));

            saveButton.Click += (s, e) => Action_SaveButton(s, e);
            hadesPath.browseButton.Click += (s, e) => Action_HadesPath_BrowseButton(s, e);
            pythonPath.browseButton.Click += (s, e) => Action_PythonPath_BrowseButton(s, e);
            projectPath.browseButton.Click += (s, e) => Action_ProjectPath_BrowseButton(s, e);
            resourcesPath.browseButton.Click += (s, e) => Action_ResourcesPath_BrowseButton(s, e);
            saveButton.Enabled = false;
        }
        private void Action_SaveButton(object sender, EventArgs e)
        {
            ConfigManager configManager = ConfigManager.GetInstance();
            configManager.ChangeConfig(new Dictionary<ConfigType, string>()
            {
                { ConfigType.HadesPath, hadesPath.GetText() },
                { ConfigType.ProjectPath, projectPath.GetText() },
                { ConfigType.PythonPath, pythonPath.GetText() },
                { ConfigType.ResourcesPath, resourcesPath.GetText() }
            });

            saveButton.Enabled = false;
        }
        private void Action_HadesPath_BrowseButton(object sender, EventArgs e)
        {
            BrowseAction(ConfigType.HadesPath, hadesPath);
        }
        private void Action_PythonPath_BrowseButton(object sender, EventArgs e)
        {
            BrowseAction(ConfigType.PythonPath, pythonPath);
        }
        private void Action_ProjectPath_BrowseButton(object sender, EventArgs e)
        {
            BrowseAction(ConfigType.ProjectPath, projectPath);
        }
        private void Action_ResourcesPath_BrowseButton(object sender, EventArgs e)
        {
            BrowseAction(ConfigType.ResourcesPath, resourcesPath);
        }
        private void BrowseAction(ConfigType type, ConfigurationLink button)
        {
            ConfigManager configManager = ConfigManager.GetInstance();
            string newPath = "";
            try
            {
                newPath = configManager.FetchPathFromType(type);
            }
            catch { }
            if (newPath != "")
            {
                button.SetText(newPath);
                saveButton.Enabled = true;
            }
        }
    }
}
