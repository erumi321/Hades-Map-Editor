using Hades_Map_Editor.Managers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using static Hades_Map_Editor.Data.Obstacle;
using static IronPython.Modules._ast;

namespace Hades_Map_Editor.Data
{
    public class MapTextData
    {
        public JSONVector3 AmbientLightColor
        {
            get { return new JSONVector3 { X = pAmbientLightColor.X, Y = pAmbientLightColor.Y, Z=pAmbientLightColor.Z }; }
            set
            {
                pAmbientLightColor = new Vector3(value.X, value.Y, value.Z);
            }
        }

        public JsonColor BaackdropColor
        {
            get { return new JsonColor { A = pColor.A, R = pColor.R, G = pColor.G, B = pColor.B }; }
            set
            {
                pColor = System.Drawing.Color.FromArgb(value.A, value.R, value.G, value.B);
            }
        }

        public float Brightness { get; set; }

        public List<ThingGroup> ThingGroups { get; set; }

        public JsonPoint TimeLapseCameraLocation
        {
            get { return new JsonPoint { X = pTimeLapseCamera.X, Y = pTimeLapseCamera.Y }; }
            set
            {
                pTimeLapseCamera = new Point((int)value.X, (int)value.Y);
            }
        }

        public int TimeLapseCameraZoom { get; set; }

        private Vector3 pAmbientLightColor;
        private Point pTimeLapseCamera;
        private Color pColor;

        public struct JSONVector3
        {
            public float X;
            public float Y;
            public float Z;
        }
    }

    public class ThingGroup
    {
        public int BlendMode { get; set; }
        public List<ThingGroupID> ChildGroups {get; set;}
        public ThingGroupID Id { get; set; }
        public int NumSelectedAsDraw {get; set;}
        public int NumSelectedAsLogic {get; set;}
        public int NumThings {get; set;}
        public int NumThingsFrozen {get; set;}
        public int NumThingsSelectable {get; set;}
        public int NumThingsVisible { get; set; }
        public ThingGroupID ParentGroup { get; set; }
        public bool Visible { get; set; }

        public struct ThingGroupID
        {
            public int Id;
            public string Name;
        }
    }
}
