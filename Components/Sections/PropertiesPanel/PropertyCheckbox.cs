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
        private Action<bool> _onChange;
        public PropertyCheckbox(string label, bool edit = false, Action<bool> onChange = null) : base(label)
        {
            canEdit = edit;
            Initialize();
            Populate();

            _onChange = onChange;

            //properties = new ThingTextProperties(this, panel);
        }
        public new void Initialize()
        {
            checkBox = new CheckBox();
            checkBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            checkBox.Enabled = canEdit;
            checkBox.Click += UpdateCheck;
            //checkBox.Dock = DockStyle.Left;
            Controls.Add(checkBox);
        }
        public void UpdateCheck(object sender, EventArgs e)
        {
            if (_onChange != null)
            {
                _onChange(checkBox.Checked);
            }
        }
        public new void Populate()
        {
        }
        public override void Update(bool value)
        {
            checkBox.Checked = value;

            if(_onChange != null)
            {
                _onChange(value);
            }
        }
    }
}
