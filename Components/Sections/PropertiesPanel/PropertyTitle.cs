using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.PropertiesSection
{
    public class PropertyTitle : PropertyItem<string>, IComponent
    {
        public PropertyTitle(PropertiesPanel parent, string label) : base(parent, label)
        {
            Initialize();
            Populate();
            //properties = new ThingTextProperties(this, panel);
        }
        public new void Initialize()
        {
            //nameLabel.Left = 50;
            nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

        }
        public new void Populate()
        {
        }
        public override void Update(string value)
        {
            title = value;
            nameLabel.Text = value;
        }
    }
}
