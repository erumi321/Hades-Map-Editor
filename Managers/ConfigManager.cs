using Hades_Map_Editor.Data;
using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;

namespace Hades_Map_Editor.Managers
{
    public sealed class ConfigManager
    {
        private static ConfigManager _instance;
        private string configPath;
        public Dictionary<ConfigType, ConfigData> configs;
        List<ProjectPath> projectList;
        public static ConfigManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ConfigManager();
            }
            return _instance;
        }
        private ConfigManager() {
            configPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\config.json";
            LoadConfig();
        }
        private void LoadConfig(bool reset = false)
        {
            ConfigFile configFile;
            bool newConfig;
            if (!File.Exists(configPath) || reset)
            {
                newConfig = true;
                configFile = new ConfigFile();
            }
            else
            {
                newConfig = false;
                string text = File.ReadAllText(configPath);
                configFile = JsonConvert.DeserializeObject<ConfigFile>(text);
            }
            CreateConfig(configFile);
            if (newConfig)
            {
                SaveConfig();
            }
        }
        private void CreateConfig(ConfigFile configFile)
        {
            configs = new Dictionary<ConfigType, ConfigData>();
            string defaultFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            bool hasHadesPath = configFile.TryGetValue(ConfigType.HadesPath, out var hadesPath);
            configs.Add(ConfigType.HadesPath, new ConfigData(ConfigType.HadesPath, defaultFolder, @"Select your Hades Game Folder, I.e: C:\Hades", (hasHadesPath) ? hadesPath : ""));
            bool hasResourcesPath = configFile.TryGetValue(ConfigType.ResourcesPath, out var resourcesPath);
            configs.Add(ConfigType.ResourcesPath, new ConfigData(ConfigType.ResourcesPath, defaultFolder, @"Select your Temporary Resources Folder, I.e: C:\...\Documents\Resources", (hasResourcesPath) ? resourcesPath : ""));
            bool hasProjectPath = configFile.TryGetValue(ConfigType.ProjectPath, out var projectPath);
            configs.Add(ConfigType.ProjectPath, new ConfigData(ConfigType.ProjectPath, defaultFolder, @"Select your Default Projects Folder, I.e: C:\...\Documents\Projects", (hasProjectPath) ? projectPath : ""));
            bool hasPythonPath = configFile.TryGetValue(ConfigType.PythonPath, out var pythonPath);
            configs.Add(ConfigType.PythonPath, new ConfigData(ConfigType.PythonPath, defaultFolder, @"Select your Python Executable Folder, I.e: C:\...\AppData\Local\Microsoft\WindowsApps", (hasPythonPath) ? pythonPath : ""));
            projectList = configFile.projectList;
        }
        public void ChangeConfig(Dictionary<ConfigType, string> configFile)
        {
            foreach (var config in configFile)
            {
                configs[config.Key].SetPath(config.Value);
            }
            SaveConfig();
        }
        private void SaveConfig()
        {
            ConfigFile content = new ConfigFile(configs, projectList);
            using (StreamWriter file = new StreamWriter(configPath))
            {
                string json = JsonConvert.SerializeObject(content);
                file.Write(json);
            }
        }
        public bool HasPath(ConfigType type)
        {
            return configs[type].HasPath();
        }
        public string GetPath(ConfigType type)
        {
            return configs[type].GetPath();
        }
        public string FetchPath(string description, string selectedPath)
        {
            string newPath = "";
            FolderBrowserDialog openFileDialog1 = new FolderBrowserDialog { };
            openFileDialog1.SelectedPath = (selectedPath == "") ? configs[ConfigType.ProjectPath].GetPath() : selectedPath;
            openFileDialog1.Description = (description == "") ? "Get a Folder Directory": description;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                newPath = openFileDialog1.SelectedPath;
                if (newPath == "") // Didn't load
                {
                    throw new NoFileLoadedException();
                }
            }
            return newPath;
        }
        public string FetchPathFromType(ConfigType type)
        {
            string newPath = "";
            FolderBrowserDialog openFileDialog1 = new FolderBrowserDialog { };
            if (configs.ContainsKey(type))
            {
                if (configs.ContainsKey(type) && configs[type].HasPath())
                {
                    openFileDialog1.SelectedPath = configs[type].GetPath();
                }
                else
                {
                    openFileDialog1.Description = configs[type].GetDefaultMessage();
                }
            }
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                newPath = openFileDialog1.SelectedPath;
                if (newPath == "") // Didn't load
                {
                    throw new NoFileLoadedException();
                }
            }
            return newPath;
        }
        public void AddProjectPath(string path)
        {
            if(projectList.Find(v => { return v.GetPath() == path; }) == null)
            {
                projectList.Add(new ProjectPath(path));
                SaveConfig();
            }
        }
        public void RemoveProjectPath(string path)
        {
            int count = projectList.RemoveAll(v => { return v.GetPath() == path; });
            if (count > 0)
            {
                SaveConfig();
            } 
        }
        public List<string> GetAllProjectPath()
        {
            List<string> result = new List<string>();
            List<ProjectPath> toremove = new List<ProjectPath>();
            foreach (var project in projectList.Where(v => { return v.isOpen; }))
            {
                if (!File.Exists(project.GetPath()))
                {
                    toremove.Add(project);
                }
                else
                {
                    result.Add(project.GetPath());
                }
            }
            foreach (var project in toremove)
            {
                projectList.Remove(project);
            }
            return result;
        }
    }
    public class ProjectPath
    {
        public bool isOpen;
        public string path;
        public bool HasPath()
        {
            return Directory.Exists(path) && path != "";
        }
        public string GetPath()
        {
            return path;
        }
        public ProjectPath(string path)
        {
            this.path = path;
            isOpen = true;
        }
    }
    public class ConfigData
    {
        ConfigType type;
        string path, defaultPath, defaultMessage;
        public string GetDefaultMessage()
        {
            return defaultMessage;
        }
        public void SetPath(string path)
        {
            this.path = path;
        }
        public bool HasPath()
        {
            return Directory.Exists(path) && path != "";
        }
        public string GetPath()
        {
            if (!HasPath())
            {
                return defaultPath;
            }
            else
            {
                return path;
            }
        }
        public ConfigData(ConfigType type, string defaultPath, string defaultMessage, string path = "")
        {
            this.type = type;
            this.defaultPath = defaultPath;
            this.defaultMessage = defaultMessage;
            this.path = path;
        }
    }
    public class ConfigFile
    {
        public List<ProjectPath> projectList;
        public Dictionary<ConfigType, string> configPaths;
        public ConfigFile()
        {
            configPaths = new Dictionary<ConfigType, string>();
            projectList = new List<ProjectPath>();
        }
        public ConfigFile(Dictionary<ConfigType, ConfigData> configData, List<ProjectPath> projectPaths)
        {
            configPaths = new Dictionary<ConfigType, string>();
            foreach (var config in configData)
            {
                if (config.Value.HasPath())
                {
                    configPaths.Add(config.Key, config.Value.GetPath());
                }
            }
            projectList = projectPaths;
        }
        public bool TryGetValue(ConfigType type, out string val)
        {
            return configPaths.TryGetValue(type, out val);
        }
    }
    public enum ConfigType
    {
        HadesPath,
        ResourcesPath,
        PythonPath,
        ProjectPath,
    }
}
