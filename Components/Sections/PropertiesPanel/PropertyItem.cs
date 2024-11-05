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
    public abstract class PropertyItem<K>: IComponent, IPropertyItem
    {
        protected string title;
        protected Label nameLabel;
        protected Panel titlePanel, fieldPanel;
        protected PropertiesPanel Parent;
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
            fieldPanel = new Panel();
            fieldPanel.Parent = Parent;
            fieldPanel.BackColor = System.Drawing.Color.Purple;
            fieldPanel.AutoScroll = false;
            fieldPanel.Dock = DockStyle.Fill;
            fieldPanel.Height = 32;
            //fieldPanel.AutoSize = true;
            //Size = new System.Drawing.Size(1, 24);

            titlePanel = new Panel();
            titlePanel.Parent = Parent;
            titlePanel.BackColor = System.Drawing.Color.Gray;
            titlePanel.AutoScroll = false;
            titlePanel.Dock = DockStyle.Fill;
            titlePanel.Height = 32;
            titlePanel.Width = Parent.Width/2;
            titlePanel.AutoSize = false;
            //Size = new System.Drawing.Size(1, 24);

            nameLabel = new Label();
            nameLabel.Text = title;
            nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            nameLabel.BackColor = System.Drawing.Color.DarkGray;
            nameLabel.AutoSize = false;
            nameLabel.Dock = DockStyle.Fill;
            //nameLabel.Size = new System.Drawing.Size(100,20);
            nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            titlePanel.Controls.Add(nameLabel);
        }
        public void Populate()
        {

        }
        public ProjectPage GetProjectPage() { return Parent.GetProjectPage(); }
        public abstract void Update(K value);

        public Control GetPanel()
        {
            return fieldPanel;
        }

        public Control GetTitle()
        {
            return titlePanel;
        }
    }
}
