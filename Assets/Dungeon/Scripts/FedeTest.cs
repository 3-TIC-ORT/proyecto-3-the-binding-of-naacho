using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class FedeTest : MonoBehaviour
{
    private GameObject grid;
    bool destroyedWalls = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.position += Vector3.right * 2;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.5f,0.5f),0);
        List<GameObject> walls = new List<GameObject>();
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Room"))
            {
                Debug.Log(collider.gameObject.name);
                walls.Add(collider.gameObject);
            }
            else if (collider.gameObject.CompareTag("SpawnPoint"))
            {
                Debug.Log(collider.gameObject.name);
            }
        }
    }

    List<GameObject> GetChildren(GameObject parent, bool filter, string tag)
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in parent.transform)
        {
            if (!filter)
            {
                children.Add(child.gameObject);
            }
            else if (filter && child.CompareTag(tag))
            {
                children.Add(child.gameObject);
            }
        }

        return children;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("holdapsalsd");

    }
}
