using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    private RoomTemplates templates;
    private GameObject naacho;
    private GameObject item;
    private BoxCollider2D boxCollider2D;
    public float oclussionCulling;
    public static List<ItemHolder> itemsHolders = new List<ItemHolder>();
    void Start()
    {
        naacho = GameObject.FindGameObjectWithTag("Player");
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        
        //print(templates.items[(int)Mathf.Floor(rand)].name);
        boxCollider2D = GetComponent<BoxCollider2D>();
        StartCoroutine(SpawnItem());
    }
    IEnumerator SpawnItem()
    {
        if (!itemsHolders.Contains(this))
        {
            itemsHolders.Add(this);
            yield return null;
            StartCoroutine(SpawnItem());
            yield break;
        }
        if (itemsHolders[0]!=this)
        {
            yield return null;
            SpawnItem();
            yield break;
        }

        //foreach (ItemHolder itemHolder in itemsHolders) 
        //{
        //    Debug.Log(itemHolder.GetInstanceID());
        //}
        itemsHolders.Remove(this);
        GameObject itemToSpawn;
        int randA = Random.Range(0, 2);
        //randA = 1; // DESPU�S SACAR PARA QUE EXISTAN LOS ITEMS NORMALESSSSSSSSSSSSSSSSS #I%#")�%I$R)IR�)$FI$RF$)
        if (randA == 0 || RoomTemplates.staticSpecialItems.Count==0)
        {
            int randB = Random.Range(0, RoomTemplates.staticNormalItems.Count);
            itemToSpawn = RoomTemplates.staticNormalItems[randB];
        }
        else
        {
            int randB = Random.Range(0, RoomTemplates.staticSpecialItems.Count);
            itemToSpawn = RoomTemplates.staticSpecialItems[randB];
            RoomTemplates.staticSpecialItems.Remove(itemToSpawn);
        }
        item = Instantiate(itemToSpawn, transform.position, Quaternion.identity, gameObject.transform);
        GameObject[] allIncompatibleItems = item.GetComponent<Item>().allIncompatibleItems;
        foreach (GameObject incompatibleItem in allIncompatibleItems)
        {
            RoomTemplates.staticNormalItems.RemoveAll(x => x.name == incompatibleItem.name);
            RoomTemplates.staticSpecialItems.RemoveAll(x => x.name == incompatibleItem.name);
        }
        Debug.Log($"{item.name} spawneado en {GameManager.cronometer} segundos por el itemHolder {GetInstanceID()}");
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
            RoomTemplates.itemsTakenNames.Add(item.name);
            ParticlesManager.Instance.InstanceParticle(particle, transform.position, transform);
            GetComponent<ItemHolder>().enabled = false;
        }
    }
}
