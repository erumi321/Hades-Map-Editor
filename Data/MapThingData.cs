using Hades_Map_Editor.Managers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IronPython.Modules._ast;

namespace Hades_Map_Editor.Data
{
    public class MapThingData
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
        private static string[] DataTypes = { "Text", "Obstacle", "Unit", "Prefab", "Weapon", "Unknown", "Projectile", "Count", "Animation", "Component" };
        private static string ReadDataType(BinaryReader stream)
        {
            uint val = ReadUInt32(stream);
            val &= 15U;

            return DataTypes[val];
        }
        private static bool? ReadTriBoolean(BinaryReader stream)
        {
            uint val = ReadUInt32(stream);
            val &= 1U;

            if (val == 0)
            {
                return true;
            }
            else if (val == 1)
            {
                return false;
            }

            return null;
        }
        private static string ReadStringAllowNull(BinaryReader stream)
        {
            bool doString = ReadBoolean(stream);

            if(doString)
            {
                return ReadString(stream);
            }
            else
            {
                return null;
            }
        }
        private static string ReadString(BinaryReader stream)
        {
            int len = ReadInt32(stream);

            return new string(stream.ReadChars(len));
        }
        private static Color ReadColor(BinaryReader stream)
        {
            int b = stream.ReadByte();
            int g = stream.ReadByte();
            int r = stream.ReadByte();
            int a = stream.ReadByte();

            //abgr order
            return Color.FromArgb(a, r, g, b);
        }
        private static float ReadSingle(BinaryReader stream)
        {
            return stream.ReadSingle();
        }
        private static bool ReadBoolean(BinaryReader stream)
        {
            return stream.ReadBoolean();
        }

        private static uint ReadUInt32(BinaryReader stream)
        {
            return stream.ReadUInt32();
        }
        private static int ReadInt32(BinaryReader stream)
        {
            return stream.ReadInt32();
        }

