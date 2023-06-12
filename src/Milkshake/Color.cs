using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
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
    /// <summary>
    /// Represents a Color.
    /// </summary>
    public struct Color : IEquatable<Color>
    {
        /// <summary>
        /// Gets the <see cref="Color"/>'s name.
        /// </summary>
        /// <returns>A <see cref="string"/> that is the name of <see cref="Color"/>. <br/>
        /// If the color does not have a defined name, the value will be the formatted <see cref="string"/> of <see cref="Code"/>.</returns>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the <see cref="Color"/> Hex Code.
        /// </summary>
        /// <returns>A <see cref="int"/> hexadecimal value of <see cref="Color"/>.</returns>
        public int Code { get; private set; }

        /// <summary>
        /// Tries to convert the <paramref name="input"/> to an instance of <see cref="Color"/>.
        /// If it successfully converts, <paramref name="color"/> will be updated.
        /// </summary>
        /// <remarks>
        /// The following string representations will be successfully converted: <br/>
        /// <list type="number">
        /// <item>
        /// <term>#BC9A7F</term>
        /// <description>The (#) is optional</description>
        /// </item>
        /// <item>
        /// <term>Snow</term>
        /// <description>The closest string on <see cref="MagickColors"/> will be selected</description>
        /// </item>
        /// </list>
        /// If none is a valid Hex Code or is close enough to any <see cref="MagickColors"/> string, the value is (Unknown #000000).
        /// </remarks>
        /// <param name="input"></param>
        /// <param name="color"></param>
        /// <returns><see langword="true"/> if the Color is successfully parsed; otherwise, <see langword="false"/>.</returns>
        public static bool TryParse(string input, out Color color)
        {
            color = new Color();

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
                value = match.Value[0] is '#' ? match.Value[1..] : match.Value;
                return true;
            }

            value = "000000";
            return false;
        }

        private static bool TryFind(string input, out KeyValuePair<string, string> color)
        {
            var result = Process.ExtractOne(input, MagickColors.Keys);

            if(result.Score > 60)
            {
                color = new KeyValuePair<string, string>(result.Value, MagickColors[result.Value]);
                return true;
            }

            color = new KeyValuePair<string, string>("Unknown", MagickColors["Black"]);
            return false;
        }

        /// <summary>
        /// Returns a formatted string of the <see cref="Color"/> Hex Code.
        /// </summary>
        /// <returns>The <see cref="Color"/> Hex Code, such as "#BC9A7F".</returns>
        public override string ToString()
        {
            var code = Code.ToString("X").ToUpper();

            if (Code is 0)
                code = "000000";

            return "#" + code;
        }

        /// <summary>
        /// Gets a Dictionary with name and hexadecimal code of each standard ImageMagick color.
        /// </summary>
        private static readonly Dictionary<string, string> MagickColorsDict = new()
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

        /// <summary>
        /// Gets a Dictionary with name and hexadecimal code of each standard ImageMagick color.
        /// </summary>
        public static ReadOnlyDictionary<string, string> MagickColors { get; } = new(MagickColorsDict);

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(Color other)
        {
            return string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase) && Code == other.Code;
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Name, StringComparer.InvariantCultureIgnoreCase);
            hashCode.Add(Code);
            return hashCode.ToHashCode();
        }

        public static bool operator ==(Color left, Color right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Color left, Color right)
        {
            return !left.Equals(right);
        }

        public static bool operator ==(Color left, string right)
        {
            if (TryParse(right, out var color))
                return left.Equals(color);

            return false;
        }

        public static bool operator !=(Color left, string right)
        {
            if(TryParse(right, out var color))
                return !left.Equals(color);

            return true;
        }
    }
}
