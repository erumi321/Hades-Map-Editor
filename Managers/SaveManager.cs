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

        public void SetCurrentProject(ProjectData p)
        {
            _projectData = p;
        }

        private SaveManager() { }
        public ProjectData CreateProject(string directoryPath)
        {
            MapThingData mapData = new MapThingData();
            MapTextData mapTextData = new MapTextData();
            mapData.Obstacles = new List<Obstacle>();
            ProjectData projectData =  new ProjectData(directoryPath+@"\myProject.hades_map", mapData, mapTextData);
            _projectData = projectData;
            SaveProject(_projectData);

            return projectData;
        }
        public void SaveProject()
        {
            if (_projectData.projectPath == "")
            {
                _projectData.projectPath = SaveFileDialog();
            }
            using (StreamWriter file = new StreamWriter(_projectData.projectPath))
            {
                string json = JsonConvert.SerializeObject(_projectData);
                file.Write(json);
            }
            ConfigManager config = ConfigManager.GetInstance();
            config.AddProjectPath(_projectData.projectPath);
        }
        public void SaveProject(ProjectData projectData)
        {
            if (projectData.projectPath == "")
            {
                _projectData.projectPath = SaveFileDialog();
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
            ProjectData projectData = null;
            if (path == "")
            {
                path = OpenFileDialog("Browse Project Files", "hades_map", "hades map project (*.hades_map)|*.hades_map");
                if (path == null)
                {
                    return null;
                }
            }


            if (path == "") // Didn't load
            {
                throw new NoFileLoadedException();
            }
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                projectData = JsonConvert.DeserializeObject<ProjectData>(json);
            }
            LoadAssetsToMapData(projectData.mapThingData);
            ConfigManager config = ConfigManager.GetInstance();
            config.AddProjectPath(projectData.projectPath);
            if (_projectData == null)
            {
                _projectData = projectData;
            }
            return projectData;            
        }
        public void ExportMap(string exportPath)
        {
            if (_projectData == null)
            {
                return;
            }
            MapThingData thingData = _projectData.mapThingData;

            if (exportPath == "")
            {
                string temp = SaveFileDialog("Export Map", ".thing_bin", "thing binary (*.thing_bin)|*.thing_bin", "", _projectData.name);
                if (temp == null)
                {
                    return;
                }
                else
                {
                    exportPath = temp;
                }
            }
            using (FileStream r = new FileStream(exportPath, FileMode.OpenOrCreate))
            {
                r.SetLength(0);
                MapThingData.Serialize(r, thingData);
            }
        }
        public ProjectData ImportMapThingBin(string thingPath, string mapPath)
        {

            MapTextData mapTextData;
            if (mapPath == "")
            {
                mapPath = OpenFileDialog("Browse Thing Texts", "map_text", "map texts (*.map_text)|*.map_text");
                
                if (mapPath == null)
                {
                    return _projectData;
                }
            }
            if (thingPath == "")
            {
                thingPath = OpenFileDialog("Browse Thing Bin", "thing_bin", "thing binary (*.thing_bin)|*.thing_bin");

                if (thingPath == null)
                {
                    return _projectData;
                }
            }
            using (StreamReader r = new StreamReader(mapPath))
            {
                string json = r.ReadToEnd();
                mapTextData = JsonConvert.DeserializeObject<MapTextData>(json);
            }
            MapThingData mapThingData;
           
            using (FileStream r = File.OpenRead(thingPath))
            {
                mapThingData = MapThingData.Deserialize(r);
            }
            LoadAssetsToMapData(mapThingData);


            string projectPath = Path.ChangeExtension(thingPath, ".hades_map");

            ProjectData projectData = new ProjectData(projectPath, mapThingData, mapTextData);
            
            ConfigManager config = ConfigManager.GetInstance();
            config.AddProjectPath(projectData.projectPath);
            if (_projectData == null)
            {
                _projectData = projectData;
            }

            SaveProject(projectData);
            return projectData;

        }
        private void LoadAssetsToMapData(MapThingData mapData)
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
                            //obs.Scale *= (double)thing["Scale"];
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
        private static string SaveFileDialog(string title = "Save File",
            string defaultExt = "hades_project",
            string filter = "hades map project (*.hades_project)|*.hades_project",
            string initialDirectory = @"",
            string fileName = "")
        {
            string path = "";
            Microsoft.Win32.SaveFileDialog openFileDialog1 = new Microsoft.Win32.SaveFileDialog
            {
                InitialDirectory = initialDirectory,
                Title = title,

                CheckPathExists = true,
                FileName = fileName,
                DefaultExt = defaultExt,
                Filter = filter,
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (openFileDialog1.ShowDialog() == true)
            {
                path = openFileDialog1.FileName;
            }
            if (path == "")
            {
                return null;
            }
            else
            {
                return path;
            }
        }
        private string OpenFileDialog(
            string title = "Browse Files",
            string defaultExt = "hades_map",
            string filter = "map texts (*.hades_map)|*.hades_map",
            string initialDirectory = @""    
            )
        {
            string path = "";
            Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog
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
            if (openFileDialog1.ShowDialog() == true)
            {
                path = openFileDialog1.FileName;
            }
            if(path == "")
            {
                return null;
            }
            else
            {
                return path;
            }
        }
    }
}
