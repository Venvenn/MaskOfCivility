using UnityEditor;
using UnityEngine;

namespace Escalon
{
    public class DataEditPanel
    {
        private ScriptableObject _scriptableObject;
        private Editor _scriptableObjectEditor;
        private string _selectionName;
        private Vector2 _scrollPos;


        public void Init(ScriptableObject scriptableObject)
        {
            if (scriptableObject == null)
            {
                EditorGUILayout.LabelField("No Data Instance Selected", GUIStyles.Heading(Color.white, bold: true));
                return;
            }

            if (!EditorGUIUtility.editingTextField)
            {
                _selectionName = scriptableObject.name;
            }

            _scriptableObject = scriptableObject;
            _scriptableObjectEditor = Editor.CreateEditor(_scriptableObject);
        }

        public void Draw()
        {
            if (_scriptableObject == null)
            {
                EditorGUILayout.LabelField("No Data Instance Selected", GUIStyles.Heading(Color.white, bold: true));
                return;
            }

            _selectionName = EditorGUILayout.TextField($"{_selectionName}", GUIStyles.Heading(Color.white));
            if (!EditorGUIUtility.editingTextField && _selectionName != _scriptableObject.name)
            {
                RenameScriptableObject(_scriptableObject, _selectionName);
            }

            GUILayout.Space(15);

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            _scriptableObjectEditor.OnInspectorGUI();
            EditorGUILayout.EndScrollView();
        }

        private void RenameScriptableObject(ScriptableObject scriptableObject, string newName)
        {
            string filePath = AssetDatabase.GetAssetPath(scriptableObject);
            AssetDatabase.RenameAsset(filePath, newName);
        }
    }
}