using UnityEditor;
using UnityEngine;

namespace Escalon.FlowState
{
    public class FlowStateSelectionDropdown : PopupWindowContent
    {
        private const float k_columnWidth = 200.0f;

        private GUIStyle _titleButtonStyle;
        private GUIStyle _buttonStyle;
        private GUIStyle _selectedButtonStyle;

        private Vector2 _scrollPosBuild;

        private FlowStateSelectionSettings _settings;

        public FlowStateSelectionDropdown(FlowStateSelectionSettings settings)
        {
            _settings = settings;
            InitStyles();
        }

        void InitStyles()
        {
            var blankTex = MakeTex(new Color(0f, 0f, 0f, 0f));
            var selectedTex = MakeTex(new Color(0f, 0f, 0f, 0.3f));

            var hoverState = new GUIStyleState()
            {
                background = selectedTex,
                textColor = GUI.skin.button.onHover.textColor,
            };
            _buttonStyle = new GUIStyle(GUI.skin.label)
            {
                onHover = hoverState,
                hover = hoverState,
            };
            _buttonStyle.normal.background = blankTex;

            _selectedButtonStyle = new GUIStyle(_buttonStyle);
            _selectedButtonStyle.normal.background = selectedTex;

            _titleButtonStyle = new GUIStyle(EditorStyles.boldLabel);
            _titleButtonStyle.onHover = _buttonStyle.onHover;
            _titleButtonStyle.hover = _buttonStyle.hover;
            _titleButtonStyle.normal.background = blankTex;
        }

        public static Texture2D MakeTex(Color col)
        {
            var texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            texture.SetPixel(0, 0, col);
            texture.Apply();
            return texture;
        }

        public override Vector2 GetWindowSize()
        {
            var width = k_columnWidth;
            var maxRow = Mathf.Max(_settings.ValidStatesForEntry.Length, 1);
            var height = Mathf.Min(22 * maxRow + 26, Screen.currentResolution.height * 0.5f);
            return new Vector2(width, height);
        }

        public override void OnClose()
        {
            if (EditorUtility.IsDirty(_settings))
            {
                AssetDatabase.SaveAssets();
            }
        }

        public override void OnGUI(Rect rect)
        {
            EditorGUILayout.BeginHorizontal();
            DrawBuildScenes();
            EditorGUILayout.EndHorizontal();

            if (Event.current.type == EventType.MouseMove && EditorWindow.mouseOverWindow == editorWindow)
            {
                if (editorWindow != null)
                {
                    editorWindow.Repaint();
                }
            }
        }

        void DrawBuildScenes()
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Valid Entry Points", EditorStyles.boldLabel, GUILayout.Height(20.0f));
            EditorGUILayout.EndHorizontal();

            if (_settings.ValidStatesForEntry.Length > 0)
            {
                _scrollPosBuild = EditorGUILayout.BeginScrollView(_scrollPosBuild);
                for (int i = 0; i < _settings.ValidStatesForEntry.Length; i++)
                {
                    DrawSelection(_settings.ValidStatesForEntry[i], i);
                }

                EditorGUILayout.EndScrollView();
            }
            else
            {
                GUILayout.Label("No States in FlowStateSelectionSettings");
            }

            EditorGUILayout.EndVertical();
        }

        void DrawSelection(string state, int index = -1)
        {
            if (state == null) return;

            GUILayout.BeginHorizontal();
            var style = _settings.SelectedEntryFlowState == state ? _selectedButtonStyle : _buttonStyle;
            if (GUILayout.Button(index >= 0 ? $"{index}\t{state}" : state, style))
            {
                SelectState(state);
            }

            GUILayout.EndHorizontal();
        }

        void SelectState(string scene)
        {
            _settings.SelectedEntryFlowState = scene;
            EditorUtility.SetDirty(_settings);
            editorWindow.Close();
        }
    }
}