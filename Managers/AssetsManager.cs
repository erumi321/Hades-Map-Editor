using Hades_Map_Editor.Data;
using Hades_Map_Editor.Properties;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hades_Map_Editor.Managers
{
    public class AssetsManager
    {
        private AssetData assets;
        private static AssetsManager _instance;
        private bool processOngoing;
        public static AssetsManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AssetsManager();
            }
            return _instance;
        }
        private AssetsManager() {
            IOManager saveManager = IOManager.GetInstance();
            try
            {
                assets = saveManager.LoadAssets();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                assets = new AssetData();
            }
        }
        public void SetAssets(AssetData assets)
        {
            this.assets = assets;
        }
        public BiomeAssetData GetBiomeAssets(string selectedBiome)
        {
            if (selectedBiome != null && assets.biomeData.ContainsKey(selectedBiome))
            {
                return assets.biomeData[selectedBiome];
            }
            return null;
        }
        public List<string> Biomes()
        {
            return new List<string>(assets.biomeData.Keys);
        }
        public bool HasLoadedResources()
        {
            ConfigManager configManager = ConfigManager.GetInstance();
            return Directory.GetDirectories(configManager.GetPath(ConfigType.ResourcesPath)).Any();
        }
        public bool HasAssets()
        {
            return assets.biomeData.Count > 0;
        }
        /*public void CompileAssets()
        {
            FormManager formManager = FormManager.GetInstance();
            var task = Task.Factory.StartNew(() =>
            {
                AssetData assets = new AssetData();
                ConfigManager configManager = ConfigManager.GetInstance();
                Console.WriteLine("Start Compilation");
                var directories = Directory.GetDirectories(configManager.GetPath(ConfigType.ResourcesPath));
                foreach (var fulldirectory in directories)
                {
                    string manifestPath = fulldirectory + @"\manifest";
                    if (!Directory.Exists(manifestPath))
                    {
                        continue;
                    }
                    string directory = Path.GetFileName(fulldirectory);
                    assets.biomeData.Add(directory, new BiomeAssetData());
                    var assetFiles = Directory.GetFiles(manifestPath);
                    foreach (var assetFile in assetFiles)
                    {
                        using (StreamReader r = new StreamReader(assetFile))
                        {
                            string json = r.ReadToEnd();
                            var asset = JsonConvert.DeserializeObject<RawAtlasJson>(json);
                            asset.AppendAssets(new BiomeAssetData());
                            formManager.GetBottomMenu().SetStatuts(asset.name);
                        }
                    }
                }
                IOManager saveManager = IOManager.GetInstance();
                saveManager.SaveAssets(assets);
                this.assets = assets;
                formManager.EndLoading();
                formManager.GetBottomMenu().SetStatuts("Compiling - done");
            });
        }*/
        public void LoadImages()
        {
            assets.LoadImages();
        }
        public bool GetAsset(string id, out Asset asset)
        {
            foreach (var biome in assets.biomeData)
            {
                foreach (var type in biome.Value.assetsData)
                {
                    if (type.Value.ContainsKey(id))
                    {
                        asset = type.Value[id];
                        return true;
                    }
                }
            }
            asset = null;
            return false;
        }
        
        /*public List<string> GetAllNames()
        {
            return assets.Where((v)=> { return v.Value.GetAssetType() == MapAssetType.Tileset; }).ToDictionary(i => i.Key, i => i.Value).Keys.ToList().GetRange(0,5);
        }
        public Image GetImage(string subAssetName)
        {
            return assets[subAssetName].GetImage();
        }*/
        /*public Image CreateImage()
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(@"C:\SomeFolder\Images\image.png");
            bitmap.DecodePixelWidth = 100;
            bitmap.EndInit();
            return new Image { Source = bitmap, Margin = new Thickness(0), Stretch = Stretch.None, ClipToBounds = false };
        }*/
    }
    public class FetchAssetWorker : BackgroundWorker
    {
        AssetsManager am;
        int doneProcess, maxProcess;
        public FetchAssetWorker() : base()
        {
            am = AssetsManager.GetInstance();
            DoWork += DoWorkFunc;
            RunWorkerCompleted += RunWorkerCompletedFunc;
        }
        private void DoWorkFunc(object sender, DoWorkEventArgs e)
        {
            FetchAssetsFromGameAsync();
        }
        private void RunWorkerCompletedFunc(object sender, RunWorkerCompletedEventArgs e)
        {
            //FormManager.GetInstance().EndLoading();
        }
        private async void FetchAssetsFromGameAsync()
        {
            try
            {
                doneProcess = 0;
                FormManager formManager = FormManager.GetInstance();
                //formManager.GetBottomMenu().SetStatuts("Fetch assets...");
                ConfigManager configManager = ConfigManager.GetInstance();
                List<string> packages = new List<string>(){"Tartarus","Asphodel"};
                maxProcess = packages.Count;
                List<Task> tasks = new List<Task>();
                foreach (string packageName in packages)
                {
                    tasks.Add(RunProcessAsync(configManager.GetPath(ConfigType.PythonPath), packageName, configManager.GetPath(ConfigType.HadesPath), configManager.GetPath(ConfigType.ResourcesPath)).ContinueWith(taskid => { UpdateDialogMessage(packageName); }));
                }
                //_ = RunProcessAsync(configManager.GetPythonPath(), "Erebus", configManager.GetHadesPath(), configManager.GetResourcesPath());
                //_ = RunProcessAsync(configManager.GetPythonPath(), "Asphodel", configManager.GetHadesPath(), configManager.GetResourcesPath());
                //_ = RunProcessAsync(configManager.GetPythonPath(), "Elysium", configManager.GetHadesPath(), configManager.GetResourcesPath());
                //_ = RunProcessAsync(configManager.GetPythonPath(), "Surface", configManager.GetHadesPath(), configManager.GetResourcesPath());
                //_ = RunProcessAsync(configManager.GetPythonPath(), "Styx", configManager.GetHadesPath(), configManager.GetResourcesPath());

                Task task = Task.WhenAll(tasks);
                await task.ContinueWith(allTask =>
                {
                    formManager.EndLoading();
                });

            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }

        }
        private void UpdateDialogMessage(string packageName)
        {
            doneProcess += 1;
            FormManager formManager = FormManager.GetInstance();
            formManager.ChangeLoadingStatus(packageName, doneProcess, maxProcess);
        }


        private Task<int> RunProcessAsync(string pythonPath, string packageName, string hadesPath, string resourcesPath)
        {
            if (!Directory.Exists(resourcesPath + @"\" + packageName))
            {
                Directory.CreateDirectory(resourcesPath + @"\" + packageName);
            }
            else
            {
                Directory.Delete(resourcesPath + @"\" + packageName, true);
                Directory.CreateDirectory(resourcesPath + @"\" + packageName);
            }
            var tcs = new TaskCompletionSource<int>();
            var process = new Process
            {
                StartInfo = {
                    FileName = pythonPath+@"\python.exe",
                    Arguments = string.Format("{0} {1} {2} {3}", "\"" + System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Python\\extract.py\"", "\""+packageName+"\"", "\""+hadesPath+"\"", "\""+resourcesPath+"\""),
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                },
                EnableRaisingEvents = true
            };
            process.Exited += (sender, args) =>
            {
                process.WaitForExit();
                tcs.SetResult(process.ExitCode);
                process.Dispose();
            };

            process.Start();

            return tcs.Task;
        }
    }
    public class CompileAssetWorker : BackgroundWorker{
        AssetsManager am;
        int doneProcess, maxProcess;
        public CompileAssetWorker() : base()
        {
            am = AssetsManager.GetInstance();
            DoWork += DoWorkFunc;
            RunWorkerCompleted += RunWorkerCompletedFunc;
        }
        private void DoWorkFunc(object sender, DoWorkEventArgs e)
        {
            CompileAsset();
        }
        private void RunWorkerCompletedFunc(object sender, RunWorkerCompletedEventArgs e)
        {
            FormManager formManager = FormManager.GetInstance();
            formManager.EndLoading();
        }
        private void CompileAsset()
        {
            doneProcess = 0;
            maxProcess = 0;
            FormManager formManager = FormManager.GetInstance();
            AssetData assets = new AssetData();
            ConfigManager configManager = ConfigManager.GetInstance();
            var directories = Directory.GetDirectories(configManager.GetPath(ConfigType.ResourcesPath));
            foreach (var fulldirectory in directories)
            {
                string manifestPath = fulldirectory + @"\manifest";
                if (!Directory.Exists(manifestPath))
                {
                    continue;
                }
                string directory = Path.GetFileName(fulldirectory);
                assets.biomeData.Add(directory, new BiomeAssetData());
                var assetFiles = Directory.GetFiles(manifestPath);
                maxProcess += assetFiles.Length;
                foreach (var assetFile in assetFiles)
                {
                    using (StreamReader r = new StreamReader(assetFile))
                    {
                        string json = r.ReadToEnd();
                        var asset = JsonConvert.DeserializeObject<RawAtlasJson>(json);
                        asset.AppendAssets(assets.biomeData[directory]);
                    }
                    UpdateDialogMessage(directory +"["+ Path.GetFileNameWithoutExtension(assetFile).Split('.')[0] + "]");
                }
            }
            IOManager saveManager = IOManager.GetInstance();
            saveManager.SaveAssets(assets);
            am.SetAssets(assets);
        }
        private void UpdateDialogMessage(string packageName)
        {
            doneProcess += 1;
            FormManager formManager = FormManager.GetInstance();
            formManager.ChangeLoadingStatus(packageName, doneProcess, maxProcess);
        }
    }
}
