using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class RoomConector : MonoBehaviour
{
    public int pointDirection;
    private GameObject grid;
    
    void Start()
    {
        grid = GameObject.Find("Grid");
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("SpawnPoint"))
        {
            ConnectRooms();
        }
    }
    private void ConnectRooms()
    {
        Debug.Log("DKSADKAS");
        if (pointDirection == 1)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll((Vector2)transform.position + Vector2.down * 5, new Vector2(1,1),0);
            List<Collider2D> collidersList = new List<Collider2D>();
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Room"))
                {
                    collidersList.Add(collider);
                }
            }
            while (collidersList.Count > 0)
            {
                Vector3 collisionPoint = colliders[0].transform.position;
      
                List<Tilemap> gridChildren = GetChildren(grid);
                foreach (Tilemap tilemap in gridChildren)
                {
                    Vector3Int gridCollisionPoint = tilemap.WorldToCell(collisionPoint);
                    TileBase wall = tilemap.GetTile(gridCollisionPoint);
                    if (wall != null)
                    {
                        tilemap.SetTile(gridCollisionPoint, null);
                    }
                    
                }
                colliders = Physics2D.OverlapBoxAll((Vector2)transform.position + Vector2.down * 5, new Vector2(1, 1), 0);
                collidersList = new List<Collider2D>();
                foreach (Collider2D collider in colliders)
                {
                    if (collider.CompareTag("Room"))
                    {
                        collidersList.Add(collider);
                    }
                }

            }

        }
        else if (pointDirection == 2)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll((Vector2)transform.position + Vector2.up * 5, new Vector2(1, 1), 0);
            List<Collider2D> collidersList = new List<Collider2D>();
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Room"))
                {
                    collidersList.Add(collider);
                }
            }
            while (collidersList.Count > 0)
            {
                Vector3 collisionPoint = colliders[0].transform.position;

                List<Tilemap> gridChildren = GetChildren(grid);
                foreach (Tilemap tilemap in gridChildren)
                {
                    Vector3Int gridCollisionPoint = tilemap.WorldToCell(collisionPoint);
                    TileBase wall = tilemap.GetTile(gridCollisionPoint);
                    if (wall != null)
                    {
                        tilemap.SetTile(gridCollisionPoint, null);
                    }

                }
                colliders = Physics2D.OverlapBoxAll((Vector2)transform.position + Vector2.up * 5, new Vector2(1, 1), 0);
                collidersList = new List<Collider2D>();
                foreach (Collider2D collider in colliders)
                {
                    if (collider.CompareTag("Room"))
                    {
                        collidersList.Add(collider);
                    }
                }

            }

        }
        else if (pointDirection == 3)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll((Vector2)transform.position + Vector2.left * 5, new Vector2(1, 1), 0);
            List<Collider2D> collidersList = new List<Collider2D>();
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Room"))
                {
                    collidersList.Add(collider);
                }
            }
            while (collidersList.Count > 0)
            {
                Vector3 collisionPoint = colliders[0].transform.position;

                List<Tilemap> gridChildren = GetChildren(grid);
                foreach (Tilemap tilemap in gridChildren)
                {
                    Vector3Int gridCollisionPoint = tilemap.WorldToCell(collisionPoint);
                    TileBase wall = tilemap.GetTile(gridCollisionPoint);
                    if (wall != null)
                    {
                        tilemap.SetTile(gridCollisionPoint, null);
                    }

                }
                colliders = Physics2D.OverlapBoxAll((Vector2)transform.position + Vector2.left * 5, new Vector2(1, 1), 0);
                collidersList = new List<Collider2D>();
                foreach (Collider2D collider in colliders)
                {
                    if (collider.CompareTag("Room"))
                    {
                        collidersList.Add(collider);
                    }
                }

            }

        }
        else if (pointDirection == 4)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll((Vector2)transform.position + Vector2.right * 5, new Vector2(1, 1), 0);
            List<Collider2D> collidersList = new List<Collider2D>();
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Room"))
                {
                    collidersList.Add(collider);
                }
            }
            while (collidersList.Count > 0)
            {
                Vector3 collisionPoint = colliders[0].transform.position;

                List<Tilemap> gridChildren = GetChildren(grid);
                foreach (Tilemap tilemap in gridChildren)
                {
                    Vector3Int gridCollisionPoint = tilemap.WorldToCell(collisionPoint);
                    TileBase wall = tilemap.GetTile(gridCollisionPoint);
                    if (wall != null)
                    {
                        tilemap.SetTile(gridCollisionPoint, null);
                    }

                }
                colliders = Physics2D.OverlapBoxAll((Vector2)transform.position + Vector2.right * 5, new Vector2(1, 1), 0);
                collidersList = new List<Collider2D>();
                foreach (Collider2D collider in colliders)
                {
                    if (collider.CompareTag("Room"))
                    {
                        collidersList.Add(collider);
                    }
                }

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
}
