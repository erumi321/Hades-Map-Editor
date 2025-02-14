using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.PropertiesSection
{
    public class PropertyTextbox : PropertyItem<string>, IComponent
    {
        protected TextBox textBox;
        protected bool canEdit;

        Action<string> _onChange;
        public PropertyTextbox(string label, bool edit = false, Action<string> onChange = null) : base(label)
        {
            canEdit = edit;
            Initialize();
            Populate();
            _onChange = onChange;
            //properties = new ThingTextProperties(this, panel);
        }
        public new void Initialize()
        {

            textBox = new TextBox();
            textBox.Dock = DockStyle.Right; 
            textBox.Enabled = canEdit;
            textBox.TextChanged += UpdateText;
            Leave += new EventHandler(Property_Leave);
            Controls.Add(textBox);
            //Controls.Add(textBox);

            //BorderStyle = BorderStyle.FixedSingle;
            //SetAutoScrollMargin(0, 0);
        }
        public new void Populate()
        {
        }
        public void UpdateText(object sender, EventArgs e)
        {
            if(_onChange != null)
            {
                _onChange(textBox.Text);
            }
        }
        public override void Update(string value)
        {
            textBox.Text = value;

            if(_onChange != null)
            {
                _onChange(value);
            }
        }
        private void Property_Leave(object sender, EventArgs e)
        {
            Console.WriteLine("1:"+Text);
            try
            {
                // Convert the text to a Double and determine if it is a negative number.
                if (double.Parse(Text) < 0)
                {
                    // If the number is negative, display it in Red.
                    ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    // If the number is not negative, display it in Black.
                    ForeColor = System.Drawing.Color.Black;
                }
            }
            catch
            {
                // If there is an error, display the text using the system colors.
                ForeColor = System.Drawing.Color.White;
            }
        }
    }
}
