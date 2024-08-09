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
        Invoke("SpawnRoomConectors", 3);
    }

    void Spawn()
    {
        if (!spawned)
        {
            // Verificar si el espacio est� vac�o antes de instanciar
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.0001f);
            if (colliders.Length <= 1 && templates.roomsGenerated < templates.roomMin)
            {
                if (openingDirection == 1)
                {
                    int rand = Random.Range(0, templates.NTdownRooms.Length);
                    Instantiate(templates.NTdownRooms[rand], transform.position, Quaternion.identity, grid.transform);
                }
                else if (openingDirection == 2)
                {
                    int rand = Random.Range(0, templates.NTtopRooms.Length);
                    Instantiate(templates.NTtopRooms[rand], transform.position, Quaternion.identity, grid.transform);
                }
                else if (openingDirection == 3)
                {
                    int rand = Random.Range(0, templates.NTleftRooms.Length);
                    Instantiate(templates.NTleftRooms[rand], transform.position, Quaternion.identity, grid.transform);
                }
                else if (openingDirection == 4)
                {
                    int rand = Random.Range(0, templates.NTrightRooms.Length);
                    Instantiate(templates.NTrightRooms[rand], transform.position, Quaternion.identity, grid.transform);
                }
                spawned = true;
                templates.roomsGenerated++;
            }
            else if (colliders.Length <= 1 && templates.roomsGenerated < templates.roomsLimit)
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
                templates.roomsGenerated++;
            }
            else if(colliders.Length <= 1 && templates.roomsGenerated >= templates.roomsLimit)
            {
                if (openingDirection == 1)
                {
                    List<GameObject> downList = new List<GameObject>(templates.downRooms);
                    GameObject downRoom = downList.Find(go => go.name == "D");
                    Instantiate(downRoom, transform.position, Quaternion.identity, grid.transform);

                }
                else if (openingDirection == 2)
                {
                    List<GameObject> upList = new List<GameObject>(templates.topRooms);
                    GameObject upRoom = upList.Find(go => go.name == "T");
                    Instantiate(upRoom, transform.position, Quaternion.identity, grid.transform);
                }
                else if (openingDirection == 3)
                {
                    List<GameObject> leftList = new List<GameObject>(templates.leftRooms);
                    GameObject leftRoom = leftList.Find(go => go.name == "L");
                    Instantiate(leftRoom, transform.position, Quaternion.identity, grid.transform);
                }
                else if (openingDirection == 4)
                {
                    List<GameObject> rightList = new List<GameObject>(templates.rightRooms);
                    GameObject rightRoom = rightList.Find(go => go.name == "R");
                    Instantiate(rightRoom, transform.position, Quaternion.identity, grid.transform);
                }
                spawned = true;
                templates.roomsGenerated++;
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

    private void SpawnRoomConectors()
    {
        Instantiate(templates.roomConector, transform.position, Quaternion.identity);
    }
}
