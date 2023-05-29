using System.Collections.Immutable;
using System.Collections.ObjectModel;
using FuzzySharp;
using ImageMagick;
// ReSharper disable SuspiciousTypeConversion.Global

namespace Milkshake
{
    /// <summary>
    /// Represents a font that can be easily recognized by Magick.NET.
    /// </summary>
    public struct Font : IEquatable<Font>
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
        /// Returns <see cref="DisplayName"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that is the name of <see cref="Font"/> with whitespaces instead of hyphens.</returns>
        public override string ToString() => DisplayName;

        /// <summary>
        /// Gets a List containing all available fonts recognized by ImageMagick.
        /// </summary>
        public static IReadOnlyList<string> AvailableFonts { get; } = new List<string>(MagickNET.FontNames);

        public override bool Equals(object? obj)
        {
            return obj switch
            {
                null => false,
                string fontName => string.Equals(Name, fontName, StringComparison.InvariantCultureIgnoreCase),
                _ => false
            };
        }

        public bool Equals(Font other)
        {
            return string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(DisplayName, other.DisplayName, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Name, StringComparer.InvariantCultureIgnoreCase);
            hashCode.Add(DisplayName, StringComparer.InvariantCultureIgnoreCase);
            return hashCode.ToHashCode();
        }
        
        public static bool operator ==(Font left, Font right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Font left, Font right)
        {
            return !left.Equals(right);
        }

        public static bool operator ==(Font left, string right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Font left, string right)
        {
            return !left.Equals(right);
        }
    }
}
