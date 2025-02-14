﻿using Hades_Map_Editor.Managers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IronPython.Modules._ast;

namespace Hades_Map_Editor.Data
{
    public class MapData
    {
        public List<Obstacle> Obstacles { get; set; }

        public List<Obstacle> GetActiveObstacles()
        {
            return Obstacles.FindAll((obs) => { return obs.Active; });
        }
        public Obstacle GetFromId(int id)
        {
            return Obstacles.Find((Obstacle val1) => { return val1.Id == id; });
        }
    }
    public class Obstacle
    {
        public bool ActivateAtRange { get; set; }
        public double ActivationRange { get; set; }
        public bool Active { get; set; }
        public bool AllowMovementReaction { get; set; }
        public double Ambient { get; set; }
        public double Angle { get; set; }
        public int AttachToID { get; set; }
        public List<int> AttachedIDs { get; set; }
        public bool CausesOcculsion { get; set; }
        public bool Clutter { get; set; }
        public bool Collision { get; set; }
        public JsonColor Color 
        {
            get { return new JsonColor { A = pColor.A, R = pColor.R, G = pColor.G, B = pColor.B }; }
            set
            {
                pColor = System.Drawing.Color.FromArgb(value.A, value.R, value.G, value.B);
            }
        }
        public object Comments { get; set; }
        public bool? CreatesShadows { get; set; }
        public string DataType { get; set; }
        public bool DrawVfxOnTop { get; set; }
        public bool FlipHorizontal { get; set; }
        public bool FlipVertical { get; set; }
        public List<string> GroupNames { get; set; }
        public object HelpTextID { get; set; }
        public double Hue { get; set; }
        public int Id { get; set; }
        public bool IgnoreGridManager { get; set; }
        public bool Invert { get; set; }
        public JsonPoint Location 
        {
            get { return new JsonPoint {X = pLocation.X, Y = pLocation.Y }; }
            set
            {
                pLocation = new Point((int)value.X, (int)value.Y);
            }
        }
        public string Name { get; set; }
        public double OffsetZ { get; set; }
        public double ParallaxAmount { get; set; }
        public List<object> Points { get; set; }
        public double Saturation { get; set; }
        public double Scale { get; set; }
        public double SkewAngle { get; set; }
        public double SkewScale { get; set; }
        public int SortIndex { get; set; }
        public bool? StopsLight { get; set; }
        public double Tallness { get; set; }
        public bool UseBoundsForSortArea { get; set; }
        public double Value { get; set; }
        private bool hidden;
        private Asset asset;
        private Size dimension;
        private Color pColor;
        private Point pLocation;
        public int GetLayerLevel()
        {
            return SortIndex;
        }
        public bool GetAsset(out Asset asset)
        {
            asset = this.asset;
            return HasAsset();
        }
        public bool HasAsset()
        {
            return asset != null;
        }
        public void SetAsset(Asset asset)
        {
            this.asset = asset;
            dimension = new Size((int)(asset.rect.width * Scale * asset.scaleRatio.x), (int)(asset.rect.height * Scale * asset.scaleRatio.y));
        }
        public bool IsHidden()
        {
            return hidden || !DisplayInEditor || Invisible;
        }
        public Size GetDimension()
        {
            if (dimension == null)
            {
                return new Size();
            }
            return dimension;
        }
        public Color GetColor()
        {
            return pColor;
        }
        public Point GetLocation()
        {
            return pLocation;
        }
        public Point GetImageLocation()
        {
            Size size = GetDimension();

            double zLocation = OffsetZ;

            double xPos = Location.X;
            double yPos = Location.Y;

            double drawScale = Scale;

            yPos -= zLocation * Math.Sin(1.0471975803375244) * ParallaxAmount;

            double parallaxX = drawScale * Offset.X * ParallaxAmount;
            double parallaxY = drawScale * Offset.Y * ParallaxAmount;

            xPos += parallaxX;
            yPos += parallaxY;

            int halfSizeX = (int)(Math.Ceiling((double)((float)size.Width)) / 2);
            int halfSizeY = (int)(Math.Ceiling((double)((float)size.Height)) / 2);

            xPos -= halfSizeX;
            yPos -= halfSizeY;

            return new Point((int)xPos, (int)yPos);
        }
        public void Hide(bool value)
        {
            hidden = value;
        }
        public struct JsonColor
        {
            public int A { get; set; }
            public int B { get; set; }
            public int G { get; set; }
            public int R { get; set; }
        }

        public struct JsonPoint
        {
            public double X { get; set; }
            public double Y { get; set; }
        }
        
        /* Non-JSON fields, such as those loaded in by SJSON */
        public bool DisplayInEditor { get; set; }
        public bool Invisible { get; set; }
        public JsonPoint Offset { get; set; }
        public string MeshType { get; set; }
        
    }
}
