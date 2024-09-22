using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class RoomConector : MonoBehaviour
{
    public int pointDirection;
    private GameObject grid;
    private RoomTemplates templates;
    public TileBase tileConector;
    public TileBase wall;
    private Tilemap targetTilemap;
    public bool bothRoomsAreConected=false;
    public bool doorsDestroyed=false;
    public bool spawnPointMoved=false;
    public bool colisioneperonosecumplio = false;
    void Start()
    {
        grid = GameObject.Find("Grid");
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        targetTilemap=GameObject.Find("Entry Room").GetComponent<Tilemap>();
        // Si aparezco en el vacío de Unity, destruyeme.
        if (targetTilemap.GetTile(targetTilemap.WorldToCell(transform.position)) == null) Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        // Si colisiono con un spawnPoint que no sea una closedRoom, una bossRoom o una treasureRoom entonces me voy a mover a mi direcci�n respectiva.
        if (col.gameObject.CompareTag("SpawnPoint"))
        {
            bool isClosedRoom = col.gameObject.GetComponent<RoomSpawner>().spawnedClosedRoom;
            bool isBossRoom = col.gameObject.GetComponent<RoomSpawner>().bossRoom;
            bool isTreasureRoom = col.gameObject.GetComponent<RoomSpawner>().treasureRoom;
            if (!isClosedRoom && !isBossRoom && !isTreasureRoom && !spawnPointMoved)
            {
                // Se posiciona el roomConector en el centro del �rea de las dos habitaciones que deber�a ser borrada.
                if (pointDirection == 1) transform.position += (Vector3)(Vector2.down * templates.centerBetweenVerticaltalRooms);
                else if (pointDirection == 2) transform.position += (Vector3)(Vector2.up * templates.centerBetweenVerticaltalRooms);
                else if (pointDirection == 3) transform.position += (Vector3)(Vector2.left * templates.centerBetweenHorizontalRooms);
                else if (pointDirection == 4) transform.position += (Vector3)(Vector2.right * templates.centerBetweenHorizontalRooms);
                spawnPointMoved = true;
                CheckIfBothRoomsAreConnected();
            }
            else if (!spawnPointMoved) Destroy(gameObject);
        }
        
    }
    // Si las habitaciones ya están conectadas, entonces no la conectes rompiendo la pared
    private void CheckIfBothRoomsAreConnected()
    {
        if (pointDirection==3 || pointDirection==4)
        {
            if (GetTile(targetTilemap, (Vector2)transform.position + Vector2.left * (templates.horizontalDoorToDoorRoomArea.x/2-0.5f)) == tileConector && GetTile(targetTilemap, (Vector2)transform.position + Vector2.right * 5.5f) == tileConector)
            {
                bothRoomsAreConected = true;
            }
        }
        else
        {
            if (GetTile(targetTilemap, (Vector2)transform.position + Vector2.down * (templates.verticalDoorToDoorRoomArea.y/2-0.5f)) == tileConector && GetTile(targetTilemap, (Vector2)transform.position + Vector2.up * 5.5f) == tileConector)
            {
                bothRoomsAreConected = true;
            }
        }
        if (!bothRoomsAreConected) StartCoroutine(ConnectRooms());
        else Destroy(gameObject);
        
    }
    // Agarra un tile dada su posición. Esta función ya existe y pero como la cree después de darme cuenta la usamos xd
    TileBase GetTile(Tilemap tilemap, Vector2 worldPosition)
    {
        Vector3Int tilePosition = tilemap.WorldToCell(worldPosition);
        TileBase tile = tilemap.GetTile(tilePosition);
        return tile;
    }
    
    // Conecta las habitaciones
    IEnumerator ConnectRooms()
    {
        doorsDestroyed = true;
        // Area de int, es decir, dos esquinas.
        BoundsInt area;
        if (pointDirection == 1 || pointDirection == 2)
        {
            // Los valores de las esquinas encierran un �rea que es el que se necesita para unir las rooms haciendo una m�s grande
            area = new BoundsInt
            (
                // Esquina 1. Se divide por -2 para que una esquina est� en la esquina inferior izquierda, al principio este roomConector
                // est� en el centro del �rea.
                templates.verticalDoorToDoorRoomArea/-2 + targetTilemap.WorldToCell(transform.position),
                // Esquina 2. Es lo mismo que el �rea.
                templates.verticalDoorToDoorRoomArea
            );
        }
        else if (pointDirection == 3 || pointDirection == 4)
        {
            area = new BoundsInt
            (
                templates.horizontalDoorToDoorRoomArea/-2 + targetTilemap.WorldToCell(transform.position),
                templates.horizontalDoorToDoorRoomArea
            );
        }
        else yield break;

        foreach (var pos in area.allPositionsWithin)
        {
            TileBase tile = targetTilemap.GetTile(pos);
            if (tile != null && tile != tileConector)
            {
                targetTilemap.SetTile(pos, null);
                targetTilemap.SetTile(pos, tileConector);
            }
            yield return null;

        }
        DestroyExtraRoomConectors();
    }
    // Si hay más de un roomConector en un lugar entonces destruí a todos menos a uno. Para lograrlo utilizo el ID único de cada GameObject
    private void DestroyExtraRoomConectors()
    {
        int layerMask = LayerMask.GetMask("Default");
        Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position,layerMask);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("RoomConector"))
            {
                int myID = gameObject.GetInstanceID();
                int colliderID = collider.gameObject.GetInstanceID();
                if (myID<colliderID) Destroy(collider.gameObject);
                else if (myID!=colliderID)Destroy(gameObject); 
            }
        }
    }

    List<Tilemap> GetChildren(GameObject parent)
    {
        List<Tilemap> children = new List<Tilemap>();
        foreach (Transform child in parent.transform)
        {
            children.Add(child.gameObject.GetComponent<Tilemap>());
        }
        return children;
    }
    void DestroyThis()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
