﻿using Hades_Map_Editor.Data;
using Hades_Map_Editor.ElementsSection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.Sections
{
    public class ElementsPanel : Panel, IComponent, IDataFeed
    {
        ProjectData data;
        public ElementsList listBox;
        public Dictionary<int, int> listBoxIndex;
        public ElementsPanel(ProjectData projectData)
        {
            data = projectData;
            Initialize();
            Populate();
        }
        public void Initialize()
        {
            listBoxIndex = new Dictionary<int, int>();
            Dock = DockStyle.Fill;
            listBox = new ElementsList(data);
            
            Controls.Add(listBox);
        }

        public void Populate()
        {
            _ = GetData();
        }

        public async Task GetData()
        {
            await Task.Factory.StartNew(() =>
            {
                //WRITING A FILE OR SOME SUCH THINGAMAGIG
                listBox.BeginUpdate();
                foreach (var obstacle in data.mapData.GetActiveObstacles())
                {
                    listBox.listBoxIndex.Add(listBox.Items.Add(obstacle.Id + ":" + obstacle.Name), obstacle);
                }
                listBox.EndUpdate();
                Console.WriteLine("Loaded Elements");
            });
        }

        public void RefreshData()
        {
            listBox.Items.Clear();
            _ = GetData();
        }
    }
}
