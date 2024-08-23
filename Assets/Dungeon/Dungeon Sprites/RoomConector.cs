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
    private Tilemap targetTilemap;
    public bool doorsDestroyed=false;
    public bool spawnPointMoved=false;
    public bool colisioneperonosecumplio = false;
    void Start()
    {
        grid = GameObject.Find("Grid");
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        targetTilemap=GameObject.Find("Entry Room").GetComponent<Tilemap>();
        // FEDE CAMBIALO DESPUÉS, NO TE OLVIDES ##############################################################
        // FEDE CAMBIALO DESPUÉS, NO TE OLVIDES ##############################################################
        Invoke("DestroyThis", 3000);
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        // Si colisiono con un spawnPoint que no sea una closedRoom, una bossRoom o una treasureRoom entonces me voy a mover a mi dirección respectiva.
        if (col.gameObject.CompareTag("SpawnPoint") && col.gameObject.GetComponent<RoomSpawner>().spawnedClosedRoom == false && !col.gameObject.GetComponent<RoomSpawner>().bossRoom && !col.gameObject.GetComponent<RoomSpawner>().treasureRoom && !spawnPointMoved)
        {
            // Se posiciona el roomConector en el centro del área de las dos habitaciones que debería ser borrada.
            if (pointDirection == 1) transform.position += (Vector3)(Vector2.down * templates.centerBetweenVerticaltalRooms);
            else if (pointDirection == 2) transform.position += (Vector3)(Vector2.up * templates.centerBetweenVerticaltalRooms);
            else if (pointDirection == 3) transform.position += (Vector3)(Vector2.left * templates.centerBetweenHorizontalRooms);
            else if (pointDirection == 4) transform.position += (Vector3)(Vector2.right * templates.centerBetweenHorizontalRooms);
            targetTilemap.SetTile(targetTilemap.WorldToCell(transform.position), tileConector);
            spawnPointMoved = true;
        }
        // Una vez que me moví, si colisiono con una pared de Room entonces voy a empezar a destruirla
        // ACLARACIÓN: Por la forma en la que lo hice, los roomConectors solo detectan los extremos o bordes de los tiles al colisionar#####
        else if (spawnPointMoved && !doorsDestroyed && col.name != "Closed(Closed)" && col.gameObject.CompareTag("Room"))
        {
            ConnectRooms(col);
        }
        
    
    }
    private void ConnectRooms(Collider2D col)
    {
        StartCoroutine(esperar());

    }
    IEnumerator esperar()
    {
        doorsDestroyed = true;
        // Área de int, es decir, dos esquinas.
        BoundsInt area;
        if (pointDirection == 1 || pointDirection == 2)
        {
            // Los valores de las esquinas encierran un área que es el que se necesita para unir las rooms haciendo una más grande
            area = new BoundsInt
            (
                // Esquina 1. Se divide por -2 para que una esquina esté en la esquina inferior izquierda, al principio este roomConector
                // está en el centro del área.
                templates.verticalDoorToDoorRoomArea/-2 + targetTilemap.WorldToCell(transform.position),
                // Esquina 2. Es lo mismo que el área.
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
