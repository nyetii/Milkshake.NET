﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FuzzySharp;
using ImageMagick;

namespace Milkshake
{
    public struct Color
    {
        public string Name;
        public int Code;

        public static bool TryParse(string input, out Color color)
        {
            color = new Color();

            //TryFind(input, out var a);

            if (TryMatch(input, out string value))
            {
                color.Code = int.Parse(value, NumberStyles.HexNumber);
                color.Name = color.ToString();
                return true;
            }
            
            if (TryFind(input, out var pair))
            {
                color.Name = pair.Key;
                color.Code = int.Parse(pair.Value[1..], NumberStyles.HexNumber);
                return true;
            }

            color.Name = "Unknown";
            color.Code = 0x000000;
            return false;
        }
        
        private static bool TryMatch(string input, out string value)
        {
            var match = Regex.Match(input, "^#?[0-9a-fA-F]{6}$");

            if (match.Success)
            {
                value = match.Value[1..];
                return true;
            }

            value = "000000";
            return false;
        }

        private static bool TryFind(string input, out KeyValuePair<string, string> color)
        {
            //var a = typeof(MagickColors).GetFields(BindingFlags.Public | BindingFlags.Static)
            //    .Where(x => x.FieldType == typeof(MagickColor))
            //    .ToDictionary(k => k.Name, 
            //        v => (MagickColor?)v.GetValue(null));

            //var t = typeof(MagickColors);

            //var fields = t.GetMembers(BindingFlags.Public | BindingFlags.Static)
            //    .OfType<PropertyInfo>().Where(x => x.PropertyType == typeof(MagickColor));

            //var dict = fields.ToDictionary(k => k.Name,
            //    v => (MagickColor?)v.GetValue(null));
            

            var result = Process.ExtractOne(input, MagickColors.Keys);

            //dict.TryGetValue(result, out var value);

            if(result.Score > 60)
            {
                color = new KeyValuePair<string, string>(result.Value, MagickColors[result.Value]);
                return true;
            }

            color = new KeyValuePair<string, string>("Black", MagickColors["Black"]);
            return false;
            //if (result.Score < 60)
            //{
            //    font.Name = "Arial";
            //    font.DisplayName = font.Name;
            //    return false;
            //}
        }

        public override string ToString()
        {
            return "#" + Code.ToString("X").ToUpper();
        }

