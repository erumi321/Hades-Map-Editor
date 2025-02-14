using Hades_Map_Editor.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IronPython.Modules.PythonIterTools;

namespace Hades_Map_Editor.Managers
{
    public sealed class SaveManager
    {
        private static SaveManager _instance;
        private ProjectData _projectData;
        public static SaveManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SaveManager();
            }
            return _instance;
        }
        private SaveManager() { }
        public ProjectData CreateProject(string directoryPath)
        {
            MapData mapData = new MapData();
            MapTextData mapTextData = new MapTextData();
            mapData.Obstacles = new List<Obstacle>();
            ProjectData projectData =  new ProjectData(directoryPath+@"\myProject.hades_map", mapData, mapTextData);
            _projectData = projectData;
            SaveProject(_projectData);

            return projectData;
        }
        public void SaveProject(ProjectData projectData)
        {
            if (projectData.projectPath == "")
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog
                {
                    InitialDirectory = @"C:\Users\Alexandre-i5\source\repos\Hades Map Helper\test_data\sample\",
                    Title = "Browse Map Texts",

                    CheckFileExists = true,
                    CheckPathExists = true,

                    DefaultExt = "hades_map",
                    Filter = "map texts (*.hades_map)|*.hades_map",
                    FilterIndex = 2,
                    RestoreDirectory = true,
                };

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    projectData.projectPath = saveFileDialog1.FileName;
                }
            }
            using (StreamWriter file = new StreamWriter(projectData.projectPath))
            {
                string json = JsonConvert.SerializeObject(projectData);
                file.Write(json);
            }
            ConfigManager config = ConfigManager.GetInstance();
            config.AddProjectPath(projectData.projectPath);
        }
        public ProjectData LoadProject(string path)
        {
            ProjectData projectData;
            if (path == "")
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog
                {
                    InitialDirectory = @"C:\Users\Alexandre-i5\source\repos\Hades Map Helper\test_data\sample\",
                    Title = "Browse Map Texts",

                    CheckFileExists = true,
                    CheckPathExists = true,

                    DefaultExt = "hades_map",
                    Filter = "map texts (*.hades_map)|*.hades_map",
                    FilterIndex = 2,
                    RestoreDirectory = true,

                    ReadOnlyChecked = true,
                    ShowReadOnly = true
                };
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    path = openFileDialog1.FileName;
                }
            }
            if(path == "") // Didn't load
            {
                throw new NoFileLoadedException();
            }
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                projectData = JsonConvert.DeserializeObject<ProjectData>(json);
            }
            LoadAssetsToMapData(projectData.mapData);
            ConfigManager config = ConfigManager.GetInstance();
            config.AddProjectPath(projectData.projectPath);
            _projectData = projectData;
            return projectData;            
        }
        public void ExportMap(MapData mapData)
        {

        }
        public ProjectData ImportMap(string mapPath)
        {
            MapData mapData;
            if (mapPath == "")
            {
                try
                {
                    mapPath = OpenFile("Browse Thing Texts", "thing_text", "thing texts (*.thing_text)|*.thing_text");
                }
                catch (NoFileLoadedException e)
                {
                    throw e;
                }
                         
            }
            using (StreamReader r = new StreamReader(mapPath))
            {
                string json = r.ReadToEnd();
                mapData = JsonConvert.DeserializeObject<MapData>(json);
            }
            LoadAssetsToMapData(mapData);
            string projectPath = Path.ChangeExtension(mapPath, ".hades_map");
            if (_projectData != null)
            {
                _projectData.projectPath = projectPath;
                _projectData.mapData = mapData;
            }
            else
            {
                _projectData = new ProjectData(projectPath, mapData);
            }
            SaveProject(_projectData);
            return _projectData;

        }
        public ProjectData ImportMapText(string mapPath)
        {
            MapTextData mapTextData;
            if (mapPath == "")
            {
                try
                {
                    mapPath = OpenFile("Browse Thing Texts", "map_text", "map texts (*.map_text)|*.map_text");
                }
                catch (NoFileLoadedException e)
                {
                    throw e;
                }

            }
            using (StreamReader r = new StreamReader(mapPath))
            {
                string json = r.ReadToEnd();
                mapTextData = JsonConvert.DeserializeObject<MapTextData>(json);
            }
            string projectPath = Path.ChangeExtension(mapPath, ".hades_map");
            if (_projectData != null)
            {
                _projectData.projectPath = projectPath;
                _projectData.mapTextData = mapTextData;
            }
            else
            {
                _projectData = new ProjectData(projectPath, null, mapTextData);
            }
            SaveProject(_projectData);
            return _projectData;

        }
        private void LoadAssetsToMapData(MapData mapData)
        {
           
            Dictionary<string, SJSONObject> allObstacles = SJSONLoader.LoadAllSJSONObstacles();

            AssetsManager assetsManager = AssetsManager.GetInstance();
            foreach (Obstacle obs in mapData.Obstacles)
            {
                obs.Invisible = false;
                obs.DisplayInEditor = true;
                obs.Offset = new Obstacle.JsonPoint();
                obs.MeshType = "Flat";

                if (allObstacles.ContainsKey(obs.Name))
                {
                    Dictionary<string, SJSONObject> obsData = allObstacles[obs.Name];
                    if (obsData.ContainsKey("Thing"))
                    {
                        Dictionary<string, SJSONObject> thing = obsData["Thing"];
                        if (thing.ContainsKey("Offset"))
                        {
                            Dictionary<string, SJSONObject> offset = thing["Offset"];
                            Obstacle.JsonPoint newPoint = new Obstacle.JsonPoint();
                            newPoint.X = (double)(offset["X"]);
                            newPoint.Y = (double)(offset["Y"]);
                            obs.Offset = newPoint;
                        }
                        if (thing.ContainsKey("Scale"))
                        {
                            obs.Scale *= (double)thing["Scale"];
                        }
                        if (thing.ContainsKey("Invisible") && thing["Invisible"] == true)
                        {
                            obs.Invisible = true;
                        }
                        if (thing.ContainsKey("MeshType"))
                        {
                            obs.MeshType = (string)(thing["MeshType"]);
                        }

                    }
                    
                    if (obsData.ContainsKey("DisplayInEditor") && ((bool)obsData["DisplayInEditor"])==false)
                    {
                        obs.DisplayInEditor = false;
                    }
                }
                Asset asset;
                if (assetsManager.GetAsset(obs.Name, out asset))
                {
                    obs.SetAsset(asset);
                }
            }
        }
        public void SaveAssets(Assets _assets)
        {
            ConfigManager configManager = ConfigManager.GetInstance();
            using (StreamWriter file = new StreamWriter(configManager.GetPath(ConfigType.ResourcesPath) + @"\assets.json"))
            {
                string json = JsonConvert.SerializeObject(_assets);
                file.Write(json);
            }
        }
        public Assets LoadAssets()
        {
            Assets assets;
            ConfigManager configManager = ConfigManager.GetInstance();
            if (File.Exists(configManager.GetPath(ConfigType.ResourcesPath) + @"\assets.json"))
            {
                using (StreamReader r = new StreamReader(configManager.GetPath(ConfigType.ResourcesPath) + @"\assets.json"))
                {
                    string json = r.ReadToEnd();
                    assets = JsonConvert.DeserializeObject<Assets>(json);
                }
                //assets.LoadImages();
            }
            else
            {
                throw new Exception("Missing assets file");
            }
            return assets;
        }
        private string OpenFile(
            string title = "Browse Files",
            string defaultExt = "hades_map",
            string filter = "map texts (*.hades_map)|*.hades_map",
            string initialDirectory = @"C:\Users\Alexandre-i5\source\repos\Hades Map Helper\test_data\sample\"    
            )
        {
            string path = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = initialDirectory,
                Title = title,

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = defaultExt,
                Filter = filter,
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog1.FileName;
            }
            if(path == "")
            {
                throw new NoFileLoadedException();
            }
            else
            {
                return path;
            }
        }
    }
}
