//using Hades_Map_Editor.Data;
using Hades_Map_Editor.Managers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Hades_Map_Editor.PropertiesSection
{
    public class PropertyColor : PropertyItem<Color>, IComponent
    {
        protected TextBox bTextBox, gTextBox, rTextBox, aTextBox;
        bool canEdit;
        Action<Color> _onChange;
        public PropertyColor(string label, bool edit = false, Action<Color> onChange = null) : base(label)
        {
            canEdit = edit;
            Initialize();
            Populate();

            _onChange = onChange;

            //properties = new ThingTextProperties(this, panel);
        }
        public new void Initialize()
        {
            rTextBox = new TextBox();
            rTextBox.Size = new Size(30, 40);
            rTextBox.Dock = DockStyle.Right;
            rTextBox.Enabled = canEdit;
            rTextBox.TextChanged += UpdateColor;
            Controls.Add(rTextBox);


            gTextBox = new TextBox();
            gTextBox.Size = new Size(30, 40);
            gTextBox.Dock = DockStyle.Right;
            gTextBox.Enabled = canEdit;
            gTextBox.TextChanged += UpdateColor;
            Controls.Add(gTextBox);


            bTextBox = new TextBox();
            bTextBox.Size = new Size(30, 40);
            bTextBox.Dock = DockStyle.Right;
            bTextBox.Enabled = canEdit;
            bTextBox.TextChanged += UpdateColor;
            Controls.Add(bTextBox);


            aTextBox = new TextBox();
            aTextBox.Size = new Size(30, 40);
            aTextBox.Dock = DockStyle.Right;
            aTextBox.Enabled = canEdit;
            aTextBox.TextChanged += UpdateColor;
            Controls.Add(aTextBox);
        }

        public new void Populate()
        {
        }
        public void UpdateColor(object sender, EventArgs e)
        {
            string a = aTextBox.Text;
            int aN = 0;
            string r = rTextBox.Text;
            int rN = 0;
            string g = gTextBox.Text;
            int gN = 0;
            string b = bTextBox.Text;
            int bN = 0;

            if (int.TryParse(a, out aN) && int.TryParse(r, out rN) && int.TryParse(g, out gN) && int.TryParse(b, out bN))
            {
                Color value = System.Drawing.Color.FromArgb(aN, rN, gN, bN);

                nameLabel.BackColor = value;

                if (_onChange != null)
                {
                    _onChange(value);
                }
            }

           
        }
        public override void Update(Color value)
        {
            bTextBox.Text = string.Format("{0:0}", value.B);
            gTextBox.Text = string.Format("{0:0}", value.G);
            rTextBox.Text = string.Format("{0:0}", value.R);
            aTextBox.Text = string.Format("{0:0}", value.A);

            nameLabel.BackColor = System.Drawing.Color.FromArgb(value.A, value.B, value.G, value.R);

            if (_onChange != null)
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