        public static MapThingData Deserialize(FileStream file)
        {
            BinaryReader stream = new BinaryReader(file);

            MapThingData data = new MapThingData();
            data.Obstacles = new List<Obstacle>();

            ReadInt32(stream); //SGB1
            ReadInt32(stream); //Version 12

            int obsCount = ReadInt32(stream);

            for(int i = 0; i < obsCount; i++)
            {
                ReadBoolean(stream); //Do create flag, should always be true

                Obstacle obstacle = new Obstacle();

                //TODO: Hades 2

                obstacle.ActivateAtRange = ReadBoolean(stream);
                obstacle.ActivationRange = ReadSingle(stream);

                obstacle.Active = ReadBoolean(stream);
                obstacle.AllowMovementReaction = ReadBoolean(stream);
                obstacle.Ambient = ReadSingle(stream);
                obstacle.Angle = ReadSingle(stream);

                obstacle.AttachedIDs = new List<int>();
                int attachedCount = ReadInt32(stream);
                for(int j = 0; j < attachedCount; j++)
                {
                    obstacle.AttachedIDs.Add(ReadInt32(stream));
                }

                obstacle.AttachToID = ReadInt32(stream);
                obstacle.CausesOcculsion = ReadBoolean(stream);
                obstacle.Clutter = ReadBoolean(stream);
                obstacle.Collision = ReadBoolean(stream);
                obstacle.Color = ReadColor(stream);
                obstacle.Comments = ReadStringAllowNull(stream);
                obstacle.CreatesShadows = ReadTriBoolean(stream);
                obstacle.DataType = ReadDataType(stream);
                obstacle.DrawVfxOnTop = ReadTriBoolean(stream);
                obstacle.FlipHorizontal = ReadBoolean(stream);
                obstacle.FlipVertical = ReadBoolean(stream);

                obstacle.GroupNames = new List<string>();
                int groupLen = ReadInt32(stream);
                for(int j =0; j < groupLen; j++)
                {
                    //TODO: HAdes 2
                    ReadSingle(stream);
                    obstacle.GroupNames.Add(ReadStringAllowNull(stream));
                }

                obstacle.HelpTextID = ReadStringAllowNull(stream);
                obstacle.Hue = ReadSingle(stream);
                obstacle.Saturation = ReadSingle(stream);
                obstacle.Value = ReadSingle(stream);
                obstacle.Id = ReadInt32(stream);
                obstacle.IgnoreGridManager = ReadBoolean(stream);
                obstacle.Invert = ReadBoolean(stream);

                obstacle.Location = new PointF(ReadSingle(stream), ReadSingle(stream)); ;

                obstacle.Name = ReadStringAllowNull(stream);
                obstacle.OffsetZ = ReadSingle(stream);
                obstacle.ParallaxAmount = ReadSingle(stream);

                obstacle.Points = new List<PointF>();
                int pointLen = ReadInt32(stream);
                for(int j = 0; j < pointLen; j++)
                {
                    obstacle.Points.Add(new PointF(ReadSingle(stream), ReadSingle(stream)));
                }

                obstacle.Scale = ReadSingle(stream);
                obstacle.SkewAngle = ReadSingle(stream);
                obstacle.SkewScale = ReadSingle(stream);
                obstacle.SortIndex = ReadInt32(stream);
                obstacle.StopsLight = ReadTriBoolean(stream);
                obstacle.Tallness = ReadSingle(stream);
                obstacle.UseBoundsForSortArea = ReadTriBoolean(stream);

                data.Obstacles.Add(obstacle);
            }

            return data;

        }
        private static void WriteDataType(BinaryWriter stream, string v)
        {
            for(int i =0; i < DataTypes.Count(); i++)
            {
                if (v == DataTypes[i])
                {
                    WriteInt32(stream, i);
                    break;
                }
            }
        }
        private static void WriteTriBoolean(BinaryWriter stream, bool? v)
        {
            if (v == null)
            {
                WriteInt32(stream, 1);
            }else if (v == true)
            {
                WriteInt32(stream, 0);
            }
            else if (v == false)
            {
                WriteInt32(stream, 2);
            }
        } 
        private static void WriteString(BinaryWriter stream, string v)
        {
            WriteInt32(stream, v.Length);

            foreach(char c in v)
            {
                stream.Write((byte)c);
            }
        }
        private static void WriteStringAllowNull(BinaryWriter stream, string v)
        {
            if (v == null)
            {
                stream.Write((byte)0);
            }
            else
            {
                stream.Write((byte)1);
                WriteString(stream, v);
            }

        } 
        private static void WriteColor(BinaryWriter stream, Color v)
        {
            stream.Write((byte)v.B);
            stream.Write((byte)v.G);
            stream.Write((byte)v.R);
            stream.Write((byte)v.A);
        }

        private static void WriteSingle(BinaryWriter stream, double v)
        {
            stream.Write(BitConverter.GetBytes((float)v));
        }
        private static void WriteBoolean(BinaryWriter stream, bool v)
        {
            stream.Write(BitConverter.GetBytes(v));
        }

        private static void WriteInt32(BinaryWriter stream, int v)
        {
            stream.Write(BitConverter.GetBytes(v));
        }
 
