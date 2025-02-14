using Hades_Map_Editor.Components;
using Hades_Map_Editor.Managers;
using IronPython.Compiler.Ast;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Hades_Map_Editor.Data
{
    internal class SJSONLoader
    {
        private static void consume(CustomFileReader inputStream, string what)
        {
            skipWhitespace(inputStream);
            int whatLen = what.Length;
            if (inputStream.Peek(whatLen) != what)
            {
                //error
            }
            inputStream.Skip(whatLen);
        }
        private static bool isWhitespace(char c)
        {
            return c == ' ' || c == '\t' || c == '\n' || c == '\r';
        }
        private static bool isNumberSeperator(char c)
        {
            return (isWhitespace(c) || c == ',' || c == ']' || c == '}');
        }
        private static void skipCStyleComment(CustomFileReader inputStream)
        {
            inputStream.Skip(2);

            while(true)
            {
                if (!inputStream.good())
                {
                    break;
                }

                char next_char = inputStream.Peek();
                if (next_char == '*')
                {
                    string comment_end = inputStream.Peek(2);
                    if (comment_end == "*/")
                    {
                        inputStream.Skip(2);
                        break;
                    }
                    else
                    {
                        inputStream.Skip(1);
                    }
                }

                inputStream.Skip(1);
            }
        }
        private static void skipCppStyleComment(CustomFileReader inputStream)
        {
            inputStream.Skip(2);
            while (true)
            {
                if (!inputStream.good())
                {
                    break;
                }

                char next_char = inputStream.Peek();
                if (next_char == '\n')
                {
                    break;
                }
                inputStream.Skip(1);
            }
        }
        private static char skipCharactersAndWhitespace(CustomFileReader inputStream, int numCharsToSkip)
        {
            inputStream.Skip(numCharsToSkip);
            return skipWhitespace(inputStream);
        }
        private static char skipWhitespace(CustomFileReader inputStream)
        {
            int i = 0;
            char next_char;
            while (true)
            {
                next_char = inputStream.Peek();
                if (!isWhitespace(next_char))
                {
                    if (next_char == '/')
                    {
                        string comment_start = inputStream.Peek(2);
                        if (comment_start == "/*")
                        {
                            skipCStyleComment(inputStream);
                            continue;
                        }
                        else if (comment_start == "//")
                        {
                            skipCppStyleComment(inputStream);
                            continue;
                        }
                    }
                    break;
                }
                inputStream.Skip(1);
            }
            return next_char;
        }
        private static bool isIdentifier(char c)
        {
            return Regex.Match(c.ToString(), "[A-Za-z0-9_]").Success;
        }
        private static SJSONObject decodeDict(CustomFileReader inputStream, bool delimited = false)
        {
            /*
            delimited -- if ``True``, parsing will stop once the end-of-dictionary
                         delimiter has been reached(``}``)
            */
            Dictionary<string, SJSONObject> result = new Dictionary<string, SJSONObject>();

            char next_char = skipWhitespace(inputStream);

            if (next_char == '{')
            {
                inputStream.Skip(1);
            }

            next_char = skipWhitespace(inputStream);

            while (true)
            {
                if (!inputStream.good())
                {
                    break;
                }

                if (next_char == '}')
                {
                    inputStream.Skip(1);
                    break;
                }

                string key = decodeString(inputStream, true).ToString();

                next_char = skipWhitespace(inputStream);
                if (next_char == '=' || next_char == ':')
                {
                    consume(inputStream, next_char.ToString());
                }

                SJSONObject value = parse(inputStream);

                result.Add(key, value);

                next_char = skipWhitespace(inputStream);
                if (next_char == ',')
                {
                    next_char = skipCharactersAndWhitespace(inputStream, 1);
                }
            }

            return new SJSONObject(result);
        }
        private static SJSONObject decodeString(CustomFileReader inputStream, bool allowIdentifiers = false)
        {
            if (inputStream._pointer == 309108)
            {
                int i = 0;
            }

            skipWhitespace(inputStream);

            string result = "";

            bool isQuoted = inputStream.Peek() == '"';

            if (!allowIdentifiers && !isQuoted)
            {
                return new SJSONObject();
            }

            bool rawQuotes = false;
            if (isQuoted)
            {
               if (inputStream.Peek(3) == "\"\"\"")
                {
                    rawQuotes = true;
                    inputStream.Skip(3);
                }
                else
                {
                    inputStream.Skip(1);
                }
            }

            bool parseAsIdentifier = !isQuoted;

            while (true)
            {
                if (!inputStream.good())
                {
                    break;
                }

                char nextChar = inputStream.Peek();

                if (parseAsIdentifier && !isIdentifier((char)nextChar))
                {
                    break;
                }

                if (rawQuotes)
                { 

                    if (inputStream.Peek(3) == "\"\"\"" && inputStream.Peek(4) != "\"\"\"\"")
                    {
                        inputStream.Skip(3);
                        break;
                    }
                    else
                    {
                        result += nextChar;
                        inputStream.Skip(1);
                    }
                }
                else
                {
                    if (nextChar == '"')
                    {
                        inputStream.Read();
                        break;
                    }
                    else
                    {
                        result += nextChar;
                        inputStream.Skip(1);
                    }
                }
            }
            return new SJSONObject(result);
        }
        private static SJSONObject decodeNumber(CustomFileReader inputStream, char next_char)
        {
            string input = "";
            while(true)
            {
                if (!inputStream.good())
                {
                    break;
                }

                if (isNumberSeperator(next_char))
                {
                    break;
                }

                input += next_char;
                inputStream.Skip(1);

                next_char = inputStream.Peek();
            }

            return new SJSONObject(Convert.ToDouble(input));
        }
        private static SJSONObject parseList(CustomFileReader inputStream)
        {
            List<SJSONObject> results = new List<SJSONObject>();

            char next_char = skipCharactersAndWhitespace(inputStream, 1);
            while(true)
            {
                if (!inputStream.good())
                {
                    break;
                }

                if (next_char == ']')
                {
                    inputStream.Skip(1);
                    break;
                }

                SJSONObject value = parse(inputStream);
                results.Add(value);

                next_char = skipWhitespace(inputStream);
                if (next_char == ',')
                {
                    next_char = skipCharactersAndWhitespace(inputStream, 1);
                }
            }

            return new SJSONObject(results);
        }

        public static SJSONObject parse(CustomFileReader inputStream)
        {
            char next_char = skipWhitespace(inputStream);

            switch(next_char)
            {
                case 't':
                    consume(inputStream, "true");
                    return new SJSONObject(true);
                case 'f':
                    consume(inputStream, "false");
                    return new SJSONObject(false);
                case 'n':
                    consume(inputStream, "null");
                    return new SJSONObject();
                case '{':
                    return decodeDict(inputStream, true);
                case '"':
                    return decodeString(inputStream);
                case '[':
                    return parseList(inputStream);
            }

            return decodeNumber(inputStream, next_char);
        }

        public static Dictionary<string, SJSONObject> LoadSJSONObject(StreamReader inputStream)
        {
            CustomFileReader fileStream = new CustomFileReader(inputStream);

            return decodeDict(fileStream, false);
        }

        public static Dictionary<string, SJSONObject> LoadSJSONObstacles(StreamReader inputStream)
        {
            Dictionary<string, SJSONObject> originalDict = LoadSJSONObject(inputStream);
            
            List<SJSONObject> obstacles = originalDict["Obstacles"];
            Dictionary<string, SJSONObject> obstacleDict = new Dictionary<string, SJSONObject>();

            //Link names for easier searching for other functions
            foreach(SJSONObject obs in obstacles)
            {
                Dictionary<string, SJSONObject> obstacle = obs;
                string name = obstacle["Name"];
                obstacleDict.Add(name, obs);
            }

            return obstacleDict;
        }

        public static Dictionary<string, SJSONObject> LoadAllSJSONObstacles()
        {
            ConfigManager configManager = ConfigManager.GetInstance();
            List<string> fileNames = new List<string>{
                configManager.GetPath(ConfigType.HadesPath) + "\\Content\\Game\\Obstacles\\House.sjson",
                configManager.GetPath(ConfigType.HadesPath) + "\\Content\\Game\\Obstacles\\Light.sjson",
                configManager.GetPath(ConfigType.HadesPath) + "\\Content\\Game\\Obstacles\\Styx.sjson",
                configManager.GetPath(ConfigType.HadesPath) + "\\Content\\Game\\Obstacles\\Surface.sjson",
                configManager.GetPath(ConfigType.HadesPath) + "\\Content\\Game\\Obstacles\\Tartarus.sjson",
                configManager.GetPath(ConfigType.HadesPath) + "\\Content\\Game\\Obstacles\\Temple.sjson",
                configManager.GetPath(ConfigType.HadesPath) + "\\Content\\Game\\Obstacles\\Travel.sjson",
                configManager.GetPath(ConfigType.HadesPath) + "\\Content\\Game\\Obstacles\\1_DevObstacles.sjson",
                configManager.GetPath(ConfigType.HadesPath) + "\\Content\\Game\\Obstacles\\AmbienceGenerators.sjson",
                configManager.GetPath(ConfigType.HadesPath) + "\\Content\\Game\\Obstacles\\Asphodel.sjson",
                configManager.GetPath(ConfigType.HadesPath) + "\\Content\\Game\\Obstacles\\Atmosphere.sjson",
                configManager.GetPath(ConfigType.HadesPath) + "\\Content\\Game\\Obstacles\\Backgrounds.sjson",
                configManager.GetPath(ConfigType.HadesPath) + "\\Content\\Game\\Obstacles\\BiomeMap.sjson",
                configManager.GetPath(ConfigType.HadesPath) + "\\Content\\Game\\Obstacles\\Chaos.sjson",
                configManager.GetPath(ConfigType.HadesPath) + "\\Content\\Game\\Obstacles\\Elysium.sjson",
                configManager.GetPath(ConfigType.HadesPath) + "\\Content\\Game\\Obstacles\\Epilogue.sjson",
                configManager.GetPath(ConfigType.HadesPath) + "\\Content\\Game\\Obstacles\\FX.sjson",
                configManager.GetPath(ConfigType.HadesPath) + "\\Content\\Game\\Obstacles\\Gameplay.sjson",
                configManager.GetPath(ConfigType.HadesPath) + "\\Content\\Game\\Obstacles\\Graybox.sjson",
                configManager.GetPath(ConfigType.HadesPath) + "\\Content\\Game\\Obstacles\\GUI.sjson"
            };
            Dictionary<string, SJSONObject> fullDict = new Dictionary<string, SJSONObject>();

            //Load all
            foreach(string file in fileNames)
            {
                StreamReader sr = new StreamReader(file);
                Dictionary<string, SJSONObject> obstacles = LoadSJSONObstacles(sr);

                foreach(KeyValuePair<string, SJSONObject> kvp in obstacles)
                {
                    fullDict.Add(kvp.Key, new SJSONObject(kvp.Value));
                }

                sr.Close();
            }

            Dictionary<string, SJSONObject> linkedDict = new Dictionary<string, SJSONObject>();

            //InhritFrom linking
            foreach (KeyValuePair<string,  SJSONObject> kvp in fullDict)
            {
                Dictionary<string, SJSONObject> obstacle = kvp.Value;
                Dictionary<string, SJSONObject> next = kvp.Value;

                while(next.ContainsKey("InheritFrom"))
                {
                    next = fullDict[(string)next["InheritFrom"]];
                    obstacle = SJSONObject.Merge(new SJSONObject(obstacle), new SJSONObject(next));
                }

                linkedDict.Add(kvp.Key, new SJSONObject(obstacle));
            }

            return linkedDict;
        }
    }
}

