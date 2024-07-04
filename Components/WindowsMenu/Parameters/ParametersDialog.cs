using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.Components.Dialogs
{
    public class ParametersDialog : Form
    {
        ParametersTab tabs;
        public ParametersDialog()
        {
            Initialize();
            Populate();
        }
        public void Initialize()
        {
            //FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Dock = DockStyle.Fill;

            tabs = new ParametersTab();
            Controls.Add(tabs);
        }

        public void Populate()
        {

        }

        public void OpenDialog()
        {
            ShowDialog();
        }
    }
}
