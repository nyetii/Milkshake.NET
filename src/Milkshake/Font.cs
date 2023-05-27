using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuzzySharp;
using FuzzySharp.Extractor;
using ImageMagick;

namespace Milkshake
{
    public struct Font
    {
        public string Name { get; private set; }
        public string DisplayName { get; private set; }

        public static bool TryFind(string input, out Font font)
        {
            font = new Font();
            var result = Process.ExtractOne(input, MagickNET.FontNames);

            if (result.Score < 60)
            {
                font.Name = "Arial";
                font.DisplayName = font.Name;
                return false;
            }

            var displayName = new string(result.Value.Replace('-', ' '));

            font.Name = result.Value;
            font.DisplayName = displayName;
            return true;
        }

        public override string ToString() => DisplayName;
    }
}
