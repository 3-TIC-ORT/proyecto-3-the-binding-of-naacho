using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    private GameObject grid;
    public int openingDirection;
    // 1 Down door     2 Top door        3  Left door       4 Right door

    private RoomTemplates templates;
    public bool spawned = false;
    void Start()
    {
        grid = GameObject.Find("Grid");
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }

    void Spawn()
    {
        if (!spawned)
        {
            // Verificar si el espacio está vacío antes de instanciar
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.0001f);
            if (colliders.Length <=1)
            {
                if (openingDirection == 1)
                {
                    int rand = Random.Range(0, templates.downRooms.Length);
                    Instantiate(templates.downRooms[rand], transform.position, Quaternion.identity, grid.transform);
                }
                else if (openingDirection == 2)
                {
                    int rand = Random.Range(0, templates.topRooms.Length);
                    Instantiate(templates.topRooms[rand], transform.position, Quaternion.identity, grid.transform);
                }
                else if (openingDirection == 3)
                {
                    int rand = Random.Range(0, templates.leftRooms.Length);
                    Instantiate(templates.leftRooms[rand], transform.position, Quaternion.identity, grid.transform);
                }
                else if (openingDirection == 4)
                {
                    int rand = Random.Range(0, templates.rightRooms.Length);
                    Instantiate(templates.rightRooms[rand], transform.position, Quaternion.identity, grid.transform);
                }
                spawned = true;
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("SpawnPoint"))
        {
            if (col.GetComponent<RoomSpawner>().spawned==false && !spawned)
            {
                Instantiate(templates.closedRoom,transform.position,Quaternion.identity, grid.transform);
            }
            spawned=true;
        }
    }
}
