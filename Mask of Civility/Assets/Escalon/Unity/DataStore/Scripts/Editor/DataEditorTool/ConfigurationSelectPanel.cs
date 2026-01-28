using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;
namespace Escalon.Unity
{
    public class ConfigurationSelectPanel
    {
        private const string k_defualtNewFileText = "NewDataConfig";

        private OpenEditorDataStore[] _dataConfigs;
        private string[] _filePathsDataStore;

        private string _newFileName;
        private string _selectionName = "";
        private int _selectionId = -1;

        private Vector2 _textScroll;
        private Vector2 _scrollPosition;
        private Action _selectionCallback;

        private EditorGUISplitView _horizontalSplitView = new EditorGUISplitView(EditorGUISplitView.Direction.Horizontal);

        private GUIStyle _panelStyle;

        private Texture2D _deleteIcon;
        private Texture2D _duplicateIcon;

        /// <summary>
        /// This method is used both to initialise the view and refresh it 
        /// </summary>
        public void Init(Action selectionCallback)
        {
            _selectionCallback = selectionCallback;
            _selectionId = -1;
            _textScroll = Vector2.zero;
            _selectionName = "";

            //Get all relevant files
            if (!Directory.Exists($"{Application.dataPath}/{DataToolPaths.s_filePathDataStore}"))
            {
                Directory.CreateDirectory($"{Application.dataPath}/{DataToolPaths.s_filePathDataStore}");
            }

            _filePathsDataStore = Directory.GetFiles($"{Application.dataPath}/{DataToolPaths.s_filePathDataStore}", "*.json", SearchOption.TopDirectoryOnly);
            var includes = AssetDatabaseUtility.FindScriptableObjectAssetsByType<EditorDataIncludedSO>(out var guids);

            //Set up variables used for displaying the panels correctly
            _newFileName = k_defualtNewFileText;
            _panelStyle = new GUIStyle
            {
                normal =
                {
                    background = GUIStyles.MakeTexture(1, 1, new Color(0.2f, 0.2f, 0.2f, 0.2f))
                },
                padding = new RectOffset(5, 2, 2, 2)
            };

            //Set up the temporary DataStore wrapper that is used to handle it while its being edited
            _dataConfigs = new OpenEditorDataStore[_filePathsDataStore.Length];
            List<string> errors = new List<string>();
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Error = delegate(object sender, ErrorEventArgs args)
                {
                    errors.Add(args.ErrorContext.Error.Message);
                    args.ErrorContext.Handled = true;
                }
            };
            for (int i = 0; i < _filePathsDataStore.Length; i++)
            {
                string dataStoreJson = File.ReadAllText(_filePathsDataStore[i]);
                _dataConfigs[i].DataStore = JsonConvert.DeserializeObject<DataStore>(dataStoreJson, jsonSerializerSettings);
                _dataConfigs[i].FilePathDataStore = _filePathsDataStore[i];

                for (int j = 0; j < errors.Count; j++)
                {
                    Debug.LogError($"[ConfigurationSelectPanel] Error: {errors[j]}");
                }

                EditorDataIncludedSO include = includes.Find(x => x.name == _dataConfigs[i].DataStore.Id);
                if (include == null)
                {
                    include = ScriptableObject.CreateInstance<EditorDataIncludedSO>();
                    include.name = _dataConfigs[i].DataStore.Id;
                    AssetDatabase.CreateAsset(include, $"{DataToolPaths.s_filePathIncluded}/{include.name}.asset");
                }
                _dataConfigs[i].IncludedSO = include;
            }

            _deleteIcon = EditorGUIUtility.Load(DataToolPaths.s_deleteIconPath) as Texture2D;
            _duplicateIcon = EditorGUIUtility.Load(DataToolPaths.s_duplicateIconPath) as Texture2D;

            GUI.FocusControl(null);
        }

        public OpenEditorDataStore GetDataConfig()
        {
            if (_selectionId != -1)
            {
                return _dataConfigs[_selectionId];
            }

            return default;
        }

        public void Draw()
        {
            _horizontalSplitView.BeginSplitView();
            CreateScrollView();
            _horizontalSplitView.Split();
            CreateDetailPanel();
            _horizontalSplitView.EndSplitView();
        }

        public void CreateScrollView()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Please Select A Data Configuration To Work On");
            GUILayout.EndHorizontal();

            GUIStyle scrollViewStyle = new GUIStyle
            {
                normal =
                {
                    background = GUIStyles.MakeTexture(1, 1, new Color(0.5f, 0.5f, 0.5f, 0.5f))
                }
            };

