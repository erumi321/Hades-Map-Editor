using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.PropertiesSection
{
    public class PropertyCheckbox: PropertyItem<bool>, IComponent
    {
        CheckBox checkBox;
        bool canEdit;
        public PropertyCheckbox(PropertiesPanel parent, string label, bool edit = false) : base(parent, label)
        {
            canEdit = edit;
            Initialize();
            Populate();
            //properties = new ThingTextProperties(this, panel);
        }
        public new void Initialize()
        {
            checkBox = new CheckBox();
            checkBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            checkBox.Enabled = canEdit;
            //checkBox.Dock = DockStyle.Left;
            fieldPanel.Controls.Add(checkBox);
        }
        public new void Populate()
        {
        }
        public override void Update(bool value)
        {
            checkBox.Checked = value;
        }
    }
}
