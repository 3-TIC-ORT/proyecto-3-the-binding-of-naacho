using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfNotChildren : MonoBehaviour
{
    void Update()
    {
         if (GetChildren(gameObject,false,"").Count==0)
        {
            Destroy(gameObject);
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
}
