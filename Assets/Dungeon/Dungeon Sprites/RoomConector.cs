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


    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("SpawnPoint"))
        {
            if (pointDirection==1)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position-Vector2.down*5, 1);
                Vector3 collisionPoint = colliders[0].transform.position;
                List<Tilemap> gridChildren = GetChildren(grid);
                foreach (Tilemap tilemap in gridChildren)
                {
                    Vector3Int gridCollisionPoint = tilemap.WorldToCell(collisionPoint);
                    TileBase wall = tilemap.GetTile(gridCollisionPoint);
                    if (wall!=null)
                    {
                        tilemap.SetTile(gridCollisionPoint, null);
                    }
                }
            }
            else if (pointDirection==2)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position+Vector2.up * 5, 1);
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

            }
            else if (pointDirection == 3)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position - Vector2.left * 5, 1);
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

            }
            else if (pointDirection == 4)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position + Vector2.right * 5, 1);
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