        public static void Serialize(Stream file, MapThingData data)
        {
            BinaryWriter stream = new BinaryWriter(file, Encoding.UTF8);

            WriteInt32(stream, 826427219); //SGB1
            WriteInt32(stream, 12); //Version 12

            WriteInt32(stream, data.Obstacles.Count);


            foreach (Obstacle obstacle in data.Obstacles)
            {
                WriteBoolean(stream, true);

                WriteBoolean(stream, obstacle.ActivateAtRange);
                WriteSingle(stream, obstacle.ActivationRange);

                WriteBoolean(stream, obstacle.Active);
                WriteBoolean(stream, obstacle.AllowMovementReaction);
                WriteSingle(stream, obstacle.Ambient);
                WriteSingle(stream, obstacle.Angle);

                WriteInt32(stream, obstacle.AttachedIDs.Count);
                for (int j = 0; j < obstacle.AttachedIDs.Count; j++)
                {
                    WriteInt32(stream, obstacle.AttachedIDs[j]);
                }

                WriteInt32(stream, obstacle.AttachToID);
                WriteBoolean(stream, obstacle.CausesOcculsion);
                WriteBoolean(stream, obstacle.Clutter);
                WriteBoolean(stream, obstacle.Collision);
                WriteColor(stream, obstacle.Color);
                WriteStringAllowNull(stream, obstacle.Comments);
                WriteTriBoolean(stream, obstacle.CreatesShadows);
                WriteDataType(stream, obstacle.DataType);
                WriteTriBoolean(stream, obstacle.DrawVfxOnTop);
                WriteBoolean(stream, obstacle.FlipHorizontal);
                WriteBoolean(stream, obstacle.FlipVertical);

                WriteInt32(stream, obstacle.GroupNames.Count);
                for (int j = 0; j < obstacle.GroupNames.Count; j++)
                {
                    //TODO: HAdes 2
                    WriteSingle(stream, 0);
                    WriteStringAllowNull(stream, obstacle.GroupNames[j]);
                }

                WriteStringAllowNull(stream, obstacle.HelpTextID);
                WriteSingle(stream, obstacle.Hue);
                WriteSingle(stream, obstacle.Saturation);
                WriteSingle(stream, obstacle.Value);
                WriteInt32(stream, obstacle.Id);
                WriteBoolean(stream, obstacle.IgnoreGridManager);
                WriteBoolean(stream, obstacle.Invert);

                WriteSingle(stream, obstacle.Location.X);
                WriteSingle(stream, obstacle.Location.Y);

                WriteStringAllowNull(stream, obstacle.Name);
                WriteSingle(stream, obstacle.OffsetZ);
                WriteSingle(stream, obstacle.ParallaxAmount);

                WriteInt32(stream, obstacle.Points.Count);
                for (int j = 0; j < obstacle.Points.Count; j++)
                {
                    WriteSingle(stream, obstacle.Points[j].X);
                    WriteSingle(stream, obstacle.Points[j].Y);
                }

                WriteSingle(stream, obstacle.Scale);
                WriteSingle(stream, obstacle.SkewAngle);
                WriteSingle(stream, obstacle.SkewScale);
                WriteInt32(stream, obstacle.SortIndex);
                WriteTriBoolean(stream, obstacle.StopsLight);
                WriteSingle(stream, obstacle.Tallness);
                WriteTriBoolean(stream, obstacle.UseBoundsForSortArea);
            }
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
        public Color Color { get; set; }
        public string Comments { get; set; }
        public bool? CreatesShadows { get; set; }
        public string DataType { get; set; }
        public bool? DrawVfxOnTop { get; set; }
        public bool FlipHorizontal { get; set; }
        public bool FlipVertical { get; set; }
        public List<string> GroupNames { get; set; }
        public string HelpTextID { get; set; }
        public double Hue { get; set; }
        public int Id { get; set; }
        public bool IgnoreGridManager { get; set; }
        public bool Invert { get; set; }
        public PointF Location { get; set; }
        public string Name { get; set; }
        public double OffsetZ { get; set; }
        public double ParallaxAmount { get; set; }
        public List<PointF> Points { get; set; }
        public double Saturation { get; set; }
        public double Scale { get; set; }
        public double SkewAngle { get; set; }
        public double SkewScale { get; set; }
        public int SortIndex { get; set; }
        public bool? StopsLight { get; set; }
        public double Tallness { get; set; }
        public bool? UseBoundsForSortArea { get; set; }
        public double Value { get; set; }
        private bool hidden;
        private Asset asset;
        private Size dimension;
        private Color pColor;
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
