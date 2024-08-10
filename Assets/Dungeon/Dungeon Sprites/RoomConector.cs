using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class RoomConector : MonoBehaviour
{
    public int pointDirection;
    private GameObject grid;
    public TileBase tileConector;
    bool doorsDestroyed=false;
    bool spawnPointMoved=false;
    void Start()
    {
        grid = GameObject.Find("Grid");
        Invoke("DestroyThis", 2);
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("SpawnPoint") && col.gameObject.GetComponent<RoomSpawner>().spawnedClosedRoom==false && !spawnPointMoved)
        {
            if (pointDirection == 1) transform.position += (Vector3)(Vector2.down * 5);
            else if (pointDirection == 2) transform.position += (Vector3)(Vector2.up * 5);
            else if (pointDirection == 3) transform.position += (Vector3)(Vector2.left * 5);
            else if (pointDirection == 4) transform.position += (Vector3)(Vector2.right * 5);
            spawnPointMoved = true;
        }
        else if (!col.gameObject.CompareTag("SpawnPoint") && !col.gameObject.CompareTag("RoomConector") && !doorsDestroyed)
        {
            ConnectRooms(col);
        }
    }
    private void ConnectRooms(Collider2D col)
    {
        doorsDestroyed = true;
        Vector2[] movimientosPeristalticosArray = { Vector2.right, Vector2.up, Vector2.left, Vector2.left, Vector2.down, Vector2.down, Vector2.right, Vector2.right };
        List<Vector2> movimientosPeristalticos = new List<Vector2>(movimientosPeristalticosArray);
        foreach (Vector2 mov in movimientosPeristalticos)
        {
            Vector3 collisionPoint = col.ClosestPoint(transform.position);
            Tilemap tilemap = col.GetComponent<Tilemap>();
            Vector3Int gridCollisionPoint = tilemap.WorldToCell(collisionPoint);
            TileBase wall = tilemap.GetTile(gridCollisionPoint);
            if (wall != null)
            {
                tilemap.SetTile(gridCollisionPoint, null);
                tilemap.SetTile(gridCollisionPoint, tileConector);
            }
            transform.position += (Vector3)mov * 0.37f;
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
