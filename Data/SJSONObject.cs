using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Hades_Map_Editor.Data
{
    public class SJSONObject
    {
        private bool _bVal;
        private Dictionary<string, SJSONObject> _dictVal;
        private string _stringVal;
        private List<SJSONObject> _listVal;
        private double _dVal;

        private int _valType; //0 = None, 1 == bool, 2 == dict, 3 == string, 4 == list, 5 == double

        public SJSONObject()
        {
            _valType = 0;
        }
        public SJSONObject(SJSONObject o)
        {
            _valType = o._valType;
            switch (o._valType)
            {
                case 1:
                    _bVal = o._bVal;
                    break;
                case 2:
                    _dictVal = o._dictVal;
                    break;
                case 3:
                    _stringVal = o._stringVal;
                    break;
                case 4:
                    _listVal = o._listVal;
                    break;
                case 5:
                    _dVal = o._dVal;
                    break;
            }
        }
        public SJSONObject(bool val)
        {
            _bVal = val;
            _valType = 1;
        }
        public SJSONObject(Dictionary<string, SJSONObject> val)
        {
            _dictVal = val;
            _valType = 2;
        }
        public SJSONObject(string val)
        {
            _stringVal = val;
            _valType = 3;
        }
        public SJSONObject(List<SJSONObject> val)
        {
            _listVal = val;
            _valType = 4;
        }
        public SJSONObject(double val)
        {
            _dVal = val;
            _valType = 5;
        }

        public Type getType()
        {
            switch (_valType)
            {
                case 0:
                    return typeof(string);
                case 1:
                    return typeof(bool);
                case 2:
                    return typeof(Dictionary<string, SJSONObject>);
                case 3:
                    return typeof(string);
                case 4:
                    return typeof(List<SJSONObject>);
                case 5:
                    return typeof(double);
            }

            return typeof(string);
        }

        public override string ToString()
        {
            string result = "";
            switch (_valType)
            {
                case 0:
                    result = "NONE";
                    break;
                case 1:
                    result = _bVal.ToString();
                    break;
                case 2:
                    result += "{\n";
                    foreach(KeyValuePair<string, SJSONObject> kvp in _dictVal)
                    {
                        result += "\t" + kvp.Key + ": {\n\t\t" + kvp.Value.ToString() + "\n\t}";
                    }
                    result += "\n}\n";
                    break;
                case 3:
                    result = _stringVal;
                    break;
                case 4:
                    result += "[";
                    foreach(SJSONObject o in _listVal)
                    {
                        result += "\t" + o.ToString() + "\n";
                    }
                    result += "]\n";
                    break;
                case 5:
                    result = _dVal.ToString();
                    break;
            }

            return result;
        }

        public static implicit operator bool(SJSONObject obj)
        {
            return obj._valType == 1? obj._bVal : false;
        }
        public static implicit operator Dictionary<string, SJSONObject>(SJSONObject obj)
        {
            return obj._valType == 2 ? obj._dictVal : new Dictionary<string, SJSONObject>();
        }
        public static implicit operator string(SJSONObject obj)
        {
            return obj._valType == 3 ? obj._stringVal : "";
        }
        public static implicit operator List<SJSONObject>(SJSONObject obj)
        {
            return obj._valType == 4 ? obj._listVal : new List<SJSONObject>();
        }
        public static implicit operator double(SJSONObject obj)
        {
            return obj._valType == 5 ? obj._dVal : 0;
        }

        public static SJSONObject Merge(SJSONObject obj1, SJSONObject obj2)
        {
            Dictionary<string, SJSONObject> result = new Dictionary<string, SJSONObject>((Dictionary<string, SJSONObject>)obj1);

            Dictionary<string, SJSONObject> dict2 = new Dictionary<string, SJSONObject>((Dictionary<string, SJSONObject>)obj2);
            if (result == dict2)
            {
                return obj1;
            }


            foreach (KeyValuePair<string, SJSONObject> kvp2 in dict2)
            {
                if(kvp2.Value.getType() == typeof(Dictionary<string, SJSONObject>))
                {
                    if (result.ContainsKey(kvp2.Key))
                    {
                        SJSONObject temp = Merge(kvp2.Value, new SJSONObject(result));
                        result[kvp2.Key] = temp;
                    }
                    else
                    {
                        result.Add(kvp2.Key, new SJSONObject(kvp2.Value));
                    }
                }
                else if (!result.ContainsKey(kvp2.Key))
                {
                    result.Add(kvp2.Key, new SJSONObject(kvp2.Value));
                }
            }

            return new SJSONObject(result);
        }
    }

    //public class SJSONMapObject
    //{
    //    public string Name;
    //    public string InheritFrom;
    //    public bool DisplayInEditor;
    //    public SJSONMapThing thing;
    //}

    //public class SJSONMapThing
    //{
    //    public bool EditorDrawOutlineBounds;
    //    public string Graphic;
    //    public double Tallness;
    //    public Vector2 Offset;
    //    public List<Vector2> Points;
    //}
}
