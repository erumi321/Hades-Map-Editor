using Hades_Map_Editor.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Hades_Map_Editor
{
    public class SubPanel : Panel, IComponent
    {
        string windowTitle;
        ProjectPage parent;
        public GroupBox ContentPanel;
        public Panel TopPanel, BottomPanel;
        Label title;
        Button closeButton;

        public SubPanel(ProjectPage pp, string windowTitle)
        {
            parent = pp;
            this.windowTitle = windowTitle;
            Initialize();
            Populate();
        }
        public void Initialize()
        {
            Dock = DockStyle.Fill;
            ContentPanel = new GroupBox();
            ContentPanel.BackColor = Color.LightGray;
            ContentPanel.Dock = DockStyle.Fill;
            TopPanel = new Panel();
            TopPanel.BackColor = Color.LightGray;
            TopPanel.Dock = DockStyle.Top;
            TopPanel.AutoSize = true;
            //TopPanel.AutoSize = true;
            BottomPanel = new Panel();
            BottomPanel.BackColor = Color.LightGray;
            BottomPanel.Dock = DockStyle.Bottom;
            BottomPanel.AutoSize = true;
            title = new Label();
            title.Text = windowTitle;
            title.AutoSize = true;
            title.TextAlign = ContentAlignment.MiddleLeft;
            closeButton = new FlatButton();
            closeButton.Image = Image.FromFile(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + @"\Res\save_button.png");
            //closeButton.AutoSize = false;
            closeButton.Dock = DockStyle.Right;

            Controls.Add(ContentPanel);
            Controls.Add(TopPanel);
            Controls.Add(BottomPanel);
            TopPanel.Controls.Add(closeButton);
            TopPanel.Controls.Add(title);
        }
        public void Populate() { }
        public ProjectPage GetProjectPage()
        {
            return parent;
        }
        public ProjectData GetData()
        {
            return GetProjectPage().GetData();
        }
    }
}
