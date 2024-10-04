using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.TopMenu
{
    public class EditMenuItem : ToolStripMenuItem, IComponent
    {
        public ToolStripMenuItem
            undo, redo, cut, copy, paste, delete;
        public EditMenuItem() : base("Edit")
        {
            Initialize();
            Populate();
            Dock = DockStyle.Top;
        }
        public void Initialize()
        {
            ConfigManager configManager = ConfigManager.GetInstance();
            ((ToolStripDropDownMenu)(DropDown)).ShowImageMargin = true;
            ((ToolStripDropDownMenu)(DropDown)).ShowCheckMargin = false;

            undo = new ToolStripMenuItem("Undo");
            redo = new ToolStripMenuItem("Redo");
            cut = new ToolStripMenuItem("Cut");
            copy = new ToolStripMenuItem("Copy");
            paste = new ToolStripMenuItem("Paste");
            delete = new ToolStripMenuItem("Delete");

            DropDownItems.Add(undo);
            DropDownItems.Add(redo);
            DropDownItems.Add(new ToolStripSeparator());
            DropDownItems.Add(copy);
            DropDownItems.Add(paste);
            DropDownItems.Add(cut);
            DropDownItems.Add(delete);
        }

        public void Populate()
        {
            undo.Click += Undo_Action;
            undo.Enabled = false;
            redo.Click += Redo_Action;
            redo.Enabled = false;
            cut.Click += Cut_Action;
            cut.Enabled = false;
            copy.Click += Copy_Action;
            copy.Enabled = false;
            paste.Click += Paste_Action;
            paste.Enabled = false;
            delete.Click += Delete_Action;
            delete.Enabled = false;

        }

        private void Undo_Action(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void Redo_Action(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void Cut_Action(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void Copy_Action(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void Paste_Action(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void Delete_Action(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}
