using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.PropertiesSection
{
    public class PropertyDouble : PropertyTextbox, IComponent
    {
        private Action<double> _onChange;
        public PropertyDouble(string label, bool edit = false, Action<double> onChange = null) : base(label, edit, null)
        {
            this.Enabled = edit;

            Initialize();
            Populate();

            this._onChange = onChange;

            //properties = new ThingTextProperties(this, panel);
        }
        public new void Initialize()
        {
            this.textBox.TextChanged += UpdateDouble;
        }
        public new void Populate()
        {
        }

        public void UpdateDouble(object sender, EventArgs e)
        {
            double dV = 0;
            if (_onChange != null && double.TryParse(this.textBox.Text, out dV))
            {
                _onChange(dV);
            }
        }

        public void Update(double value)
        {
            base.Update(string.Format("{0:0.00}", value));

            if (_onChange != null)
            {
                _onChange(value);
            }
        }
    }
}
