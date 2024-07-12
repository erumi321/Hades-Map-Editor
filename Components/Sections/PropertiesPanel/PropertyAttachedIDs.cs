using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IronPython.Modules._ast;

namespace Hades_Map_Editor.PropertiesSection
{
    public class PropertyAttachedIDs : PropertyItem<int[]>, IComponent
    {
        protected List<Button> listButton;
        protected Label noneLabel;
        public PropertyAttachedIDs(string label) : base(label)
        {
            Initialize();
            Populate();
            //properties = new ThingTextProperties(this, panel);
        }
        public new void Initialize()
        {
            listButton = new List<Button>();
            noneLabel = new Label();
            noneLabel.Text = "None";
            noneLabel.Dock = DockStyle.Right;
            Controls.Add(noneLabel);
        }
        public new void Populate()
        {
        }
        public override void Update(int[] value)
        {
            foreach (var button in listButton)
            {
                button.Visible = false;
            }
            int index = 0;
            if(value.Length == 0)
            {
                noneLabel.Visible = true;
            }
            foreach (var v in value)
            {
                noneLabel.Visible = false;
                if (listButton.Count <= index)
                {
                    var newButton = new Button();
                    newButton.Dock = DockStyle.Right;
                    newButton.Visible = true;
                    newButton.Text = v.ToString();
                    newButton.Click += (s, e) => PropertyIDs_Click(s, e);
                    Controls.Add(newButton);
                    listButton.Add(newButton);
                }
                else
                {
                    var button = listButton[index];
                    button.Visible = true;
                    button.Text = v.ToString();
                }
                index++;
            }
        }
        private void PropertyIDs_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int id = Int32.Parse(button.Text);
            FormManager formManager = FormManager.GetInstance();
            Console.WriteLine(id);
            formManager.GetPropertiesPanel().FocusOn(id);
            formManager.GetMapPanel().FocusOn(id);
            formManager.GetElementsPanel().FocusOn(id);
        }
    }
}
