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
    public class AssetsMenuItem : ToolStripMenuItem, IComponent
    {
        public ToolStripMenuItem
            createElement, createEmptyElement,
            fetch, compile, biomes;
        public AssetsMenuItem() : base("Assets")
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

            fetch = new ToolStripMenuItem("Fetch Assets From Hades");
            compile = new ToolStripMenuItem("Compile Assets For Map");
            biomes = new ToolStripMenuItem("Biomes");

            DropDownItems.Add(fetch);
            DropDownItems.Add(compile);
            DropDownItems.Add(biomes);
            /*foreach (string biome in AssetsManager.GetInstance().Biomes())
            {
                biomes.DropDownItems.Add(new ToolStripMenuItem(biome));
            }*/
        }

        public void Populate()
        {
            DropDownOpening += Self_Open;
            fetch.Click += (s, e) => FetchAssets_Action(s, e);
            compile.Click += (s, e) => CompileResources_Action(s, e);
            if (biomes.DropDownItems.Count > 0)
            {
                foreach (ToolStripMenuItem biome in biomes.DropDownItems)
                {
                    biome.Click += Biome_Action;
                }
            }
            else
            {
                biomes.Enabled = false;
            }
        }

        private void Biome_Action(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FetchAssets_Action(object sender, EventArgs e)
        {
            FormManager formManager = FormManager.GetInstance();
            try
            {
                formManager.StartLoading(new FetchAssetWorker());
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private void CompileResources_Action(object sender, EventArgs e)
        {
            FormManager formManager = FormManager.GetInstance();
            try
            {
                formManager.StartLoading(new CompileAssetWorker());
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
        private void Self_Open(object sender, EventArgs e)
        {
            //Console.WriteLine("Map Clicked");
            //FormManager formManager = FormManager.GetInstance();
            //bool hasMapOpen = formManager.HasTabOpen();
            //createElement.Enabled = hasMapOpen
            //createEmptyElement.Enabled = hasMapOpen;
        }
    }
}
