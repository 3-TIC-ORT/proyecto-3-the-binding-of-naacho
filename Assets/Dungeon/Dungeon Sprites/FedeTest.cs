using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class FedeTest : MonoBehaviour
{
    private GameObject grid;
    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("Grid");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D col)
    {

        Vector3 collisionPoint = col.ClosestPoint(transform.position);
        Debug.Log(collisionPoint);
        List<Tilemap> gridChildren = GetChildren(grid);

        foreach (Tilemap tilemap in gridChildren)
        {
            Debug.Log(tilemap);
            Vector3Int gridCollisionPoint = tilemap.WorldToCell(collisionPoint);
            TileBase wall = tilemap.GetTile(gridCollisionPoint);
            if (wall != null)
            {
                tilemap.SetTile(gridCollisionPoint, null);
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