            GUIStyle buttonStyle = new GUIStyle();
            buttonStyle.normal.background = GUIStyles.MakeTexture(1, 1, new Color(0.8f, 0.8f, 0.8f, 0.8f));
            buttonStyle.padding = new RectOffset(5, 5, 2, 2);

            GUIStyle itemStyle = new GUIStyle();
            itemStyle.normal.background = GUIStyles.MakeTexture(1, 1, new Color(0.2f, 0.2f, 0.2f, 0.2f));
            itemStyle.padding = new RectOffset(10, 10, 10, 10);

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, true, GUIStyle.none,
                GUIStyle.none, scrollViewStyle);

            GUILayout.Space(2);
            for (int i = 0; i < _dataConfigs.Length; i++)
            {
                CreateInstancePanel(i);
            }

            EditorGUILayout.BeginHorizontal(itemStyle);
            _newFileName = GUILayout.TextField(_newFileName);
            if (GUILayout.Button("[+] Create New Data Configuration"))
            {
                CreateNewFile();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndScrollView();
        }

        private void CreateInstancePanel(int index)
        {
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fixedHeight = 30,
                alignment = TextAnchor.MiddleLeft
            };
            buttonStyle.normal.background = GUIStyles.MakeTexture(1, 1, new Color(0.8f, 0.8f, 0.8f, 0.8f));
            buttonStyle.normal.textColor = Color.black;

            EditorGUILayout.BeginHorizontal(_panelStyle);

            if (GUILayout.Button(_dataConfigs[index].DataStore.Id, buttonStyle))
            {
                _selectionId = index;
                _textScroll = Vector2.zero;
                _selectionName = _dataConfigs[index].DataStore.Id;
            }

            if (GUILayout.Button(_duplicateIcon, GUIStyles.Button(new Color(0.2f, 0.2f, 0.6f, 1), 30, 30)))
            {
                DuplicateConfig(index);
            }

            if (GUILayout.Button(_deleteIcon, GUIStyles.Button(new Color(0.6f, 0.2f, 0.2f, 1), 30, 30)))
            {
                DeleteConfig(index);
            }

            EditorGUILayout.EndHorizontal();
        }

        private void CreateDetailPanel()
        {
            if (_selectionId != -1)
            {
                EditorGUILayout.BeginVertical();

                _selectionName = EditorGUILayout.TextField($"{_selectionName}", GUIStyles.Heading(Color.white));
                if (!EditorGUIUtility.editingTextField && _selectionName != _dataConfigs[_selectionId].DataStore.Id)
                {
                    RenameConfig(_selectionId, _selectionName);
                }

                GUILayout.Space(5);

                string currentVersion = _dataConfigs[_selectionId].DataStore.Version == Application.version
                    ? "(Current Version)"
                    : string.Empty;
                EditorGUILayout.LabelField(
                    $"Created With Game Version: {_dataConfigs[_selectionId].DataStore.Version} {currentVersion}");

                DrawIncludeInGameButtons();

                EditorGUILayout.LabelField("Notes:");
                _textScroll = EditorGUILayout.BeginScrollView(_textScroll, GUILayout.ExpandHeight(true),
                    GUILayout.ExpandHeight(false));
                EditorStyles.textField.wordWrap = true;
                _dataConfigs[_selectionId].DataStore.Notes =
                    EditorGUILayout.TextArea(_dataConfigs[_selectionId].DataStore.Notes);
                EditorGUILayout.EndScrollView();

                GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
                {
                    fixedHeight = 50,
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 24,
                    padding = new RectOffset(10, 5, 5, 10)
                };
                if (GUILayout.Button("Edit", buttonStyle))
                {
                    GUI.FocusControl(null);
                    _selectionCallback.Invoke();
                }

                EditorGUILayout.EndVertical();
            }
        }

        private void DrawIncludeInGameButtons()
        {
            string buttonText;
            bool drawDefaultButton;
            Color includedColour;

            if (_dataConfigs[_selectionId].DataStore.IncludeInGame)
            {
                buttonText = "Included In Game";
                includedColour = new Color(0.2f, 0.6f, 0.2f, 1);
                drawDefaultButton = true;
            }
            else
            {
                buttonText = "Not Included In Game";
                includedColour = new Color(0.4f, 0.2f, 0.2f, 1);
                drawDefaultButton = false;
            }

            if (GUILayout.Button(buttonText, GUIStyles.Button(includedColour)))
            {
                _dataConfigs[_selectionId].DataStore.IncludeInGame =
                    !_dataConfigs[_selectionId].DataStore.IncludeInGame;

                if (!_dataConfigs[_selectionId].DataStore.IncludeInGame)
                {
                    _dataConfigs[_selectionId].DataStore.Default = false;
                }
            }

            if (drawDefaultButton)
            {
                string defaultButtonText;
                Color defaultColour;
                if (_dataConfigs[_selectionId].DataStore.Default)
                {
                    defaultButtonText = "Default";
                    defaultColour = new Color(0.2f, 0.2f, 0.6f, 1);
                }
                else
                {
                    defaultButtonText = "Not Default";
                    defaultColour = Color.gray;
                }

                if (GUILayout.Button(defaultButtonText, GUIStyles.Button(defaultColour)))
                {
                    _dataConfigs[_selectionId].DataStore.Default = !_dataConfigs[_selectionId].DataStore.Default;
                    _dataConfigs[_selectionId].Save();
                    for (int i = 0; i < _dataConfigs.Length; i++)
                    {
                        if (i != _selectionId && _dataConfigs[_selectionId].DataStore.Default)
                        {
                            _dataConfigs[i].DataStore.Default = false;
                            _dataConfigs[i].Save();
                        }
                    }
                }
            }
        }

        private void CreateNewFile()
        {
            //Data Store
            string fileName = _newFileName;
            string path = $"{Application.dataPath}/{DataToolPaths.s_filePathDataStore}/{_newFileName}";
            if (File.Exists(path + ".json"))
            {
                path += "_New";
                fileName += "_New";
            }

            DataStore newDataStore = new DataStore
            {
                Id = fileName,
                Version = Application.version
            };

            string json = JsonConvert.SerializeObject(newDataStore);
            File.WriteAllText(path + ".json", json);

            //SO Include
            EditorDataIncludedSO include = ScriptableObject.CreateInstance<EditorDataIncludedSO>();
            include.name = newDataStore.Id;
            AssetDatabase.CreateAsset(include, $"{DataToolPaths.s_filePathIncluded}/{include.name}.asset");
            AssetDatabase.SaveAssets();
            Init(_selectionCallback);
        }

        private void DeleteConfig(int index)
        {
            //Data Store
            File.Delete(
                $"{Application.dataPath}/{DataToolPaths.s_filePathDataStore}/{_dataConfigs[index].DataStore.Id}.json");
            File.Delete(
                $"{Application.dataPath}/{DataToolPaths.s_filePathDataStore}/{_dataConfigs[index].DataStore.Id}.json.meta");

            //SO Include
            AssetDatabase.DeleteAsset(
                $"{DataToolPaths.s_filePathIncluded}/{_dataConfigs[index].IncludedSO.name}.asset");

            Init(_selectionCallback);
        }

        private void DuplicateConfig(int index)
        {
            //Data Store
            DataStore newDataStore = _dataConfigs[index].DataStore;
            newDataStore.Id += "_Copy";
            newDataStore.Default = false;
            newDataStore.IncludeInGame = false;
            string json = JsonConvert.SerializeObject(newDataStore);
            File.WriteAllText($"{Application.dataPath}/{DataToolPaths.s_filePathDataStore}/{newDataStore.Id}.json",
                json);

            //SO Include
            EditorDataIncludedSO newInclude = UnityEngine.Object.Instantiate(_dataConfigs[index].IncludedSO);
            newInclude.name = newDataStore.Id;
            AssetDatabase.CreateAsset(newInclude, $"{DataToolPaths.s_filePathIncluded}/{newInclude.name}.asset");

            Init(_selectionCallback);
        }

        private void RenameConfig(int index, string newName)
        {
            //Data Store
            File.Delete(
                $"{Application.dataPath}/{DataToolPaths.s_filePathDataStore}/{_dataConfigs[index].DataStore.Id}.json");
            File.Delete(
                $"{Application.dataPath}/{DataToolPaths.s_filePathDataStore}/{_dataConfigs[index].DataStore.Id}.json.meta");

            _dataConfigs[_selectionId].DataStore.Id = newName;
            string filePath =
                $"{Application.dataPath}/{DataToolPaths.s_filePathDataStore}/{_dataConfigs[_selectionId].DataStore.Id}.json";
            _dataConfigs[_selectionId].FilePathDataStore = filePath;
            string json = JsonConvert.SerializeObject(_dataConfigs[_selectionId].DataStore);
            File.WriteAllText(filePath, json);

            //SO Include
            AssetDatabase.RenameAsset($"{DataToolPaths.s_filePathIncluded}/{_dataConfigs[index].IncludedSO.name}.asset",
                newName);
        }
    }
}
