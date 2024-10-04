using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.TopMenu
{
    public class MapMenuItem : ToolStripMenuItem, IComponent
    {
        public ToolStripMenuItem
            refreshMap, loadMapText, metadataView;
        public MapMenuItem() : base("Map")
        {
            Initialize();
            Populate();
            Dock = DockStyle.Top;
        }
        public void Initialize()
        {
            ConfigManager configManager = ConfigManager.GetInstance();
            ((ToolStripDropDownMenu)(DropDown)).ShowImageMargin = true;
            ((ToolStripDropDownMenu)(DropDown)).ShowCheckMargin = false;

            refreshMap = new ToolStripMenuItem("Refresh Map");
            loadMapText = new ToolStripMenuItem("Load .map_text");
            metadataView = new ToolStripMenuItem("Open Map Metadata");

            DropDownItems.Add(refreshMap);
            DropDownItems.Add(new ToolStripSeparator());
            DropDownItems.Add(loadMapText);
            DropDownItems.Add(metadataView);
        }

        public void Populate()
        {
            DropDownOpening += Self_Open;
            refreshMap.Click += RefreshMap_Action;
            refreshMap.Enabled = false;
            loadMapText.Click += LoadMapText_Action;
            loadMapText.Enabled = false;
            metadataView.Click += MetadataView_Action;
            metadataView.Enabled = false;
        }

        private void MetadataView_Action(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LoadMapText_Action(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void RefreshMap_Action(object sender, EventArgs e)
        {
            FormManager formManager = FormManager.GetInstance();
            var pp = (Parent as TopMenuStrip).GetMainControl().GetCurrentProjectPage();
            pp.assetsPanel.RefreshData();
            pp.elementsPanel.RefreshData();
        }
        private void Self_Open(object sender, EventArgs e)
        {
            Console.WriteLine("Map Clicked");
            FormManager formManager = FormManager.GetInstance();
            bool hasMapOpen = formManager.HasTabOpen();
            refreshMap.Enabled = false;
            //loadMapText.Enabled = hasMapOpen;
            //metadataView.Enabled = hasMapOpen;
        }
    }
}
