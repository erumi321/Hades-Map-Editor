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
        Label colorPreview;
        bool canEdit;
        public PropertyColor(string label, bool edit = false) : base(label)
        {
            canEdit = edit;
            Initialize();
            Populate();
            //properties = new ThingTextProperties(this, panel);
        }
        public new void Initialize()
        {
            colorPreview = new Label();
            colorPreview.BackColor = System.Drawing.Color.Transparent;
            Controls.Add(aTextBox);

            bTextBox = new TextBox();
            bTextBox.Dock = DockStyle.Right;
            bTextBox.Width = 32;
            bTextBox.Enabled = canEdit;
            Controls.Add(bTextBox);

            gTextBox = new TextBox();
            gTextBox.Dock = DockStyle.Right;
            gTextBox.Width = 32;
            gTextBox.Enabled = canEdit;
            Controls.Add(gTextBox);

            rTextBox = new TextBox();
            rTextBox.Dock = DockStyle.Right;
            rTextBox.Width = 32;
            rTextBox.Enabled = canEdit;
            Controls.Add(rTextBox);

            aTextBox = new TextBox();
            aTextBox.Dock = DockStyle.Right;
            aTextBox.Width = 32;
            aTextBox.Enabled = canEdit;
            Controls.Add(aTextBox);
        }
        public new void Populate()
        {
        }
        public override void Update(Color value)
        {
            bTextBox.Text = string.Format("{0:0}", value.B);
            gTextBox.Text = string.Format("{0:0}", value.G);
            rTextBox.Text = string.Format("{0:0}", value.R);
            aTextBox.Text = string.Format("{0:0}", value.A);

            BackColor = System.Drawing.Color.FromArgb(value.A, value.B, value.G, value.R);
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
