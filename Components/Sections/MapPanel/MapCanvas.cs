using Hades_Map_Editor.Data;
using Hades_Map_Editor.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Community.CsharpSqlite.Sqlite3;

namespace Hades_Map_Editor.MapSection
{
    public class MapCanvas : Panel, IComponent
    {
        List<Obstacle> listOfLoadedAssets;
        public Dictionary<string, int> groupOrdering;
        static int X = 200, Y = 200;
        float CurrentScale = 1.0f;
        Obstacle selected;
        Dictionary<int, Tuple<Image, Point, string>> obstacleImages;
        List<int> obstacleOrder;
        Rectangle selectRect;
        PictureBox canvas;
        ContextMenu canvasContextMenu;
        ProjectData data;
        public MapCanvas(ProjectData data)
        {
            obstacleImages = new Dictionary<int, Tuple<Image, Point, string>>();
            obstacleOrder = new List<int>();
            listOfLoadedAssets = new List<Obstacle>();
            Initialize();
            Populate();
            this.data = data;
        }
        public void Initialize()
        {
            UnsetSelect();
            //CurrentScale = 1.0f;
            Name = "Canvas";
            BackColor = System.Drawing.Color.Transparent;
            AutoScroll = true;
            Dock = DockStyle.Fill;
            
            //overlayCanvas = new Panel();
            //overlayCanvas.BackColor = System.Drawing.Color.Transparent;
            //Controls.Add(overlayCanvas);

            canvas = new PictureBox();
            canvas.BackColor = System.Drawing.Color.LightGray;
            canvas.Paint += new PaintEventHandler(MapCanvas_Paint);
            Controls.Add(canvas);
            canvasContextMenu = new ContextMenu();
            canvasContextMenu.MenuItems.Add("Item 1");
            canvasContextMenu.MenuItems.Add("Item 2");
        }
        public void Populate()
        {
            //MouseDown += new System.Windows.Forms.MouseEventHandler(MapCanvas_MouseDown);
            canvas.MouseDown += (s, e) => MapCanvas_MouseDown(s, e);
            canvas.ContextMenu = canvasContextMenu;
        }

