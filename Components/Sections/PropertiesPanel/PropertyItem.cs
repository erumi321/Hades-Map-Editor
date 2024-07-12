using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.PropertiesSection
{
    public abstract class PropertyItem<K>: FlowLayoutPanel, IComponent
    {
        protected string title;
        protected Label nameLabel;
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
            FlowDirection = FlowDirection.LeftToRight;

            nameLabel = new Label();
            nameLabel.Text = title;
            nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            nameLabel.BackColor = System.Drawing.Color.BlueViolet;
            //nameLabel.Size = new System.Drawing.Size(100,20);
            nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Controls.Add(nameLabel);
        }
        public void Populate()
        {

        }
        public abstract void Update(K value);
    }
}
