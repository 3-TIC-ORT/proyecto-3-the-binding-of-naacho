using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;
using UnityEngine.XR;

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
    // Espera a que no se generen más rooms para llamar a MergeTilemaps()
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
        yield return new WaitForSecondsRealtime(1f);
        if (templates.minCompleted) MergeTilemaps();
        else WaitForMergeTilemaps();
    }
    // Pone todos los tiles de todos los tilemaps en el tilemap EntryRoom
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


        // Lista de los closedTilemaps
        List<Tilemap> closedTilemaps = new List<Tilemap>();
        // Transfiere los tiles de cada Tilemap hijo al EntryRoom
        foreach (Tilemap tilemap in childTilemaps)
        {   // Si no es un closedRoom
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
            // Añade el closedRoom a closedTilemaps 
            else closedTilemaps.Add(tilemap);
            
        }
        if (closedTilemaps.Count > 0)
        {
            // Lo mismo que el otro foreach pero para los closedRooms
            // La razón de esto es asegurarse que los tiles de los closedRoom "predominen". Si no, podría pasar que
            // se pongan los tiles del closedRoom en la Entry Room y que después estos tiles sean tapados por los tiles
            // de la puerta de otra room
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
        // Por si acaso xd
        targetTilemap.RefreshAllTiles();
    }
}
