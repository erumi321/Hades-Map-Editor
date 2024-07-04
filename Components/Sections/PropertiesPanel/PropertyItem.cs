using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.PropertiesSection
{
    public abstract class PropertyItem<K>: TableLayoutPanel, IComponent
    {
        string title;
        Label nameLabel;
        protected SplitContainer split;
        public PropertyItem(string label)
        {
            title = label;
            Initialize();
            Populate();
            //Children needs to call Initialize() and Populate();
        }
        public void Initialize()
        {
            BackColor = System.Drawing.Color.Gray;
            AutoScroll = false;
            Dock = DockStyle.Fill;
            Size = new System.Drawing.Size(1, 24);

            split = new SplitContainer();
            split.Dock = DockStyle.Fill;
            //split.Panel1MinSize = 30;
            //split.Panel2MinSize = 30;
            split.SplitterDistance = 70;
            Controls.Add(split);

            nameLabel = new Label();
            nameLabel.Text = title;
            nameLabel.AutoSize = true;
            nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            nameLabel.BackColor = System.Drawing.Color.BlueViolet;
            //nameLabel.Size = new System.Drawing.Size(100,20);
            nameLabel.Dock = DockStyle.Left;
            nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            split.Panel1.Controls.Add(nameLabel);
        }
        public void Populate()
        {

        }
        public abstract void Update(K value);
    }
}
