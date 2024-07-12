using Hades_Map_Editor.Data;
using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IronPython.Modules._ast;
using static System.Net.Mime.MediaTypeNames;

namespace Hades_Map_Editor.PropertiesSection
{
    public class PropertyID : PropertyItem<int>, IComponent
    {
        protected Button idButton;
        protected Label noneLabel;
        public PropertyID(string label) : base(label)
        {
            Initialize();
            Populate();
            //properties = new ThingTextProperties(this, panel);
        }
        public new void Initialize()
        {
            noneLabel = new Label();
            noneLabel.Text = "None";
            noneLabel.Dock = DockStyle.Right;
            Controls.Add(noneLabel);

            idButton = new Button();
            idButton.Dock = DockStyle.Right;
            idButton.Visible = false;
            Controls.Add(idButton);
            //Controls.Add(textBox);

            //BorderStyle = BorderStyle.FixedSingle;
            //SetAutoScrollMargin(0, 0);
        }
        public new void Populate()
        {
            idButton.Click += (s, e) => PropertyID_Click(s, e);
        }
        public override void Update(int value)
        {
            if(value == 0)
            {
                idButton.Visible = false;
                noneLabel.Visible = true;
            }
            else
            {
                noneLabel.Visible=false;
                idButton.Visible = true;
                idButton.Text = value.ToString();
            }
        }
        private void PropertyID_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(idButton.Text);
            FormManager formManager = FormManager.GetInstance();
            Console.WriteLine(id);
            formManager.GetPropertiesPanel().FocusOn(id);
            formManager.GetMapPanel().FocusOn(id);
            formManager.GetElementsPanel().FocusOn(id);
        }
    }
}
