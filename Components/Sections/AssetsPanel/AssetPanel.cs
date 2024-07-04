using Hades_Map_Editor.Data;
using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.AssetsSection
{
    public class AssetPanel : Panel, IComponent
    {
        Panel margin;
        PictureBox picture;
        Label name;
        public bool isSelected = false;
        public AssetPanel()
        {
            Initialize();
            Populate();
            //properties = new ThingTextProperties(this, panel);
        }
        public void Initialize()
        {
            margin = new Panel();
            margin.Location = new System.Drawing.Point(3, 3);
            margin.Size = new Size(ClientSize.Width-6, ClientSize.Height-6);
            name = new Label();
            name.Dock = DockStyle.Bottom;
            picture = new PictureBox();
            picture.SizeMode = PictureBoxSizeMode.AutoSize;
            BorderStyle = BorderStyle.FixedSingle;

            margin.Controls.Add(name);
            margin.Controls.Add(picture);
            Controls.Add(margin);
        }
        public void Populate()
        {
            Click += (s, e) => AssetPanel_Click(s, e);
            Paint += (s, e) => AssetPanel_Paint(s, e);
        }
        public void SetImage(string name, Image image)
        {
            this.name.Text = name;
            picture.Image = image;
            picture.Refresh();
        }

        private void AssetPanel_Click(object sender, EventArgs e)
        {
            isSelected = true;
            Refresh();
            Console.WriteLine("Clicked");
        }
        private void AssetPanel_Paint(object sender, PaintEventArgs e)
        {
            if (isSelected)
            {
                int thickness = 2;//it's up to you
                int halfThickness = thickness / 2;
                using (Pen p = new Pen(System.Drawing.Color.Black, thickness))
                {
                    e.Graphics.DrawRectangle(p, new Rectangle(halfThickness,
                                                              halfThickness,
                                                              ClientSize.Width - thickness,
                                                              ClientSize.Height - thickness));
                }
            }
        }
            /*public void Select(int selectedId)
            {
                properties.Load(elementManager.GetElement(selectedId).info);
                this.selectedId = selectedId;
            }*/
            /*public void CheckActive(bool value)
            {
                if(selectedId != 0)
                {
                    elementManager.GetElement(selectedId).info.Active = value;
                }
            }*/
        }
}
