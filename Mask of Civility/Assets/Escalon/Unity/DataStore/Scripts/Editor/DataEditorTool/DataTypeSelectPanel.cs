using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Escalon
{
    public class DataTypeSelectPanel
    {
        private string _newFileName;
        private int _typeSelectionId = -1;
        private int _instanceTypeId = -1;
        private Vector2 _leftScrollPosition;
        private Vector2 _rightScrollPosition;

        private Action _selectionCallback;
        private OpenEditorDataStore _openEditorDataStore;

        private List<Type> _types;
        private List<GameDataInstance> _dataInstances = new List<GameDataInstance>();

        private EditorGUISplitView _horizontalSplitView;
        private GUIStyle _instanceStyle;

        private Texture2D _deleteIcon;
        private Texture2D _duplicateIcon;

        public DataTypeSelectPanel()
        {
            // Using reflection get all the valid types that can be edited with the tool
            Type type = typeof(IEditorGameData);
            _types = Assembly.GetExecutingAssembly().GetTypesWithInterface(type).ToList();

            _horizontalSplitView = new EditorGUISplitView(EditorGUISplitView.Direction.Horizontal, 0.4f);
        }

        public void Init(Action selectionCallback)
        {
            _selectionCallback = selectionCallback;

            _instanceStyle = new GUIStyle
            {
                normal =
                {
                    background = GUIStyles.MakeTexture(1, 1, new Color(0.2f, 0.2f, 0.2f, 0.2f))
                },
                padding = new RectOffset(5, 5, 2, 2)
            };

            _deleteIcon = EditorGUIUtility.Load(DataToolPaths.s_deleteIconPath) as Texture2D;
            _duplicateIcon = EditorGUIUtility.Load(DataToolPaths.s_duplicateIconPath) as Texture2D;
        }

        public void SetDataStore(OpenEditorDataStore openEditorDataStore)
        {
            _openEditorDataStore = openEditorDataStore;
        }

        public void Draw()
        {
            if (_openEditorDataStore.IncludedSO == null)
            {
                EditorGUILayout.LabelField("No Config Selected", GUIStyles.Heading(Color.white, bold: true));
                return;
            }

            //Header
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("You are editing the data config: ", GUIStyles.Heading(Color.white, bold: true));
            EditorGUILayout.LabelField($"{_openEditorDataStore.DataStore.Id}", GUIStyles.Heading(Color.white, false));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(20);

            //Body
            _horizontalSplitView.BeginSplitView();
            EditorGUILayout.LabelField("Please Select A Data Type To Work On");
            CreateTypeView();
            _horizontalSplitView.Split();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Select a Data Instance to Edit");
            CreateInstanceView();
            EditorGUILayout.EndVertical();
            _horizontalSplitView.EndSplitView();

        }

        public void CreateTypeView()
        {
            _leftScrollPosition = EditorGUILayout.BeginScrollView(_leftScrollPosition);

            for (int i = 0; i < _types.Count; i++)
            {
                if (GUILayout.Button(_types[i].Name))
                {
                    _typeSelectionId = i;
                    InitInstances();
                }
            }

            EditorGUILayout.EndScrollView();
        }

        public void CreateInstanceView()
        {
            if (_typeSelectionId != -1)
            {
                GUIStyle itemStyle = new GUIStyle();
                itemStyle.normal.background = GUIStyles.MakeTexture(1, 1, new Color(0.2f, 0.2f, 0.2f, 0.2f));
                itemStyle.padding = new RectOffset(10, 10, 10, 10);

                _rightScrollPosition = EditorGUILayout.BeginScrollView(_rightScrollPosition);

                for (int i = 0; i < _dataInstances.Count; i++)
                {
                    CreateInstancePanel(i);
                }

                GUILayout.Space(10);

                EditorGUILayout.BeginHorizontal(itemStyle);
                _newFileName = GUILayout.TextField(_newFileName);
                if (GUILayout.Button("[+] Create New Data Configuration"))
                {
                    CreateNewFile();
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndScrollView();
            }
        }

        private void CreateInstancePanel(int index)
        {
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fixedHeight = 30
            };

            EditorGUILayout.BeginHorizontal(_instanceStyle);

            CreateToggleButton(index);

            if (GUILayout.Button(_dataInstances[index].DataWrapper.name, buttonStyle))
            {
                _instanceTypeId = index;
                _selectionCallback.Invoke();
            }

            if (GUILayout.Button(_duplicateIcon, GUIStyles.Button(new Color(0.2f, 0.2f, 0.6f, 1), 30, 30)))
            {
                DuplicateInstance(index);
            }

            if (GUILayout.Button(_deleteIcon, GUIStyles.Button(new Color(0.6f, 0.2f, 0.2f, 1), 30, 30)))
            {
                DeleteInstance(index);
            }

            EditorGUILayout.EndHorizontal();
        }

        private void CreateToggleButton(int index)
        {
            string buttonText;
            Color includedColour;

            if (_dataInstances[index].Include)
            {
                buttonText = "Included";
                includedColour = new Color(0.2f, 0.6f, 0.2f, 1);
            }
            else
            {
                buttonText = "Not Included";
                includedColour = new Color(0.4f, 0.2f, 0.2f, 1);
            }

            if (GUILayout.Button(buttonText, GUIStyles.Button(includedColour, 100, 25)))
            {
                _dataInstances[index].Include = !_dataInstances[index].Include;
                SetIncluded(index, _dataInstances[index].Include);
                for (int i = 0; i < _dataInstances.Count; i++)
                {
                    if (i != index && _dataInstances[index].Include)
                    {
                        SetIncluded(i, false);
                    }
                }
            }
        }

        private void SetIncluded(int index, bool include)
        {
            ScriptableObject data = _dataInstances[index].DataWrapper;

            _dataInstances[index].Include = include;

            if (include)
            {
                _openEditorDataStore.IncludedSO.IncludedDataWrappers.Add(data);
            }
            else if (_openEditorDataStore.IncludedSO.IncludedDataWrappers.Contains(data))
            {
                _openEditorDataStore.IncludedSO.IncludedDataWrappers.Remove(data);
            }
        }

        private void InitInstances()
        {
            var scriptableObjects =
                AssetDatabaseUtility.FindScriptableObjectAssetsByType(_types[_typeSelectionId], out var guids);

            _dataInstances.Clear();
            for (int i = 0; i < scriptableObjects.Count; i++)
            {
                GameDataInstance gameDataInstance = new GameDataInstance
                {
                    DataWrapper = scriptableObjects[i],
                    Include = _openEditorDataStore.IncludedSO.IncludedDataWrappers.Contains(scriptableObjects[i]),
                    GUID = guids[i]
                };
                _dataInstances.Add(gameDataInstance);
            }

            _newFileName = _types[_typeSelectionId].Name;
        }

        public ScriptableObject GetDataInstance()
        {
            if (_instanceTypeId != -1)
            {
                return _dataInstances[_instanceTypeId].DataWrapper;
            }

            return null;
        }

        private void CreateNewFile()
        {
            string path = $"{DataToolPaths.s_dataFilePath}/{_newFileName}";
            var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>($"{path}.asset");
            if (asset != null)
            {
                path += "_New";
            }

            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance(_types[_typeSelectionId]), $"{path}.asset");
            InitInstances();
        }

        private void DeleteInstance(int index)
        {
            if (_openEditorDataStore.IncludedSO.IncludedDataWrappers.Contains(_dataInstances[index].DataWrapper))
            {
                _openEditorDataStore.IncludedSO.IncludedDataWrappers.Remove(_dataInstances[index].DataWrapper);
            }

            IEditorGameData gameDataWrapper = (IEditorGameData)_dataInstances[index].DataWrapper;
            gameDataWrapper.Delete();

            InitInstances();
        }

        private void DuplicateInstance(int index)
        {
            IEditorGameData gameDataWrapper = (IEditorGameData)_dataInstances[index].DataWrapper;
            gameDataWrapper.Duplicate($"{DataToolPaths.s_dataFilePath}/", $"{_dataInstances[index].DataWrapper.name}_Copy.asset");
            InitInstances();
        }

        public void SaveDataStore()
        {
            if (_openEditorDataStore.IncludedSO != null)
            {
                _openEditorDataStore.Save();
            }
        }

        public void Reset()
        {
            _typeSelectionId = -1;
            _instanceTypeId = -1;
        }
    }
}