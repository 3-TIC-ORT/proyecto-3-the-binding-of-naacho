using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class RoomConector : MonoBehaviour
{
    public int pointDirection;
    private GameObject grid;
    public TileBase tileConector;
    private Tilemap targetTilemap;
    public bool doorsDestroyed=false;
    public bool spawnPointMoved=false;
    public bool colisioneperonosecumplio = false;
    void Start()
    {
        grid = GameObject.Find("Grid");
        targetTilemap=GameObject.Find("Entry Room").GetComponent<Tilemap>();
        Invoke("DestroyThis", 3000);
    }


    private void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.CompareTag("SpawnPoint") && col.gameObject.GetComponent<RoomSpawner>().spawnedClosedRoom == false && !col.gameObject.GetComponent<RoomSpawner>().bossRoom && !spawnPointMoved)
        {
            if (pointDirection == 1) transform.position += (Vector3)(Vector2.down * 10);
            else if (pointDirection == 2) transform.position += (Vector3)(Vector2.up * 10);
            else if (pointDirection == 3) transform.position += (Vector3)(Vector2.left * 13);
            else if (pointDirection == 4) transform.position += (Vector3)(Vector2.right * 13);

            spawnPointMoved = true;
        }
        else if (spawnPointMoved && !doorsDestroyed && col.name != "Closed(Closed)" && col.gameObject.CompareTag("Room"))
        {
            ConnectRooms(col);
        }
        else
        {
            Vector3Int gridPoint = targetTilemap.WorldToCell(transform.position);
            TileBase tile = targetTilemap.GetTile(gridPoint);
            if (tile!=null && tile!=tileConector && col.name != "Closed(Closed)" && spawnPointMoved && !doorsDestroyed)
            {
                ConnectRooms(col);
            }
        }
    }
    private void ConnectRooms(Collider2D col)
    {
        StartCoroutine(esperar());



    }
    IEnumerator esperar()
    {
        doorsDestroyed = true;
        List<Vector2> movimientosPeristalticos = new List<Vector2>();
        if (pointDirection == 1 || pointDirection == 2)
        {

            movimientosPeristalticos = new List<Vector2>(CreateMovimientosPeristalticos(11, 13));
            movimientosPeristalticos.Insert(0, Vector2.down * 5.5f);
            movimientosPeristalticos.Insert(1, Vector2.left * 6.5f);
        }
        else if (pointDirection == 3 || pointDirection == 4)
        {
            movimientosPeristalticos = new List<Vector2>(CreateMovimientosPeristalticos(7, 11));
            movimientosPeristalticos.Insert(0, Vector2.down * 3.5f);
            movimientosPeristalticos.Insert(1, Vector2.left * 5.5f);
        }


        foreach (Vector2 mov in movimientosPeristalticos)
        {
            //Collider2D collider = Physics2D.OverlapBox(transform.position, new Vector2(0.1f, 0.1f), 0);
            //Vector3 collisionPoint = collider.ClosestPoint(transform.position);
            //Tilemap tilemap = collider.GetComponent<Tilemap>();
            //Vector3Int gridCollisionPoint = tilemap.WorldToCell(collisionPoint);
            //TileBase wall = tilemap.GetTile(gridCollisionPoint);
            Vector3Int gridPoint = targetTilemap.WorldToCell(transform.position);
            TileBase tile = targetTilemap.GetTile(gridPoint);
            bool isConnected = false;
            if (tile != null && tile != tileConector && !isConnected)
            {
                targetTilemap.SetTile(gridPoint, null);
                targetTilemap.SetTile(gridPoint, tileConector);
            }
            else if (tile == tileConector)
            {
                isConnected = true;
            }
            transform.position += (Vector3)mov;
            yield return new WaitForSecondsRealtime(0.05f);

        }
    }

    List<Vector2> CreateMovimientosPeristalticos(int repeticiones, int pasos)
    {
        List<Vector2> movimientos = new List<Vector2>();
        for (int i = 0; i < repeticiones+1; i++)
        {
            for (int j = 0; j < pasos; j++)
            {
                movimientos.Add(Vector2.right);
            }
            movimientos.Add(Vector2.left*pasos);
            movimientos.Add(Vector2.up);
        }
        return movimientos;
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
