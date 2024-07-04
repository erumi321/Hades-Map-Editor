using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.Components.Dialogs
{
    class ParametersTab : TabControl, IComponent
    {
        public TabPage generalTab, interfaceTab, pathsTab, informationsTab;
        public ParametersTab()
        {
            Initialize();
            Populate();
        }
        public void Initialize()
        {
            Alignment = TabAlignment.Left;
            SizeMode = TabSizeMode.Fixed;
            ItemSize = new System.Drawing.Size(25,100);
            Dock = DockStyle.Fill;
            DrawMode = TabDrawMode.OwnerDrawFixed;
            DrawItem += new DrawItemEventHandler(DrawEvent_DrawItem);

            generalTab = new GeneralTab();
            TabPages.Add(generalTab);
            interfaceTab = new TabPage("Interface");
            TabPages.Add(interfaceTab);
            pathsTab = new PathsTab();
            TabPages.Add(pathsTab);
            informationsTab = new TabPage("Informations");
            TabPages.Add(informationsTab);
        }

        public void Populate()
        {

        }

        private void DrawEvent_DrawItem(Object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush _textBrush;

            // Get the item from the collection.
            TabPage _tabPage = TabPages[e.Index];

            // Get the real bounds for the tab rectangle.
            Rectangle _tabBounds = GetTabRect(e.Index);

            if (e.State == DrawItemState.Selected)
            {

                // Draw a different background color, and don't paint a focus rectangle.
                _textBrush = new SolidBrush(Color.Red);
                g.FillRectangle(Brushes.Gray, e.Bounds);
            }
            else
            {
                _textBrush = new System.Drawing.SolidBrush(e.ForeColor);
                e.DrawBackground();
            }

            // Use our own font.
            Font _tabFont = new Font("Arial", 10.0f, FontStyle.Bold, GraphicsUnit.Pixel);

            // Draw string. Center the text.
            StringFormat _stringFlags = new StringFormat();
            _stringFlags.Alignment = StringAlignment.Center;
            _stringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(_tabPage.Text, _tabFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));
        }
    }
}
