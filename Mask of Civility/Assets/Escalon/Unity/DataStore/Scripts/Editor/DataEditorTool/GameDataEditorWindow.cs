using UnityEditor;
using UnityEngine;

namespace Escalon.Unity
{
  /// <summary>
  /// Main Window for the Game Data Editor Tool
  /// </summary>
  public class GameDataEditorWindow : EditorWindow
  {
    private ConfigurationSelectPanel _configurationSelectPanel = new ConfigurationSelectPanel();
    private DataTypeSelectPanel _dataTypeSelectPanel = new DataTypeSelectPanel();
    private DataEditPanel _dataEditPanel = new DataEditPanel();

    private Texture2D _icon;
    private int _toolbarId = 0;
    private string[] _toolbarStrings = { "Select Configuration", "Select Data Type", "Edit Data" };

    [MenuItem("Tools/Game Data Editor")]
    public static void ShowDataEditor()
    {
      EditorWindow wnd = GetWindow<GameDataEditorWindow>();
      wnd.titleContent = new GUIContent("Game Data Editor");

      // Limit size of the window
      wnd.minSize = new Vector2(720, 480);

      // Set Icon
      wnd.titleContent.image = EditorGUIUtility.Load(DataToolPaths.s_dataToolIconPath) as Texture2D;
    }

    private void OnEnable()
    {
      _configurationSelectPanel.Init(ConfigSelectionCallback);
      _dataTypeSelectPanel.Init(InstanceSelectionCallback);
    }

    private void CreateGUI()
    {
      _icon = EditorGUIUtility.Load(DataToolPaths.s_dataToolIconPath) as Texture2D;
    }

    public void OnGUI()
    {
      GUILayout.Space(15);

      //Header
      GUI.DrawTexture(new Rect(0, 0, 50, 50), _icon);
      EditorGUILayout.LabelField("Data Editor Tool", GUIStyles.Title(Color.white));
      if (GUI.Button(new Rect(position.width - 80, 0, 80, 30), "Save"))
      {
        _dataTypeSelectPanel.SaveDataStore();
      }

      GUILayout.Space(25);

      //Toolbar
      _toolbarId = GUILayout.Toolbar(_toolbarId, _toolbarStrings);
      GUILayout.Space(15);
      switch (_toolbarId)
      {
        case 0:
        {
          _configurationSelectPanel.Draw();
          break;
        }
        case 1:
        {
          _dataTypeSelectPanel.Draw();
          DrawBackButton();
          break;
        }
        case 2:
        {
          _dataEditPanel.Draw();
          DrawBackButton();
          break;
        }
      }

      Repaint();
    }

    private void OnLostFocus()
    {
      GUI.FocusControl(null);
      _dataTypeSelectPanel.SaveDataStore();
    }

    private void ConfigSelectionCallback()
    {
      _toolbarId = 1;
      _dataTypeSelectPanel.Reset();
      _dataTypeSelectPanel.SetDataStore(_configurationSelectPanel.GetDataConfig());
      GUI.FocusControl(null);
    }

    private void InstanceSelectionCallback()
    {
      _toolbarId = 2;
      _dataEditPanel.Init(_dataTypeSelectPanel.GetDataInstance());
      GUI.FocusControl(null);
    }

    private void DrawBackButton()
    {
      GUILayout.Space(5);

      GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
      {
        fixedHeight = 30
      };

      if (GUILayout.Button("Back", buttonStyle))
      {
        if (_toolbarId == 1)
        {
          _dataTypeSelectPanel.Reset();
        }

        GUI.FocusControl(null);
        _toolbarId = Mathf.Max(0, _toolbarId - 1);
      }
    }
  }
}