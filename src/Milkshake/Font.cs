using FuzzySharp;
using ImageMagick;

namespace Milkshake
{
    /// <summary>
    /// Represents a font that can be easily recognized by Magick.NET.
    /// </summary>
    public struct Font
    {
        /// <summary>
        /// Gets the <see cref="Font"/> name.
        /// </summary>
        /// <returns>A <see cref="string"/> that is the unformatted name of <see cref="Font"/>.</returns>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the formatted <see cref="Font"/> name.
        /// </summary>
        /// <returns>A <see cref="string"/> that is the name of <see cref="Font"/> with whitespaces instead of hyphens.</returns>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Tries to convert the <paramref name="input"/> to an instance of <see cref="Font"/>.
        /// If it successfully converts, <paramref name="font"/> will be updated.
        /// </summary>
        /// <remarks>
        /// If <paramref name="input"/> is not close enough to any <see cref="MagickNET.FontNames"/> string, the font is Arial.
        /// </remarks>
        /// <param name="input"></param>
        /// <param name="font"></param>
        /// <returns><see langword="true"/> if the Color is successfully parsed; otherwise, <see langword="false"/>.</returns>
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

        /// <summary>
        /// Returns <see cref="Font.DisplayName"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that is the name of <see cref="Font"/> with whitespaces instead of hyphens.</returns>
        public override string ToString() => DisplayName;
    }
}
