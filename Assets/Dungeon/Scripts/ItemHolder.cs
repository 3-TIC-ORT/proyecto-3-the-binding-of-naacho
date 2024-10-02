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
        float rand = Random.Range(0, templates.items.Length*2)/2;
        item = Instantiate(templates.items[(int)Mathf.Floor(rand)], transform.position+Vector3.up*0.5f, Quaternion.identity,gameObject.transform);
        //print(templates.items[(int)Mathf.Floor(rand)].name);
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if ((naacho.transform.position - transform.position).magnitude < oclussionCulling) boxCollider2D.enabled = true;
        else boxCollider2D.enabled=false;
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
