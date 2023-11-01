using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

public class PrefabUtils
{
    public static string GetPrefabPath(GameObject prefab)
    {
        if (PrefabUtility.IsPartOfPrefabAsset(prefab))
            return AssetDatabase.GetAssetPath(prefab);
        else if (PrefabUtility.IsPartOfPrefabInstance(prefab))
        {
            prefab = PrefabUtility.GetCorrespondingObjectFromSource(prefab);
            return AssetDatabase.GetAssetPath(prefab);
        }
        else
        {
            var currentStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (currentStage)
                return currentStage.assetPath;
        }

        Debug.LogError("无法找到预设路径，请检查！");
        return string.Empty;
    }
}
