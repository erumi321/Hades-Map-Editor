using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.Components
{
    public class TopMenuStrip: MenuStrip, IComponent
    {
        public ToolStripMenuItem 
            filesMenu, editMenu, viewMenu, assetMenu, mapMenu, helpMenu;
        HadesMapEditor app;
        public TopMenuStrip(HadesMapEditor app)
        {
            this.app = app;
            Initialize();
            Populate();
            Dock = DockStyle.Top;
        }
        public void Initialize()
        {
            // Create a MenuStrip control with a new window.
            filesMenu = new FilesMenuItem(app);
            editMenu = new EditMenuItem(app);
            viewMenu = new ViewMenuItem(app);
            assetMenu = new AssetsMenuItem(app);
            mapMenu = new MapMenuItem(app);
            helpMenu = new HelpMenuItem(app);

            // Assign the ToolStripMenuItem that displays 
            // the list of child forms.
            MdiWindowListItem = filesMenu;

            // Add the window ToolStripMenuItem to the MenuStrip.
            Items.Add(filesMenu);
            Items.Add(editMenu);
            Items.Add(viewMenu);
            Items.Add(assetMenu);
            Items.Add(mapMenu);
            Items.Add(helpMenu);
        }

        public void Populate()
        {
        }
    }
}
