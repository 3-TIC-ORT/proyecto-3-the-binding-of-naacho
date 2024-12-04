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
        randA = 1; // DESPU�S SACAR PARA QUE EXISTAN LOS ITEMS NORMALESSSSSSSSSSSSSSSSS #I%#")�%I$R)IR�)$FI$RF$)
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
        item = Instantiate(itemToSpawn, transform.position, Quaternion.identity, gameObject.transform);
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
        GameObject particle = ParticlesManager.Instance.onPickUpItemParticle;
        if (col.gameObject.CompareTag("Player") && item!=null)
        {
            Item itemComponent = item.GetComponent<Item>();
            itemComponent.onPickup();
            ParticlesManager.Instance.InstanceParticle(particle, transform.position, transform);
            GetComponent<ItemHolder>().enabled = false;
        }
    }
}
