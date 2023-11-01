
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityToolbarExtender;

public class OperateHelper
{
    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        EditorApplication.projectWindowItemOnGUI += OnProjectWindowGUI;
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarWindowGUI;

        ToolbarExtender.LeftToolbarGUI.Add(OnLeftToolbarGUI);
        ToolbarExtender.RightToolbarGUI.Add(OnRightToolbarGUI);
    }

    public static void OnProjectWindowGUI(string guid, Rect selectionRect)
    { 
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Space && selectionRect.Contains(Event.current.mousePosition))
        {
            string selectAsset = AssetDatabase.GetAssetPath(Selection.activeObject);
            EditorUtility.RevealInFinder(selectAsset);
            Event.current.Use();
        }
    }

    private static void OnHierarWindowGUI(int instanceID, Rect selectionRect)
    {
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Space)
        {
            var activeGO = Selection.activeGameObject;
            Undo.RecordObject(activeGO, "Active");
            if (activeGO != null)
            {
                activeGO.SetActive(!activeGO.activeSelf);
            }
            Event.current.Use();
        }
    }

    private static void OnLeftToolbarGUI()
    {
        if (GUILayout.Button(new GUIContent("������", "��������"), ToolbarStyles.commandButtonStyle))
        {
            EditorSceneManager.OpenScene("Assets/Scenes/GameRoot.unity");
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(new GUIContent("��������", "���еĵ�һ������"), ToolbarStyles.commandButtonStyle))
        {
            EditorSceneManager.OpenScene("Assets/Scenes/GameEntry.unity");
        }
    }

    private static void OnRightToolbarGUI()
    {
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(new GUIContent("�ұ߰�ť", ""), ToolbarStyles.commandButtonStyle))
        {
            EditorSceneManager.OpenScene("Assets/Scenes/GameRoot.unity");
        }
    }
}
