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
    public bool tilemapsMerged;

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
        if (templates.minCompleted) OrderTilemaps();
        else StartCoroutine(WaitForMergeTilemaps());
    }
    // Ordena los tilemaps para que cuando se mergeen lo hagan por prioridad. Primero los normales, después las 1DoorRooms...
    void OrderTilemaps()
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
        List<Tilemap> oneDoorTilemaps = new List<Tilemap>();
        List<Tilemap> normalTilemaps = new List<Tilemap>();
        // Transfiere los tiles de cada Tilemap hijo al EntryRoom
        foreach (Tilemap tilemap in childTilemaps)
        {   
            string tilemapName = tilemap.name;
            string tilemapTag = tilemap.tag;

            if (tilemapName == "Closed(Clone)")
            {
                // Añade el closedRoom a closedTilemaps 
                closedTilemaps.Add(tilemap);
            }
            else if (tilemapTag=="1DoorRoom")
            {
                // Añade el 1DoorRoom a oneDoorTilemaps 
                oneDoorTilemaps.Add(tilemap);
            }
            // Si no es un closedRoom ni una 1DoorRoom
            else
            {
                normalTilemaps.Add(tilemap);
            }
            
            
        }
        // La razón de esto es asegurarse que los tiles de los closedRoom "predominen". Si no, podría pasar que
        // se pongan los tiles del closedRoom en la Entry Room y que después estos tiles sean tapados por los tiles
        // de la puerta de otra room
        if (normalTilemaps.Count > 0) MergeTilemaps(normalTilemaps);

        if (oneDoorTilemaps.Count > 0) MergeTilemaps(oneDoorTilemaps);
        
        if (closedTilemaps.Count > 0) MergeTilemaps(closedTilemaps);
        
        // Por si acaso xd
        targetTilemap.RefreshAllTiles();
    }
    // Pone todos los tiles de todos los tilemaps en el tilemap EntryRoom
    private void MergeTilemaps(List<Tilemap> tilemaps)
    {
        
        foreach (Tilemap tilemap in tilemaps)
        {
            BoundsInt bounds = tilemap.cellBounds;

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
        tilemapsMerged = true;
    }
    
}
