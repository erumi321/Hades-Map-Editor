//using Hades_Map_Editor.Data;
using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Hades_Map_Editor.PropertiesSection
{
    public class PropertyLocation : PropertyItem<Point>, IComponent
    {
        protected TextBox xTextBox;
        protected TextBox yTextBox;
        bool canEdit;
        public PropertyLocation(PropertiesPanel parent, string label, bool edit = false) : base(parent, label)
        {
            canEdit = edit;
            Initialize();
            Populate();
            //properties = new ThingTextProperties(this, panel);
        }
        public new void Initialize()
        {
            xTextBox = new TextBox();
            xTextBox.Dock = DockStyle.Right;
            xTextBox.Enabled = canEdit;
            Controls.Add(xTextBox);

            yTextBox = new TextBox();
            yTextBox.Dock = DockStyle.Right;
            yTextBox.Enabled = canEdit;
            Controls.Add(yTextBox);
            //Controls.Add(textBox);

            //BorderStyle = BorderStyle.FixedSingle;
            //SetAutoScrollMargin(0, 0);
        }
        public new void Populate()
        {
        }
        public override void Update(Point value)
        {
            xTextBox.Text = string.Format("{0:0}", value.X);
            yTextBox.Text = string.Format("{0:0}", value.Y);
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
                    ForeColor = Color.Red;
                }
                else
                {
                    // If the number is not negative, display it in Black.
                    ForeColor = Color.Black;
                }
            }
            catch
            {
                // If there is an error, display the text using the system colors.
                ForeColor = Color.White;
            }
        }
    }
}
