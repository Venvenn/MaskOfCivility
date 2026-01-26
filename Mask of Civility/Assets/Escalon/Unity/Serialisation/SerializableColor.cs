using System;
using UnityEngine;

namespace Escalon
{
    [Serializable]
    public struct SerializableColor
    {
        public float R;
        public float G;
        public float B;
    }

    public static class SerializableColorExtensions
    {
        public static Color ToUnityColor(this SerializableColor color)
        {
            return new Color(color.R, color.G, color.B);
        }

        public static SerializableColor ToSerializableColor(this Color color)
        {
            return new SerializableColor()
            {
                R = color.r,
                G = color.g,
                B = color.b,
            };
        }

        public static SerializableColor LerpTo(this SerializableColor color, SerializableColor to, float t)
        {
            return Color.Lerp(color.ToUnityColor(), to.ToUnityColor(), t).ToSerializableColor();
        }
    }
}
