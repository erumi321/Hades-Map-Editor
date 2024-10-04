using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Hades_Map_Editor.PropertiesSection
{
    public abstract class PropertyItem<K>: Panel, IComponent
    {
        protected string title;
        protected Label nameLabel;
        public PropertyItem(PropertiesPanel propertiesPanel, string label)
        {
            Parent = propertiesPanel;
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
            AutoSize = true;
            //Size = new System.Drawing.Size(1, 24);

            nameLabel = new Label();
            nameLabel.Text = title;
            nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            nameLabel.BackColor = System.Drawing.Color.BlueViolet;
            //nameLabel.AutoSize = true;
            nameLabel.Dock = DockStyle.Fill;
            //nameLabel.Size = new System.Drawing.Size(100,20);
            nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Controls.Add(nameLabel);
        }
        public void Populate()
        {

        }
        public ProjectPage GetProjectPage() { return ((SubPanel)Parent).GetProjectPage(); }
        public abstract void Update(K value);
    }
}
