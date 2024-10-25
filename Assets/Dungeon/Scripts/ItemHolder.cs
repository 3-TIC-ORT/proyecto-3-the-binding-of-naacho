using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    private RoomTemplates templates;
    private GameObject naacho;
    private GameObject item;
    private BoxCollider2D boxCollider2D;
    public float oclussionCulling;
    void Start()
    {
        naacho = GameObject.FindGameObjectWithTag("Player");
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        
        //print(templates.items[(int)Mathf.Floor(rand)].name);
        boxCollider2D = GetComponent<BoxCollider2D>();
        SpawnItem();
    }
    private void SpawnItem()
    {
        GameObject itemToSpawn;
        int randA = Random.Range(0, 2);
        if (randA == 0 || templates.specialItems.Count==0)
        {
            int randB = Random.Range(0, templates.items.Count * 2) / 2;
            itemToSpawn = templates.items[randB];
        }
        else
        {
            int randB = Random.Range(0, templates.specialItems.Count * 2) / 2;
            itemToSpawn = templates.specialItems[randB];
            templates.specialItems.Remove(itemToSpawn);

        }
        item = Instantiate(itemToSpawn, transform.position + Vector3.up * 0.5f, Quaternion.identity, gameObject.transform);
    }
    private void Update()
    {
        if (naacho != null) 
        {
            if ((naacho.transform.position - transform.position).magnitude < oclussionCulling) boxCollider2D.enabled = true;
            else boxCollider2D.enabled=false;
        }
        else if (!GameManager.Instance.nachoNullPrinted)
        {
            Debug.LogWarning("Che macho, Naacho es null");
            GameManager.Instance.nachoNullPrinted = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Item itemComponent = item.GetComponent<Item>();
            itemComponent.onPickup();
            GetComponent<ItemHolder>().enabled = false;
        }
    }
}
