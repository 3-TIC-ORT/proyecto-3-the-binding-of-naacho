using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Hole Tile", menuName = "2D/Tiles/Hole Tile")]
public class HoleTIle : Tile
{
#if UNITY_EDITOR
    [UnityEditor.MenuItem("Assets/Create/2D/Tiles/Hole Tile")]
    public static void CreateHoleTile()
    {
        string path = UnityEditor.EditorUtility.SaveFilePanelInProject("Save Hole Tile", "New Hole Tile", "asset", "a", "asset");
        if (path=="")
        {
            return;
        }
        UnityEditor.AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<HoleTIle>(), path);
    }
#endif
}
