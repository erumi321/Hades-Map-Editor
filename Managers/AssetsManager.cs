﻿using Hades_Map_Editor.Data;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        private Assets assets;
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
            SaveManager saveManager = SaveManager.GetInstance();
            try
            {
                assets = saveManager.LoadAssets();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                assets = new Assets();
            }
        }
        public Dictionary<string, Dictionary<AssetType, Dictionary<string, Asset>>> GetAssets()
        {
            return assets.biomes;
        }
        public List<string> Biomes()
        {
            return new List<string>(assets.biomes.Keys);
        }
        public async Task CompileAssets()
        {
            processOngoing = true;
            var task = Task.Factory.StartNew(() =>
            {
                FormManager formManager = FormManager.GetInstance();
                formManager.GetBottomMenu().SetStatuts("Compiling assets...");
                Assets assets = new Assets();
                ConfigManager configManager = ConfigManager.GetInstance();
                Console.WriteLine("Start Compilation");
                var directories = Directory.GetDirectories(configManager.GetResourcesPath());
                foreach (var fulldirectory in directories)
                {
                    string directory = Path.GetFileName(fulldirectory);
                    Dictionary<AssetType, Dictionary<string, Asset>> biomeAssets = new Dictionary<AssetType, Dictionary<string, Asset>>();
                    assets.biomes.Add(directory, biomeAssets);
                    var assetFiles = Directory.GetFiles(fulldirectory + @"\manifest");
                    foreach (var assetFile in assetFiles)
                    {
                        using (StreamReader r = new StreamReader(assetFile))
                        {
                            string json = r.ReadToEnd();
                            var asset = JsonConvert.DeserializeObject<RawAtlasJson>(json);
                            asset.AppendAssets(biomeAssets);
                            formManager.GetBottomMenu().SetStatuts(asset.name);
                        }
                    }
                }
                SaveManager saveManager = SaveManager.GetInstance();
                saveManager.SaveAssets(assets);
                this.assets = assets;
                processOngoing = false;
                formManager.GetBottomMenu().SetStatuts("Compiling - done");
            });
            await task;
        }
        public void LoadImages()
        {
            assets.LoadImages();
        }
        public bool GetAsset(string id, out Asset asset)
        {
            foreach(var biome in assets.biomes)
            {
                foreach (var type in biome.Value)
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
        public async void FetchAssetsFromGameAsync()
        {
            try
            {
                processOngoing = true;
                FormManager formManager = FormManager.GetInstance();
                formManager.GetBottomMenu().SetStatuts("Fetch assets...");
                ConfigManager configManager = ConfigManager.GetInstance();
                List<Task> tasks = new List<Task>();
                tasks.Add(RunProcessAsync(configManager.GetPythonPath(), "Tartarus",configManager.GetHadesPath(), configManager.GetResourcesPath()));
                //_ = RunProcessAsync(configManager.GetPythonPath(), "Erebus", configManager.GetHadesPath(), configManager.GetResourcesPath());
                //_ = RunProcessAsync(configManager.GetPythonPath(), "Asphodel", configManager.GetHadesPath(), configManager.GetResourcesPath());
                //_ = RunProcessAsync(configManager.GetPythonPath(), "Elysium", configManager.GetHadesPath(), configManager.GetResourcesPath());
                //_ = RunProcessAsync(configManager.GetPythonPath(), "Surface", configManager.GetHadesPath(), configManager.GetResourcesPath());
                //_ = RunProcessAsync(configManager.GetPythonPath(), "Styx", configManager.GetHadesPath(), configManager.GetResourcesPath());
                
                Task task =  Task.WhenAll(tasks);
                await task.ContinueWith(allTask =>
                {
                    formManager.GetBottomMenu().SetStatuts("Fetch assets - done");
                    Console.WriteLine("Finished!");
                    processOngoing = false;
                });
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }

        }

        
        private async Task<int> RunProcessAsync(string pythonPath, string packageName, string hadesPath, string resourcesPath)
        {
            if (!Directory.Exists(resourcesPath + @"\"+packageName))
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
                tcs.SetResult(process.ExitCode);
                process.Dispose();
            };

            process.Start();

            return await tcs.Task;
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
}
