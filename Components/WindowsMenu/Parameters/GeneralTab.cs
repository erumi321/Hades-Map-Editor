using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.Components.Dialogs
{
    class GeneralTab : TabPage, IComponent
    {
        public GeneralTab() : base("General")
        {
            Initialize();
            Populate();
        }
        public void Initialize()
        {
            TextBox textbox = new TextBox();
            textbox.Text = "General";
            Controls.Add(textbox);
        }

        public void Populate()
        {

        }
    }
}
