using Hades_Map_Editor.Data;
using Hades_Map_Editor.Managers;
using Hades_Map_Editor.MapSection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hades_Map_Editor.Sections
{
    public class MapPanel : Panel, IComponent, Focusable
    {
        private MapCanvas canvas;
        private ProjectData data;
        private MenuStrip topMenu;
        private ToolStripMenuItem zoomIn, zoomOut;
        string selectGroup = "";
        public MapPanel(ProjectData data)
        {
            this.data = data;
            Initialize();
            Populate();
        }

        public MapCanvas getCanvas()
        {
            return canvas;
        }

        public void Initialize()
        {
            BackColor = System.Drawing.Color.BlueViolet;
            AutoScroll = false;
            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.FixedSingle;
            SetAutoScrollMargin(0, 50);

            zoomOut = new ToolStripMenuItem();
            zoomOut.Text = "+";
            zoomOut.Click += (s, e) => MapPanel_ZoomOut_Click(s, e);

            zoomIn = new ToolStripMenuItem();
            zoomIn.Text = "-";
            zoomIn.Click += (s, e) => MapPanel_ZoomIn_Click(s, e);

            topMenu = new MenuStrip();
            topMenu.Dock = DockStyle.Top;
            topMenu.Items.Add(zoomIn);
            topMenu.Items.Add(zoomOut);
            Controls.Add(topMenu);

            canvas = new MapCanvas(data);
            canvas.ZoomIn();
            canvas.ZoomIn();
            canvas.ZoomIn();
            canvas.ZoomIn();
            canvas.ZoomIn();
            canvas.ZoomIn();
            Controls.Add(canvas);

            /*canvas.MouseClick += new MouseEventHandler((o, e) => {
                MouseEventArgs me = (MouseEventArgs)e;
                System.Drawing.Point coordinates = me.Location;
                var list = mapManager.GetElementsAt(coordinates);
                if (list.Count > 0)
                {
                    //app.Select(list.First().info.Id, true);
                }
                else
                {
                    //app.Unload();
                }
            });*/
            //RefreshMap();
            /*canvasSelector = new PictureBox();
            canvasSelector.BorderStyle = BorderStyle.FixedSingle;
            canvasSelector.ClientSize = new Size(20, 20);
            canvasSelector.Visible = true;
            Controls.Add(canvasSelector);*/
            /*foreach (int id in elementManager.GetAllIds())
            {
                var info = elementManager.GetElement(id);               
                Graphics g = e.Graphics;

                var controls = this.ParentPictureBox.Controls.OfType<PictureBox>();
                foreach (var c in controls)
                {
                    g.DrawImage(c.Image, c.Location.X, c.Location.Y, c.Width, c.Height);
                }
            }*/
        }

        public void Populate()
        {
            RefreshData();
        }
        public void UnFocus()
        {
        }
        public void FocusOn(int id)
        {
            Obstacle obs = data.mapThingData.GetFromId(id);


            Size offset = canvas.GetOffset(obs.GetImageLocation());
            //Size mapOffset = canvas.GetMapOffset();

            if (obs.HasAsset())
            {
                //subtracting by 2040 centers the item within the middle of the screen, without the very top-left of the image is at (0,0) on the viewport
                canvas.VerticalScroll.Value = (int)(Math.Max(Math.Min(offset.Height - 2040, canvas.VerticalScroll.Maximum), 0) * canvas.getCurrentScale());
                canvas.HorizontalScroll.Value = (int)(Math.Max(Math.Min(offset.Width - 2040, canvas.HorizontalScroll.Maximum), 0) * canvas.getCurrentScale());
                canvas.SetSelect(obs);
                selectGroup = obs.GroupNames[0];
                //PerformLayout();
            }
            else
            {
                canvas.UnsetSelect();
            }
            canvas.Refresh();
            canvas.PerformLayout();
        }
        public void TryFocusOff(string groupName)
        {
            if (groupName == "" || groupName == selectGroup)
            {
                canvas.UnsetSelect();
                canvas.Refresh();
            }
        }
        public void FocusOff()
        {
            canvas.UnsetSelect();
            canvas.Refresh();
        }

        public void GetData()
        {
            AssetsManager assetsManager = AssetsManager.GetInstance();
            foreach (Obstacle obs in data.mapThingData.Obstacles)
            {
                Asset asset;
                if (assetsManager.GetAsset(obs.Name, out asset))
                {
                    canvas.AddItem(obs);
                }
            }
            if (data.mapTextData != null && data.mapTextData.ThingGroups != null)
            {
                Dictionary<string, int> groupOrdering = new Dictionary<string, int>();
                int i = 0;
                foreach (ThingGroup group in data.mapTextData.ThingGroups)
                {
                    groupOrdering.Add(group.Id.Name, i);
                    i++;
                }

                canvas.groupOrdering = groupOrdering;
            }

        }

        public void RefreshData()
        {
            GetData();
            canvas.MapRefresh();
        }

        public void RefreshObstacle(int id)
        {
            canvas.RefreshObstacle(id);
        }
        // Actions
        private void MapPanel_ZoomIn_Click(object sender, EventArgs e)
        {
            canvas.ZoomIn();
            zoomIn.Enabled = canvas.CanZoomIn();
            zoomOut.Enabled = canvas.CanZoomOut();
            Console.WriteLine("Zoom In Clicked");
        }
        private void MapPanel_ZoomOut_Click(object sender, EventArgs e)
        {
            canvas.ZoomOut();
            zoomIn.Enabled = canvas.CanZoomIn();
            zoomOut.Enabled = canvas.CanZoomOut();
            Console.WriteLine("Zoom Out Clicked");
        }
    }
}