        private static readonly Dictionary<string, string> MagickColors = new()
        {
            {"None", "#00000000"},
            {"Transparent", "#00000000"},
            {"AliceBlue", "#F0F8FF"},
            {"AntiqueWhite", "#FAEBD7"},
            {"Aqua", "#00FFFF"},
            {"Aquamarine", "#7FFFD4"},
            {"Azure", "#F0FFFF"},
            {"Beige", "#F5F5DC"},
            {"Bisque", "#FFE4C4"},
            {"Black", "#000000"},
            {"BlanchedAlmond", "#FFEBCD"},
            {"Blue", "#0000FF"},
            {"BlueViolet", "#8A2BE2"},
            {"Brown", "#A52A2A"},
            {"BurlyWood", "#DEB887"},
            {"CadetBlue", "#5F9EA0"},
            {"Chartreuse", "#7FFF00"},
            {"Chocolate", "#D2691E"},
            {"Coral", "#FF7F50"},
            {"CornflowerBlue", "#6495ED"},
            {"Cornsilk", "#FFF8DC"},
            {"Crimson", "#DC143C"},
            {"Cyan", "#00FFFF"},
            {"DarkBlue", "#00008B"},
            {"DarkCyan", "#008B8B"},
            {"DarkGoldenrod", "#B8860B"},
            {"DarkGray", "#A9A9A9"},
            {"DarkGreen", "#006400"},
            {"DarkKhaki", "#BDB76B"},
            {"DarkMagenta", "#8B008B"},
            {"DarkOliveGreen", "#556B2F"},
            {"DarkOrange", "#FF8C00"},
            {"DarkOrchid", "#9932CC"},
            {"DarkRed", "#8B0000"},
            {"DarkSalmon", "#E9967A"},
            {"DarkSeaGreen", "#8FBC8F"},
            {"DarkSlateBlue", "#483D8B"},
            {"DarkSlateGray", "#2F4F4F"},
            {"DarkTurquoise", "#00CED1"},
            {"DarkViolet", "#9400D3"},
            {"DeepPink", "#FF1493"},
            {"DeepSkyBlue", "#00BFFF"},
            {"DimGray", "#696969"},
            {"DodgerBlue", "#1E90FF"},
            {"Firebrick", "#B22222"},
            {"FloralWhite", "#FFFAF0"},
            {"ForestGreen", "#228B22"},
            {"Fuchsia", "#FF00FF"},
            {"Gainsboro", "#DCDCDC"},
            {"GhostWhite", "#F8F8FF"},
            {"Gold", "#FFD700"},
            {"Goldenrod", "#DAA520"},
            {"Gray", "#808080"},
            {"Green", "#008000"},
            {"GreenYellow", "#ADFF2F"},
            {"Honeydew", "#F0FFF0"},
            {"HotPink", "#FF69B4"},
            {"IndianRed", "#CD5C5C"},
            {"Indigo", "#4B0082"},
            {"Ivory", "#FFFFF0"},
            {"Khaki", "#F0E68C"},
            {"Lavender", "#E6E6FA"},
            {"LavenderBlush", "#FFF0F5"},
            {"LawnGreen", "#7CFC00"},
            {"LemonChiffon", "#FFFACD"},
            {"LightBlue", "#ADD8E6"},
            {"LightCoral", "#F08080"},
            {"LightCyan", "#E0FFFF"},
            {"LightGoldenrodYellow", "#FAFAD2"},
            {"LightGreen", "#90EE90"},
            {"LightGray", "#D3D3D3"},
            {"LightPink", "#FFB6C1"},
            {"LightSalmon", "#FFA07A"},
            {"LightSeaGreen", "#20B2AA"},
            {"LightSkyBlue", "#87CEFA"},
            {"LightSlateGray", "#778899"},
            {"LightSteelBlue", "#B0C4DE"},
            {"LightYellow", "#FFFFE0"},
            {"Lime", "#00FF00"},
            {"LimeGreen", "#32CD32"},
            {"Linen", "#FAF0E6"},
            {"Magenta", "#FF00FF"},
            {"Maroon", "#800000"},
            {"MediumAquamarine", "#66CDAA"},
            {"MediumBlue", "#0000CD"},
            {"MediumOrchid", "#BA55D3"},
            {"MediumPurple", "#9370DB"},
            {"MediumSeaGreen", "#3CB371"},
            {"MediumSlateBlue", "#7B68EE"},
            {"MediumSpringGreen", "#00FA9A"},
            {"MediumTurquoise", "#48D1CC"},
            {"MediumVioletRed", "#C71585"},
            {"MidnightBlue", "#191970"},
            {"MintCream", "#F5FFFA"},
            {"MistyRose", "#FFE4E1"},
            {"Moccasin", "#FFE4B5"},
            {"NavajoWhite", "#FFDEAD"},
            {"Navy", "#000080"},
            {"OldLace", "#FDF5E6"},
            {"Olive", "#808000"},
            {"OliveDrab", "#6B8E23"},
            {"Orange", "#FFA500"},
            {"OrangeRed", "#FF4500"},
            {"Orchid", "#DA70D6"},
            {"PaleGoldenrod", "#EEE8AA"},
            {"PaleGreen", "#98FB98"},
            {"PaleTurquoise", "#AFEEEE"},
            {"PaleVioletRed", "#DB7093"},
            {"PapayaWhip", "#FFEFD5"},
            {"PeachPuff", "#FFDAB9"},
            {"Peru", "#CD853F"},
            {"Pink", "#FFC0CB"},
            {"Plum", "#DDA0DD"},
            {"PowderBlue", "#B0E0E6"},
            {"Purple", "#800080"},
            {"RebeccaPurple", "#663399"},
            {"Red", "#FF0000"},
            {"RosyBrown", "#BC8F8F"},
            {"RoyalBlue", "#4169E1"},
            {"SaddleBrown", "#8B4513"},
            {"Salmon", "#FA8072"},
            {"SandyBrown", "#F4A460"},
            {"SeaGreen", "#2E8B57"},
            {"SeaShell", "#FFF5EE"},
            {"Sienna", "#A0522D"},
            {"Silver", "#C0C0C0"},
            {"SkyBlue", "#87CEEB"},
            {"SlateBlue", "#6A5ACD"},
            {"SlateGray", "#708090"},
            {"Snow", "#FFFAFA"},
            {"SpringGreen", "#00FF7F"},
            {"SteelBlue", "#4682B4"},
            {"Tan", "#D2B48C"},
            {"Teal", "#008080"},
            {"Thistle", "#D8BFD8"},
            {"Tomato", "#FF6347"},
            {"Turquoise", "#40E0D0"},
            {"Violet", "#EE82EE"},
            {"Wheat", "#F5DEB3"},
            {"White", "#FFFFFF"},
            {"WhiteSmoke", "#F5F5F5"},
            {"Yellow", "#FFFF00"},
            {"YellowGreen", "#9ACD32"}
        };

    }
}
