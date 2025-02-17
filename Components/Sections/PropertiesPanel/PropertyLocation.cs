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
    public class PropertyLocation : PropertyItem<PointF>, IComponent
    {
        protected TextBox xTextBox;
        protected TextBox yTextBox;
        bool canEdit;

        Action<PointF> _onChange;
        public PropertyLocation(string label, bool edit = false, Action<PointF> onChange = null) : base(label)
        {
            canEdit = edit;
            Initialize();
            Populate();

            _onChange = onChange;

            //properties = new ThingTextProperties(this, panel);
        }
        public new void Initialize()
        {
            xTextBox = new TextBox();
            xTextBox.Dock = DockStyle.Right;
            xTextBox.Enabled = canEdit;
            xTextBox.Size = new Size(80, 40);
            xTextBox.KeyUp += UpdateLocation;
            Controls.Add(xTextBox);

            yTextBox = new TextBox();
            yTextBox.Dock = DockStyle.Right;
            yTextBox.Enabled = canEdit;
            yTextBox.Size = new Size(80, 40);
            yTextBox.KeyUp += UpdateLocation;
            Controls.Add(yTextBox);
            //Controls.Add(textBox);

            //BorderStyle = BorderStyle.FixedSingle;
            //SetAutoScrollMargin(0, 0);
        }
        public new void Populate()
        {
        }

        public void UpdateLocation(object sender, EventArgs e)
        {
            string x = xTextBox.Text;
            int xN = 0;
            string y = yTextBox.Text;
            int yN = 0;

            if (int.TryParse(x, out xN) && int.TryParse(y, out yN))
            {
                Point value = new System.Drawing.Point(xN, yN);

                if (_onChange != null)
                {
                    _onChange(value);
                }
            }


        }

        public override void Update(PointF value)
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
