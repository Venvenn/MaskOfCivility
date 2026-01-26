using UnityEditor;
using UnityEngine;

namespace Escalon
{
    public static class GUIStyles
    {
        public static GUIStyle Title(Color textColour)
        {
            GUIStyle style = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 24
            };
            style.normal.textColor = textColour;
            return style;
        }

        public static GUIStyle Heading(Color textColour, bool centered = true, bool bold = true, bool wordWrap = false)
        {
            GUIStyle style = new GUIStyle
            {
                alignment = centered ? TextAnchor.MiddleCenter : TextAnchor.MiddleLeft,
                fontStyle = bold ? FontStyle.Bold : FontStyle.Normal,
                fontSize = 18,
                normal =
                {
                    textColor = textColour,
                },
                wordWrap = wordWrap,
            };
            return style;
        }

        public static GUIStyle Label(Color textColour, bool centered = false, bool bold = false)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                alignment = centered ? TextAnchor.MiddleCenter : TextAnchor.MiddleLeft,
                fontStyle = bold ? FontStyle.Bold : FontStyle.Normal,
                normal =
                {
                    textColor = textColour
                }
            };
            return style;
        }

        public static GUIStyle Padded(int left, int right, int top, int bottom)
        {
            GUIStyle style = new GUIStyle
            {
                padding = new RectOffset(left, right, top, bottom)
            };



            return style;
        }

        public static GUIStyle Button(Color backgroundColour, Color textColor, int width = -1, int height = -1)
        {
            GUIStyle style = new GUIStyle(GUI.skin.button)
            {
                normal =
                {
                    background = MakeTexture(1, 1, backgroundColour),
                    textColor = textColor,
                },
                active =
                {
                    background = MakeTexture(1, 1,
                        new Color(backgroundColour.r - 0.2f, backgroundColour.g - 0.2f, backgroundColour.b - 0.2f)),
                    textColor = textColor,
                },
                margin = new RectOffset(EditorGUI.indentLevel * 15, 0, 0, 0),
            };

            if (width != -1)
            {
                style.fixedWidth = width;
            }

            if (height != -1)
            {
                style.fixedHeight = height;
            }

            return style;
        }

        public static GUIStyle Button(Color backgroundColour, int width = -1, int height = -1)
        {
            return Button(backgroundColour, Color.white, width, height);
        }

        public static GUIStyle Tool()
        {
            GUIStyle style = new GUIStyle(GUI.skin.box);
            style.normal.background = GUIStyles.MakeTexture(1, 1, CustomEditorUtilities.k_colorToolBackground);
            return style;
        }

        public static Texture2D MakeTexture(int width, int height, Color colour)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = colour;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }
    }
}