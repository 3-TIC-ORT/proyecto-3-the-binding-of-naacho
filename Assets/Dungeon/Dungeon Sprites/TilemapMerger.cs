using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapMerger : MonoBehaviour
{
    public Grid grid; // Asigna aquí tu Grid
    public Tilemap targetTilemap; // El Tilemap donde se fusionarán todos los tiles

    void Start()
    {
        Invoke("MergeTilemaps", 5);
    }

    void MergeTilemaps()
    {
        List<Tilemap> childTilemaps = new List<Tilemap>();

        // Recolecta todos los Tilemaps hijos
        foreach (Transform child in grid.transform)
        {
            Tilemap tilemap = child.GetComponent<Tilemap>();
            if (tilemap != null && tilemap != targetTilemap)
            {
                childTilemaps.Add(tilemap);
            }
        }

        // Transfiere los tiles de cada Tilemap hijo al Tilemap objetivo
        foreach (Tilemap tilemap in childTilemaps)
        {
            BoundsInt bounds = tilemap.cellBounds;

            // Itera sobre cada posición en los límites del Tilemap
            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                if (tilemap.HasTile(pos))
                {
                    TileBase tile = tilemap.GetTile(pos);
                    Vector2 worldPosition = tilemap.CellToWorld(pos);
                    Vector3Int whereToSet = targetTilemap.WorldToCell(worldPosition);
                    targetTilemap.SetTile(whereToSet, tile); // Coloca el tile en el Tilemap objetivo
                    tilemap.SetTile(pos, null); // Elimina el tile del Tilemap hijo
                }
            }
        }

        // Actualiza el mapa para asegurarse de que todos los tiles se muestran correctamente
        targetTilemap.RefreshAllTiles();
    }
}
