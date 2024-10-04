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

namespace Hades_Map_Editor.AssetsSection
{
    public class AssetsPanel : SubPanel, IDataFeed, IComponent
    {
        private Label loadingLabel;
        public Dictionary<string, AssetTab> assetsTab;
        public TabControl assetsTabControl;

        public AssetsPanel(ProjectPage projectPage):base(projectPage,"Hades Assets")
        {
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
            loadingLabel.Text = "Loading...";
            loadingLabel.Visible = true;
            AssetsManager assetsManager = AssetsManager.GetInstance();
            string currentbiome = GetData().mapbiome;
            var biomeData = assetsManager.GetBiomeAssets(currentbiome);
            if (biomeData != null && biomeData.assetsData.Count != 0)
            {
                int count = 0;
                foreach (var assets in biomeData.assetsData)
                {
                    if (assets.Key == AssetType.Tilesets)
                    {
                        AssetTab assetTab = new AssetTab(this);
                        assetTab.Text = currentbiome;
                        assetTab.LoadItems(assets.Value.Values.ToList());

                        assetsTabControl.Controls.Add(assetTab);
                        count++;
                    }
                    if (count > 10)
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
        }
        public AssetTab GetCurrentTab()
        {
            return assetsTabControl.SelectedTab as AssetTab;
        }

        public void RefreshData()
        {

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
    }
}
