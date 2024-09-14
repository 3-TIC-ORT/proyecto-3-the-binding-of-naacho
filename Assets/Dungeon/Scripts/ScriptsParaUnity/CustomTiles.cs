using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Custom Tile", menuName = "2D/Tiles/Custom Tile")]

public class CustomTiles : Tile
{
#if UNITY_EDITOR
    [UnityEditor.MenuItem("Assets/Create/2D/Tiles/Custom Tile")]
    public static void CreateHoleTile()
    {
        string path = UnityEditor.EditorUtility.SaveFilePanelInProject("Save Custom Tile", "New Custom Tile", "asset", "a", "asset");
        if (path == "")
        {
            return;
        }
        UnityEditor.AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<CustomTiles>(), path);
    }
#endif
}
