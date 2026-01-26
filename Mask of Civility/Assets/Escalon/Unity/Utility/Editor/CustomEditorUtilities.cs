using UnityEditor;
using UnityEngine;

namespace Escalon
{
    public static class CustomEditorUtilities
    {
        #region Colors

        public static Color k_colorOff = new Color(239 / 255.0f, 81 / 255.0f, 81 / 255.0f); // Soft red
        public static Color k_colorOn = new Color(139 / 255.0f, 239 / 255.0f, 139 / 255.0f); // Soft green
        public static Color k_colorNeutral = new Color(255 / 255.0f, 198 / 255.0f, 78 / 255.0f); // Mustard yellow
        public static Color k_colorCardBoss = new Color(0.4f, 0.2f, 0.4f, 1); // Purple
        public static Color k_colorCardStandard = new Color(0.2f, 0.2f, 0.4f, 1); // Dark blue
        public static Color k_colorCardExample = new Color(0.2f, 0.8f, 0.4f, 1); // Sea green
        public static Color k_colorInfo = Color.gray; // Gray
        public static Color k_colorCardBackground = new Color(0.2f, 0.2f, 0.2f, 1); // Dark gray
        public static Color k_colorToolBackground = new Color(0.15f, 0.15f, 0.15f, 1); // Very dark gray

        #endregion

        #region Icons

        private static string k_iconDir = "Assets/Cellumancer/ArtAssets/Sprites/Debug/Icons";
        private static Texture2D _validIcon;

        public static Texture2D ValidIcon
        {
            get
            {
                if (_validIcon == null)
                {
                    _validIcon = EditorGUIUtility.Load(k_iconDir + "/ValidIcon.png") as Texture2D;
                }

                return _validIcon;
            }
        }

        private static Texture2D _crossIcon;

        public static Texture2D CrossIcon
        {
            get
            {
                if (_crossIcon == null)
                {
                    _crossIcon = EditorGUIUtility.Load(k_iconDir + "/cancel.png") as Texture2D;
                }

                return _crossIcon;
            }
        }

        private static Texture2D _thumbUpIcon;

        public static Texture2D ThumbUpIcon
        {
            get
            {
                if (_thumbUpIcon == null)
                {
                    _thumbUpIcon = EditorGUIUtility.Load(k_iconDir + "/ThumbUpIcon.png") as Texture2D;
                }

                return _thumbUpIcon;
            }
        }

        private static Texture2D _thumbDownIcon;

        public static Texture2D ThumbDownIcon
        {
            get
            {
                if (_thumbDownIcon == null)
                {
                    _thumbDownIcon = EditorGUIUtility.Load(k_iconDir + "/ThumbDownIcon.png") as Texture2D;
                }

                return _thumbDownIcon;
            }
        }

        private static Texture2D _cardHandIcon;

        public static Texture2D CardHandIcon
        {
            get
            {
                if (_cardHandIcon == null)
                {
                    _cardHandIcon = EditorGUIUtility.Load(k_iconDir + "/CardHandIcon.png") as Texture2D;
                }

                return _cardHandIcon;
            }
        }

        #endregion

        private static GUIStyle _horizontalLineStyle = null;

        public static GUIStyle HorizontalLineStyle
        {
            get
            {
                if (_horizontalLineStyle == null)
                {
                    _horizontalLineStyle = new GUIStyle();
                    _horizontalLineStyle.normal.background = EditorGUIUtility.whiteTexture;
                    _horizontalLineStyle.margin = new RectOffset(0, 0, 4, 4);
                    _horizontalLineStyle.fixedHeight = 1;
                }

                return _horizontalLineStyle;
            }
        }

        private static GUIStyle _smallButtonStyle = null;

        public static GUIStyle SmallButtonStyle
        {
            get
            {
                if (_smallButtonStyle == null)
                {
                    _smallButtonStyle = GUIStyles.Button(k_colorInfo);
                    _smallButtonStyle.fontSize = 8;
                }

                return _smallButtonStyle;
            }
        }

        public static void HorizontalLine(Color color)
        {
            var currentColor = GUI.color;
            GUI.color = color;
            GUILayout.Box(GUIContent.none, HorizontalLineStyle);
            GUI.color = currentColor;
        }

        public static void ToggleLabelField(Color colorOn, Color colorOff, string labelOn, string labelOff,
            bool toggleValue, Color textColor, float width = -1)
        {
            var color = colorOff;
            if (toggleValue)
                color = colorOn;
            var verifiedTex = GUIStyles.MakeTexture(1, 1, color);
            var style = new GUIStyle(GUI.skin.label);
            style.normal.background = verifiedTex;
            style.normal.textColor = textColor;
            if (width != -1)
            {
                style.fixedWidth = width;
            }

            style.alignment = TextAnchor.MiddleCenter;

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(toggleValue ? labelOn : labelOff, style);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public static string TextAreaLabelled(string label, string text, params GUILayoutOption[] options)
        {
            EditorGUILayout.LabelField(label);
            return EditorGUILayout.TextArea(text, options);
        }

        public static Texture2D ToggleIcon(bool toggleCondition, Texture2D iconOn, Texture2D iconOff)
        {
            return toggleCondition ? iconOn : iconOff;
        }

        public static string ToggleText(bool toggleCondition, string textOn, string textOff)
        {
            return toggleCondition ? textOn : textOff;
        }

        public static Color ToggleColor(bool toggleCondition, Color colorOn, Color colorOff)
        {
            return toggleCondition ? colorOn : colorOff;
        }

        public static bool GUIButtonToggleIcon(bool toggleCondition, Texture2D iconOn, Texture2D iconOff, Color colorOn,
            Color colorOff)
        {
            return GUILayout.Button(ToggleIcon(toggleCondition, iconOn, iconOff),
                GUIStyles.Button(ToggleColor(toggleCondition, colorOn, colorOff), 30, 30));
        }

        public static bool GUIButtonToggleText(bool toggleCondition, string textOn, string textOff, Color colorOn,
            Color colorOff, int width = 150, int height = 50)
        {
            return GUILayout.Button(ToggleText(toggleCondition, textOn, textOff),
                GUIStyles.Button(ToggleColor(toggleCondition, colorOn, colorOff), width, height));
        }

        public static bool GUISmallButton(string text, params GUILayoutOption[] layoutOptions)
        {
            return GUILayout.Button(text, SmallButtonStyle, layoutOptions);
        }

        public static bool GUIButtonUpArrow(params GUILayoutOption[] layoutOptions)
        {
            return GUISmallButton("↑", layoutOptions);
        }

        public static bool GUIButtonDownArrow(params GUILayoutOption[] layoutOptions)
        {
            return GUISmallButton("↓", layoutOptions);
        }

        public static bool GUIButtonLeftArrow(params GUILayoutOption[] layoutOptions)
        {
            return GUISmallButton("<", layoutOptions);
        }

        public static bool GUIButtonRightArrow(params GUILayoutOption[] layoutOptions)
        {
            return GUISmallButton(">", layoutOptions);
        }

        public static bool AddButton(string label = "", int size = 30)
        {
            return GUILayout.Button($"+{label}", GUILayout.Height(30), GUILayout.Width(size));
        }

        public static bool MinusButton(string label = "", int size = 30)
        {
            return GUILayout.Button($"-{label}", GUILayout.Height(30), GUILayout.Width(size));
        }
    }
}