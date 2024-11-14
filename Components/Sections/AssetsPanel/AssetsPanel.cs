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
        EmptyAssetPanel emptyAssetPanel;
        AssetMenu assetMenu;
        AssetTable assetTable;
        PagingComponent paging;
        private int maxRow = 5, maxColumn = 7, wsize = 100, hsize = 120;

        public AssetsPanel(ProjectPage projectPage):base(projectPage,"Hades Assets")
        {
            Initialize();
            Populate();
        }
        public void Initialize()
        {
            
            assetMenu = new AssetMenu(this);
            assetTable = new AssetTable(this);
            emptyAssetPanel = new EmptyAssetPanel(this);
            paging = new PagingComponent(assetTable);

            BottomPanel.Controls.Add(paging);
            TopPanel.Controls.Add(assetMenu);
            ContentPanel.Controls.Add(emptyAssetPanel);
            ContentPanel.Controls.Add(assetTable);
        }
        public void Populate()
        {
            RefreshData();
            closeButton.Click += CloseButton_Click;
            assetTable.Visible = false;

            ShowAssetTable(assetMenu.selectBiomeDropdown.Items[0].ToString());
        }

        public void RefreshData()
        {
            AssetsManager am = AssetsManager.GetInstance();
            if (am.HasAssets())
            {
                emptyAssetPanel.Visible = false;
            }
            else
            {
                emptyAssetPanel.Visible = true;
            }
            assetMenu.RefreshData();
        }
        private void CloseButton_Click(object sender, EventArgs e)
        {
            parent.ToggleAssetsPanel();
        }
        public void ShowAssetTable(string biomeName)
        {
            if(assetMenu.selectBiomeDropdown.SelectedItem == null || assetMenu.selectBiomeDropdown.SelectedItem.ToString() != biomeName)
            {
                assetMenu.selectBiomeDropdown.SelectedItem = biomeName;
            }
            AssetsManager am = AssetsManager.GetInstance();
            List<Asset> list = am.GetBiomeAssets(biomeName).GetAssetsByType(AssetType.Tilesets);
            paging.SetupPaging((int)Math.Ceiling((double)list.Count / (maxRow * maxColumn)));
            assetTable.LoadItems(list);
            assetTable.Visible = true;
            emptyAssetPanel.Visible = false;
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
