using Hades_Map_Editor.Data;
using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.AssetsSection
{
    public class AssetItem : Panel, IComponent
    {
        PictureBox picture;
        Label name;
        public bool isSelected = false;
        ContextMenu assetContextMenu;
        MenuItem create, replace;
        public AssetItem(AssetsPanel assetPanel)
        {
            Parent = assetPanel;
            Initialize();
            Populate();
            //properties = new ThingTextProperties(this, panel);
        }
        public void Initialize()
        {
            name = new Label();
            name.Dock = DockStyle.Bottom;

            //name.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Regular);
            name.AutoSize = true;
            name.BackColor = Color.AntiqueWhite;
            picture = new PictureBox();
            picture.SizeMode = PictureBoxSizeMode.AutoSize;
            BorderStyle = BorderStyle.FixedSingle;

            assetContextMenu = new ContextMenu();
            create = assetContextMenu.MenuItems.Add("Create Element");
            replace = assetContextMenu.MenuItems.Add("Replace Selected Element");

            Controls.Add(name);
            Controls.Add(picture);
        }
        public void Populate()
        {
            picture.Click += (s, e) => AssetPanel_Click(s, e);
            name.Click += (s, e) => AssetPanel_Click(s, e);
            picture.Paint += (s, e) => AssetPanel_Paint(s, e);
            name.Paint += (s, e) => AssetPanel_Paint(s, e);
            
            picture.ContextMenu = assetContextMenu;

            create.Click += (s, e) => AssetContextMenu_Click(s, e);
        }
        public void SetImage(string name, Image image)
        {
            this.name.Text = name;
            picture.Image = image;
            picture.Refresh();
        }

        private void AssetPanel_Click(object sender, EventArgs e)
        {
        }
        private void AssetContextMenu_Click(object sender, EventArgs e)
        {
            FormManager formManager = FormManager.GetInstance();
        }
        public void Unselect()
        {
            isSelected = false;
            Refresh();
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
        private void AssetPanel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            System.Drawing.Point point = new System.Drawing.Point(e.X, e.Y);
            switch (e.Button)
            {
                case MouseButtons.Left:
                    {
                        LeftMouseDown(point);
                    }
                    break;
                case MouseButtons.Right:
                    {
                        RightMouseDown(point);
                    }
                    break;
            }

        }
        private void LeftMouseDown(System.Drawing.Point point)
        {
            isSelected = true;
            /*ProjectPage pp = ((AssetsPanel)Parent).GetProjectPage();
            pp.assetsPanel.GetCurrentTab().SelectAsset(this);
            Debug.WriteLine(name.Text);
            Refresh();
            Console.WriteLine("Left:" + point.ToString());*/
            //AssetsManager assetsManager = AssetsManager.GetInstance();
        }
        private void RightMouseDown(System.Drawing.Point point)
        {
            System.Drawing.Point adjustedPoint = new System.Drawing.Point(point.X - HorizontalScroll.Value, point.Y - VerticalScroll.Value);
            assetContextMenu.Show(this, adjustedPoint);//places the menu at the pointer position
            Console.WriteLine("Right:" + adjustedPoint.ToString());
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
