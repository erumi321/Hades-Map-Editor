using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.Components
{
    public class BottomMenuStrip: StatusStrip, IComponent
    {
        public ToolStripMenuItem statutsLabel, hadesVersion;
        public BottomMenuStrip()
        {
            Initialize();
            Populate();
        }
        public void Initialize()
        {
            Dock = DockStyle.Bottom;
            statutsLabel = new System.Windows.Forms.ToolStripMenuItem()
            {
                Name = "Statuts",
                Text = "Idle",
            };
            statutsLabel.Dock = DockStyle.Right;
            hadesVersion = new System.Windows.Forms.ToolStripMenuItem()
            {
                Name = "Version",
                Text = "Hades",
            };
            Items.Add(statutsLabel);
            Items.Add(hadesVersion);
        }

        public void Populate()
        {

        }
        public void SetStatuts(string newStatuts)
        {
            statutsLabel.Text = newStatuts;
        }

    }
}
