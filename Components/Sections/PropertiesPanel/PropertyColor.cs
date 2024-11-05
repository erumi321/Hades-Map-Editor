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
        public PropertyColor(PropertiesPanel parent, string label, bool edit = false) : base(parent, label)
        {
            canEdit = edit;
            Initialize();
            Populate();
            //properties = new ThingTextProperties(this, panel);
        }
        public new void Initialize()
        {
            bTextBox = new TextBox();
            bTextBox.Dock = DockStyle.Left;
            bTextBox.Width = 32;
            bTextBox.Enabled = canEdit;
            fieldPanel.Controls.Add(bTextBox);

            gTextBox = new TextBox();
            gTextBox.Dock = DockStyle.Left;
            gTextBox.Width = 32;
            gTextBox.Enabled = canEdit;
            fieldPanel.Controls.Add(gTextBox);

            rTextBox = new TextBox();
            rTextBox.Dock = DockStyle.Left;
            rTextBox.Width = 32;
            rTextBox.Enabled = canEdit;
            fieldPanel.Controls.Add(rTextBox);

            aTextBox = new TextBox();
            aTextBox.Dock = DockStyle.Left;
            aTextBox.Width = 32;
            aTextBox.Enabled = canEdit;
            fieldPanel.Controls.Add(aTextBox);
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

            fieldPanel.BackColor = System.Drawing.Color.FromArgb(value.A, value.B, value.G, value.R);
        }
        private void Property_Leave(object sender, EventArgs e)
        {
            Console.WriteLine("1:"+ fieldPanel.Text);
            try
            {
                // Convert the text to a Double and determine if it is a negative number.
                if (double.Parse(fieldPanel.Text) < 0)
                {
                    // If the number is negative, display it in Red.
                    fieldPanel.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    // If the number is not negative, display it in Black.
                    fieldPanel.ForeColor = System.Drawing.Color.Black;
                }
            }
            catch
            {
                // If there is an error, display the text using the system colors.
                fieldPanel.ForeColor = System.Drawing.Color.White;
            }
        }
    }
}
