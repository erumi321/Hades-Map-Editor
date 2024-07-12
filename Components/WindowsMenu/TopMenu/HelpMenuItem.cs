using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Hades_Map_Editor.Components
{
    public class HelpMenuItem : ToolStripMenuItem, IComponent
    {
        public ToolStripMenuItem
            githubLink, discordLink, aboutItem;
        HadesMapEditor app;
        public HelpMenuItem(HadesMapEditor app) : base("Help")
        {
            this.app = app;
            Initialize();
            Populate();
            Dock = DockStyle.Top;
        }
        public void Initialize()
        {
            ConfigManager configManager = ConfigManager.GetInstance();
            ((ToolStripDropDownMenu)(DropDown)).ShowImageMargin = true;
            ((ToolStripDropDownMenu)(DropDown)).ShowCheckMargin = false;

            githubLink = new ToolStripMenuItem("Github");
            discordLink = new ToolStripMenuItem("Discord");
            aboutItem = new ToolStripMenuItem("About");

            DropDownItems.Add(githubLink);
            DropDownItems.Add(discordLink);
            DropDownItems.Add(new ToolStripSeparator());
            DropDownItems.Add(aboutItem);
        }

        public void Populate()
        {
            githubLink.Click += (s, e) => GithubRedirect_Action(s, e);
            discordLink.Click += (s, e) => DiscordRedirect_Action(s, e);
            aboutItem.Click += (s, e) => About_Action(s, e);
        }

        private void DiscordRedirect_Action(object sender, System.EventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.com/invite/KuMbyrN");
        }
        private void GithubRedirect_Action(object sender, System.EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/AlexKage69/Hades-Map-Editor");
        }
        private void About_Action(object sender, System.EventArgs e)
        {
            MessageBox.Show("Version: 0.03-alpha\n\nCreated by AlexKage", "About");
        }
    }
}
