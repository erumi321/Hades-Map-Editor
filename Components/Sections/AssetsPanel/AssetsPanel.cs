using Hades_Map_Editor.AssetsSection;
using Hades_Map_Editor.Data;
using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.Sections
{
    public class AssetsPanel : Panel, IComponent
    {
        private Label loadingLabel;
        public Dictionary<string, AssetTab> assetsTab;
        public TabControl assetsTabControl;
        private ProjectData data;

        public AssetsPanel(ProjectData data)
        {
            this.data = data;
            Initialize();
            Populate();
        }
        public void Initialize()
        {
            BackColor = System.Drawing.Color.White;
            AutoScroll = true;
            Dock = DockStyle.Fill;

            loadingLabel = new Label();
            loadingLabel.Text = "Loading...";
            loadingLabel.AutoSize = false;
            loadingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            loadingLabel.Dock = DockStyle.Fill;
            loadingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Controls.Add(loadingLabel);

            assetsTabControl = new TabControl();
            assetsTabControl.Dock = DockStyle.Fill;
            Controls.Add(assetsTabControl);
            
            //BorderStyle = BorderStyle.FixedSingle;
            //SetAutoScrollMargin(0, 0);
        }
        public void Populate()
        {
            GetData();
        }
        public AssetTab GetCurrentTab()
        {
            return assetsTabControl.SelectedTab as AssetTab;
        }
        /*public void Select(int selectedId)
        {
            properties.Load(elementManager.GetElement(selectedId).info);
            this.selectedId = selectedId;
        }*/
        /*public void CheckActive(bool value)
        {
            if(selectedId != 0)
            {
                elementManager.GetElement(selectedId).info.Active = value;
            }
        }*/
        public void GetData()
        {
            //await Task.Factory.StartNew(() =>
            //{
                loadingLabel.Text = "Loading...";
                loadingLabel.Visible = true;
                //WRITING A FILE OR SOME SUCH THINGAMAGIG
                Console.WriteLine("Start Assets");
                AssetsManager assetsManager = AssetsManager.GetInstance();
                Dictionary<string, Dictionary<AssetType, Dictionary<string, Asset>>> biomes = assetsManager.GetAssets();
                if (biomes != null && biomes.Count != 0)
                {
                    int count = 0;
                    foreach (var biome in biomes)
                    {
                        if (biome.Value.ContainsKey(AssetType.Tilesets))
                        {
                            AssetTab assetTab = new AssetTab();
                            assetTab.Text = biome.Key;
                            assetTab.LoadItems(biome.Value[AssetType.Tilesets].Values.ToList());

                            assetsTabControl.Controls.Add(assetTab);
                            count++;
                        }
                        if(count > 10)
                        {
                            break;
                        }
                    }
                    loadingLabel.Visible = false;
                }
                else
                {
                    loadingLabel.Text = "No Assets detected. \n Try compiling.";
                    loadingLabel.Visible = true;
                }
                Console.WriteLine("Loaded Assets");
            //});
        }

        public void RefreshData()
        {
            assetsTabControl.TabPages.Clear();
            GetData();
        }
    }
}
