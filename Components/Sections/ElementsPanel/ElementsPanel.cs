using Hades_Map_Editor.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.ElementsSection
{
    public class ElementsPanel : SubPanel, IComponent, IDataFeed, Focusable
    {
        public ElementsList listBox;
        public Dictionary<int, int> listBoxIndex;
        public Panel headerPanel;
        public ElementsPanel(ProjectPage parent): base(parent,"Map Elements")
        {
            Parent = parent;
            Initialize();
            Populate();
        }
        public void Initialize()
        {
            listBoxIndex = new Dictionary<int, int>();
            listBox = new ElementsList(this);

            headerPanel = new Panel();
            headerPanel.Size = new Size(10, 10);
            headerPanel.BackColor = Color.LightSeaGreen;

            TopPanel.Controls.Add(headerPanel);
            ContentPanel.Controls.Add(listBox);
        }

        public void Populate()
        {
            var data = GetData();
            //WRITING A FILE OR SOME SUCH THINGAMAGIG
            listBox.BeginUpdate();
                    foreach (var obstacle in data.mapData.GetAllObstacles())
                    {
                listBox.listBoxIndex.Add(listBox.Items.Add(obstacle.Id + ":" + obstacle.Name), obstacle);
                    }
            listBox.EndUpdate();
        }

        public void UnFocus()
        {
            listBox.UnFocus();
        }
        public void FocusOn(int id)
        {
            listBox.FocusOn(id);
        }

        public void RefreshData()
        {
            listBox.Items.Clear();
            //_ = GetData();
        }
    }
}
