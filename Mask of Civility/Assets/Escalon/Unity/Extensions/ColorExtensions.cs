using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Escalon.Unity
{
    public static class ColorExtensions
    {
        public static Color FromRgb255(float r255, float g255, float b255)
        {
            return new Color(r255 / 255.0f, g255 / 255.0f, b255 / 255.0f);
        }
        
        public static Color RandomHueColor(string seed, float saturation = 0.4f, float value = 0.9f)
        {
            Random.InitState(seed.GetHashCode());
            return Color.HSVToRGB(Random.Range(0.0f, 1.0f), saturation, value);
        }
        
        public static System.Numerics.Vector4 ToVector4(this Color colour)
        {
            return new System.Numerics.Vector4(colour.r, colour.g, colour.b, colour.a);
        }
        
        public static Color ToColor(this System.Numerics.Vector4 vector4)
        {
            return new Color(vector4.X, vector4.Y, vector4.Z, vector4.W);
        }
        
        public static string GetRandomColorFromStringSeed(this string raw)
        {
            using MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(raw));
            return BitConverter.ToString(data).Replace("-", string.Empty).Substring(0, 6);
        }

        public static Color32 ToColor32(this Color color)
        {
            return new Color32(
                (byte)(255 * color.r),
                (byte)(255 * color.g),
                (byte)(255 * color.b),
                (byte)(255 * color.a)
            );
        }

        public static Color HexToColor(this string hex)
        {
            Color color = Color.black;

            if (!string.IsNullOrEmpty(hex))
            {
                if (hex.StartsWith("#"))
                {
                    hex = hex[1..];
                }

                byte r = 0, g = 0, b = 0, a = 255;

                if (hex.Length == 6)
                {
                    r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                }
                else if (hex.Length == 8)
                {
                    byte.TryParse( hex.Substring(0, 2), NumberStyles.HexNumber,new CultureInfo("en-US"), out r);
                    byte.TryParse(hex.Substring(2, 2), NumberStyles.HexNumber, new CultureInfo("en-US"), out g);
                    byte.TryParse(hex.Substring(4, 2), NumberStyles.HexNumber, new CultureInfo("en-US"), out b);
                    byte.TryParse(hex.Substring(6, 2), NumberStyles.HexNumber, new CultureInfo("en-US"), out a);
                }

                color = new Color32(r, g, b, a);
            }

            return color;
        }
    }
}
    