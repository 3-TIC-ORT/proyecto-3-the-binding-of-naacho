using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapMerger : MonoBehaviour
{
    private RoomTemplates templates;
    public Grid grid; // Asigna aquí tu Grid
    public Tilemap targetTilemap; // El Tilemap donde se fusionarán todos los tiles

    void Start()
    {
        templates = GetComponent<RoomTemplates>();
        StartCoroutine(WaitForMergeTilemaps());
    }

    IEnumerator WaitForMergeTilemaps()
    {
        int localRoomsGenerated = templates.roomsGenerated;
        int lastLocalRoomsGenerated = -1;
        while (lastLocalRoomsGenerated != localRoomsGenerated)
        {
            lastLocalRoomsGenerated = localRoomsGenerated;
            yield return new WaitForSecondsRealtime(2f);
            localRoomsGenerated = templates.roomsGenerated;
        }
        yield return new WaitForSecondsRealtime(3f);
        MergeTilemaps();
    }
    void MergeTilemaps()
    {
        Debug.Log("ASD");
        List<Tilemap> childTilemaps = new List<Tilemap>();

        foreach (Transform child in grid.transform)
        {
            Tilemap tilemap = child.GetComponent<Tilemap>();
            if (tilemap != null && tilemap != targetTilemap)
            {
                childTilemaps.Add(tilemap);
            }
        }

        // Transfiere los tiles de cada Tilemap hijo al Tilemap objetivo
        List<Tilemap> closedTilemaps = new List<Tilemap>();
        foreach (Tilemap tilemap in childTilemaps)
        {
            if (tilemap.name!="Closed(Clone)")
            {
                //Debug.Log(tilemap.name);
                BoundsInt bounds = tilemap.cellBounds;

                // Itera sobre cada posición en los límites del Tilemap
                foreach (Vector3Int pos in bounds.allPositionsWithin)
                {
                    if (tilemap.HasTile(pos))
                    {
                        TileBase tile = tilemap.GetTile(pos);
                        Vector2 worldPosition = tilemap.CellToWorld(pos);
                        Vector3Int whereToSet = targetTilemap.WorldToCell(worldPosition);
                        targetTilemap.SetTile(whereToSet, tile);
                        tilemap.SetTile(pos, null); 
                    }
                }
            }
            else closedTilemaps.Add(tilemap);
            
        }
        if (closedTilemaps.Count > 0)
        {
            Debug.Log(closedTilemaps.Count);
            foreach (Tilemap closedTilemap in closedTilemaps)
            {
                BoundsInt bounds = closedTilemap.cellBounds;

                foreach (Vector3Int pos in bounds.allPositionsWithin)
                {
                    if (closedTilemap.HasTile(pos))
                    {
                        TileBase tile = closedTilemap.GetTile(pos);
                        Vector2 worldPosition = closedTilemap.CellToWorld(pos);
                        Vector3Int whereToSet = targetTilemap.WorldToCell(worldPosition);
                        targetTilemap.SetTile(whereToSet, tile);
                        closedTilemap.SetTile(pos, null);
                    }
                }
            }
        }

        targetTilemap.RefreshAllTiles();
    }
}