        public void AddItem(Obstacle obs)
        {
            Point loc = obs.GetImageLocation();
            listOfLoadedAssets.Add(obs);
        }
        public void SelectItem(Obstacle obstacle)
        {
            selected = obstacle;
        }
        public void MapRefresh()
        {
            obstacleImages.Clear();
            canvas.Size = new Size(7000, 7000);

            if (groupOrdering != null)
            {
                //load the obstacle ids into obstacleOrder so painting works in the correct order
                List<Obstacle> sortedObstacles = listOfLoadedAssets.OrderByDescending(o => -1 * groupOrdering[o.GroupNames[0]])/*.ThenBy(o => o.UseBoundsForSortArea == true? o.Location.Y : 0)*/.ToList();

                List<int> beenAttached = new List<int>();

                List<int> nextIdQueue = new List<int> { sortedObstacles[0].Id };
                while (nextIdQueue.Count > 0 && sortedObstacles.Count > 0)
                {
                    IEnumerable<Obstacle> findResult = sortedObstacles.Where(x => x.Id == nextIdQueue[0]);
                    Obstacle obs = sortedObstacles[0];
                    if (findResult.Count() > 0)
                    {
                        obs = findResult.First();
                    }
                    else if (sortedObstacles.Count == 0)
                    {
                        break;
                    }

                    if (!beenAttached.Contains(obs.AttachToID))
                    {
                        beenAttached.Add(obs.AttachToID);
                        nextIdQueue.Insert(0, obs.AttachToID);
                        continue;
                    }
                    else
                    {
                        beenAttached.Remove(obs.AttachToID);
                    }

                    nextIdQueue.RemoveAt(0);
                    sortedObstacles.Remove(obs);

                    obstacleOrder.Add(obs.Id);

                    if (sortedObstacles.Count > 0)
                    {
                        nextIdQueue.Add(sortedObstacles[0].Id);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            foreach (int id in obstacleOrder)
            {
                LoadObstacle(id);
            }
            


            
            BackColor = System.Drawing.Color.Transparent;
        }

        public void RefreshObstacle(int obstacleId)
        {
            obstacleImages.Remove(obstacleId);
            LoadObstacle(obstacleId);
            Refresh();
        }

        public static Image RotateImage(Image img, float rotationAngle)
        {
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, new Point(0, 0));

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }
        public static Tuple<int, int, int> ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return new Tuple<int,int,int>(v, t, p);
            else if (hi == 1)
                return new Tuple<int,int,int>(q, v, p);
            else if (hi == 2)
                return new Tuple<int,int,int>(p, v, t);
            else if (hi == 3)
                return new Tuple<int,int,int>(p, q, v);
            else if (hi == 4)
                return new Tuple<int,int,int>(t, p, v);
            else
                return new Tuple<int,int,int>(v, p, q);
        }

        private void LoadObstacle(int obstacleId)
        {
            FormManager formManager = FormManager.GetInstance();

            IEnumerable<Obstacle> findResult = listOfLoadedAssets.Where(x => x.Id == obstacleId);
            if (findResult.Count() == 0)
            {
                return;
            }
            Obstacle obs = findResult.First();

            Asset asset;
            formManager.GetBottomMenu().SetStatuts(obs.Name);
            if (!obs.Active || !obs.GetAsset(out asset) || obs.IsHidden())
            {
                return;
            }

            Size size = obs.GetDimension();

            size.Width = (int)(size.Width * CurrentScale);
            size.Height = (int)(size.Height * CurrentScale);

            System.Drawing.Image image = asset.GetImage(size);
            image = RotateImage(image, (float)obs.Angle);

            Bitmap bmp = new Bitmap(image.Width, image.Height);
            Graphics obsGraphic = Graphics.FromImage(bmp);

            //obsGraphic.ScaleTransform(CurrentScale, CurrentScale);
            //Transform colors based on obstacle color
            Color obsColor = obs.Color;
            Tuple<int,int,int> obsHSV = ColorFromHSV(obs.Hue, obs.Saturation, obs.Value);

            ImageAttributes imageAttributes = new ImageAttributes();
            float[][] colorMatrixElements = {
                new float[] {(float)obsColor.R / 255,  0,  0,  0, 0},        // red scaling factor
                new float[] {0,  (float)obsColor.G / 255,  0,  0, 0},        // green scaling factor
                new float[] {0,  0,  (float)obsColor.B / 255,  0, 0},        // blue scaling factor
                new float[] {0,  0,  0,  (float)obsColor.A / 255, 0},        // alpha scaling factor of 1
                new float[] {(float)obsHSV.Item1 / 255, (float)obsHSV.Item2 / 255, (float)obsHSV.Item3 / 255,  0, 1 } // additive factor of HSV shift
            };
            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(colorMatrix,
                                            ColorMatrixFlag.Default,
                                            ColorAdjustType.Bitmap);

            Utility.FlipImage(image, obs.FlipHorizontal ^ obs.Invert, obs.FlipVertical ^ obs.Invert);

            obsGraphic.DrawImage(image, new Rectangle(0, 0, size.Width, size.Height), 0, 0, size.Width, size.Height, GraphicsUnit.Pixel, imageAttributes);


            Point imageLoc = obs.GetImageLocation();
            obstacleImages.Add(obs.Id, new Tuple<Image, Point, string>(bmp, new Point((int)(imageLoc.X), (int)(imageLoc.Y)), obs.GroupNames[0]));

            //obsGraphic.Dispose();
            
        }

        private Rectangle GetObstacleRect(Obstacle obs)
        {
            Asset asset;
            if(obs.GetAsset(out asset))
            {
                Point loc = obs.GetImageLocation();

                int subZeroX = Math.Min((int)(loc.X * CurrentScale), 0);
                int subZeroY = Math.Min((int)(loc.Y * CurrentScale), 0);

                return new Rectangle(Math.Max((int)(loc.X * CurrentScale), 0), Math.Max((int)(loc.Y * CurrentScale), 0), (int)(asset.rect.width * obs.Scale * asset.scaleRatio.x * CurrentScale + subZeroX), (int)(asset.rect.height * obs.Scale * asset.scaleRatio.y * CurrentScale + subZeroY));
            }
            else
            {
                return new Rectangle(-1,-1,-1,-1);
            }
        }
        public Size GetOffset(Point location )
        {
            return new Size((int)(location.X),(int)(location.Y));
        }
        public Size GetMapOffset()
        {
            return canvas.Size;
        }
        private void MapCanvas_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

            // Create a local version of the graphics object for the PictureBox.
            Graphics g = e.Graphics;
            //AssetsManager assetsManager = AssetsManager.GetInstance();

            foreach (int id in obstacleOrder)
            {
                if (obstacleImages.ContainsKey(id))
                {
                    if (data.hiddenGroups.Contains(obstacleImages[id].Item3))
                    {
                        continue;
                    }
                    g.DrawImage(obstacleImages[id].Item1, new Point((int)(obstacleImages[id].Item2.X * CurrentScale), (int)(obstacleImages[id].Item2.Y * CurrentScale)));
                }
            }

            // Create pen.
            Pen magentaPen = new Pen(System.Drawing.Color.Magenta, 5);

            // Draw rectangle to screen.
            if(selectRect.X >= 0)
            {
                e.Graphics.DrawRectangle(magentaPen, selectRect.X, selectRect.Y, selectRect.Width, selectRect.Height);
            }
        }
        public void SetSelect(Obstacle obs)
        {
            Rectangle rect = GetObstacleRect(obs);
            if(rect.X < 0)
            {
                return;
            }
            selectRect = rect;
        }
        public void UnsetSelect()
        {
            selectRect = new Rectangle(-1, -1, -1, -1);
        }
        public bool CanZoomIn()
        {
            return CurrentScale > 0.25f;
        }
        public void ZoomIn()
        {
            if(!CanZoomIn()){
                return;
            }
            switch (CurrentScale)
            {
                case 1.5f:
                    CurrentScale = 1.25f;
                    break;
                case 1.25f:
                    CurrentScale = 1.0f;
                    break;
                case 1.0f:
                    CurrentScale = 0.75f;
                    break;
                case 0.75f:
                    CurrentScale = 0.5f;
                    break;
                case 0.5f:
                    CurrentScale = 0.25f;
                    break;
                default:
                    CurrentScale = 0.25f;
                    break;
            }
            MapRefresh();
            Refresh();
        }
        public bool CanZoomOut()
        {
            return CurrentScale < 1.5;
        }
        public void ZoomOut()
        {
            if (!CanZoomOut())
            {
                return;
            }
            switch (CurrentScale)
            {
                case 1.25f:
                    CurrentScale = 1.5f;
                    break;
                case 1.0f:
                    CurrentScale = 1.25f;
                    break;
                case 0.75f:
                    CurrentScale = 1.0f;
                    break;
                case 0.5f:
                    CurrentScale = 0.75f;
                    break;
                case 0.25f:
                    CurrentScale = 0.5f;
                    break;
                default:
                    CurrentScale = 1.5f;
                    break;
            }
            MapRefresh();
            Refresh();
        }

        public float getCurrentScale()
        {
            return CurrentScale;
        }

        private void MapCanvas_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
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
            foreach (var obstacle in listOfLoadedAssets)
            {
                Asset asset;
                if (obstacle.GetAsset(out asset))
                {
                    //asset.rect.x
                }
            }
            Console.WriteLine("Left:" + point.ToString());
            //AssetsManager assetsManager = AssetsManager.GetInstance();
        }
        private void RightMouseDown(System.Drawing.Point point)
        {
            System.Drawing.Point adjustedPoint = new System.Drawing.Point(point.X - HorizontalScroll.Value, point.Y - VerticalScroll.Value);
            canvasContextMenu.Show(this, adjustedPoint);//places the menu at the pointer position
            Console.WriteLine("Right:" + adjustedPoint.ToString());
            var filteredList = listOfLoadedAssets.Where((Obstacle obs) => {
                return false;
            }).ToList();
        }
        /*private void CanvasContextMenu_MouseClick(object sender, MouseEventArgs me)
        {
            System.Drawing.Point coordinates = me.Location;
            canvasContextMenu.ContextMenu.Show(canvasContextMenu, new System.Drawing.Point(coordinates.X, coordinates.Y));
        }*/
    }
}
