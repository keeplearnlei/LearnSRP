using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PathUtils
{
    public static readonly string ASSET_DATA_PATH = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("/") + 1);

    public static string AssetPathToFilePath(string assetPath)
    { 
        return Path.Combine(ASSET_DATA_PATH, assetPath);
    }

    public static string FilePathToAssetPath(string filePath)
    {
        filePath = filePath.Replace("\\", "/");
        return filePath.Replace(ASSET_DATA_PATH, "");
    }
}
