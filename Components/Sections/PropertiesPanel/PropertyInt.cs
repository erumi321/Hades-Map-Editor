using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.PropertiesSection
{
    public class PropertyInt: PropertyTextbox, IComponent
    {
        Action<int> _onChange;
        public PropertyInt(string label, bool edit = false, Action<int> onChange = null) : base(label)
        {
            Initialize();
            Populate();

            this.Enabled = edit;

            _onChange = onChange;

            //properties = new ThingTextProperties(this, panel);
        }
        public new void Initialize()
        {
        }
        public new void Populate()
        {
        }        
        public void Update(int value)
        {
            base.Update(string.Format("{0:0}", value));

            if (_onChange != null)
            {
                _onChange(value);
            }
        }
    }
}
