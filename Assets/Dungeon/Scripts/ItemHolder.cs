using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    private RoomTemplates templates;
    private GameObject item;
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        int rand = Random.Range(0, templates.items.Length);
        item = Instantiate(templates.items[rand], transform.position, Quaternion.identity);
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
