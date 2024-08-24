using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    private GameObject grid;
    public int openingDirection;
    // 1 Down door     2 Top door        3  Left door       4 Right door
    private RoomTemplates templates;
    public bool spawned = false;
    public bool spawnedClosedRoom=false;
    public bool bossRoom = false;
    public bool treasureRoom = false;
    void Start()
    {
        grid = GameObject.Find("Grid");
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
        StartCoroutine(WaitForSpawnRoomConectors());
    }

    void Spawn()
    {
        if (!spawned)
        {
            // Verificar si el espacio está vacío antes de instanciar
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.0001f);
            if (colliders.Length <= 1 && templates.roomsGenerated < templates.roomsMin)
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
            // La primera habitación de cierre (D, T, R, L) será la BossRoom
            else if (colliders.Length <= 1 && templates.roomsGenerated < templates.roomsLimit)
            {
                if (openingDirection == 1)
                {
                    int rand = Random.Range(0, templates.downRooms.Length);
                    Instantiate(templates.downRooms[rand], transform.position, Quaternion.identity, grid.transform);
                    SpawnSpecialRoom(rand,templates.downRooms,"D",false);
                }
                else if (openingDirection == 2)
                {
                    int rand = Random.Range(0, templates.topRooms.Length);
                    Instantiate(templates.topRooms[rand], transform.position, Quaternion.identity, grid.transform);
                    SpawnSpecialRoom(rand, templates.topRooms, "T",false);
                }
                else if (openingDirection == 3)
                {
                    int rand = Random.Range(0, templates.leftRooms.Length);
                    Instantiate(templates.leftRooms[rand], transform.position, Quaternion.identity, grid.transform);
                    SpawnSpecialRoom(rand, templates.leftRooms, "L",false);
                }
                else if (openingDirection == 4)
                {
                    int rand = Random.Range(0, templates.rightRooms.Length);
                    Instantiate(templates.rightRooms[rand], transform.position, Quaternion.identity, grid.transform);
                    SpawnSpecialRoom(rand, templates.rightRooms, "R", false);
                }
                spawned = true;
                templates.roomsGenerated++;
            }
            // El primer argumento de SpawnBossRoom es el cumpleaños de Feli, es así para que el primer if de la función de false.
            else if(colliders.Length <= 1 && templates.roomsGenerated >= templates.roomsLimit)
            {
                if (openingDirection == 1)
                {
                    List<GameObject> downList = new List<GameObject>(templates.downRooms);
                    // Expresión lambda. go es como la variable de i en un for. Se transforma en cada elemento de la lista y devuelve
                    // al elemento que cumpla la condición.
                    GameObject downRoom = downList.Find(go => go.name == "D");
                    Instantiate(downRoom, transform.position, Quaternion.identity, grid.transform);
                    SpawnSpecialRoom(1607, templates.downRooms, "D", true);

                }
                else if (openingDirection == 2)
                {
                    List<GameObject> upList = new List<GameObject>(templates.topRooms);
                    GameObject upRoom = upList.Find(go => go.name == "T");
                    Instantiate(upRoom, transform.position, Quaternion.identity, grid.transform);
                    SpawnSpecialRoom(1607, templates.downRooms, "D", true);
                }
                else if (openingDirection == 3)
                {
                    List<GameObject> leftList = new List<GameObject>(templates.leftRooms);
                    GameObject leftRoom = leftList.Find(go => go.name == "L");
                    Instantiate(leftRoom, transform.position, Quaternion.identity, grid.transform);
                    SpawnSpecialRoom(1607, templates.downRooms, "D", true);
                }
                else if (openingDirection == 4)
                {
                    List<GameObject> rightList = new List<GameObject>(templates.rightRooms);
                    GameObject rightRoom = rightList.Find(go => go.name == "R");
                    Instantiate(rightRoom, transform.position, Quaternion.identity, grid.transform);
                    SpawnSpecialRoom(1607, templates.downRooms, "D", true);
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
            bool colIsSpawned = col.GetComponent<RoomSpawner>().spawned;
            Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position);

            // Si hay dos spawnPoints sin haber spawneado ninguna room
            if (!colIsSpawned && !spawned)
            {
                // Si solo son spawnPoints, spawnea la closedRoom
                if (colliders.Length <= 2)
                {
                    grid = GameObject.Find("Grid");
                    templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();

                    Instantiate(templates.closedRoom, transform.position, Quaternion.identity, grid.transform);
                    spawnedClosedRoom = true;
                    col.GetComponent<RoomSpawner>().spawnedClosedRoom = true;
                    col.GetComponent<RoomSpawner>().spawned = true;
                }
                else
                {
                    // Puede ocurrir que haya un spawnPoint spawned y que otros dos spawnPoints que se acaban de generar
                    // hayan aparecido arriba del primero spawnPoint.
                    bool noSpawnedOne = true;
                    // Ver que no haya ningún spawnPoint que ya haya spawneado una room.
                    foreach (Collider2D collider in colliders)
                    {
                        if (collider.gameObject.GetComponent<RoomSpawner>().spawned == true) noSpawnedOne = false;
                    }
                    if (noSpawnedOne == true)
                    {
                        Debug.Log("PRUEBA FEDE FUNCIONA");
                        grid = GameObject.Find("Grid");
                        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();

                        Instantiate(templates.closedRoom, transform.position, Quaternion.identity, grid.transform);
                        spawnedClosedRoom = true;
                        col.GetComponent<RoomSpawner>().spawned = true;
                    }
                }

            }
            // Si soy un closedRoom y me aparece un spawnPoint entonces que no haga nada
            else if (spawnedClosedRoom && !colIsSpawned)
            {
                col.GetComponent<RoomSpawner>().spawnedClosedRoom = true;
                col.GetComponent<RoomSpawner>().spawned = true;
            }
            // Si soy la bossRoom y me aparece un spawnPoint destruilo para que no joda. Así no puede hacer aparecer roomConectores
            else if (bossRoom && !colIsSpawned) Destroy(col.gameObject);
            else if (treasureRoom && !colIsSpawned) Destroy(col.gameObject);
            // Si soy un spawnPoint normal que toco a otro spawnPoint que no spawneo entonces su spawned va a ser true
            else col.GetComponent<RoomSpawner>().spawned = true;
        }
        // Si toco a  un roomConector y soy una closedRoom le seteo su doorsDestroyed y spawnPointMoved a true para que no hada nada más
        if (col.gameObject.CompareTag("RoomConector") && spawnedClosedRoom)
        {
            col.GetComponent<RoomConector>().doorsDestroyed = true;
            col.GetComponent<RoomConector>().spawnPointMoved = true;
        }
    }

    // NO siempre crea una BossRoom
    public void SpawnSpecialRoom(int rand,GameObject[] rooms, string room, bool closing)
    {
        if (closing && !templates.treasureRoomSpawned) SpawnTreasureRoom();

        // closing es true cuando ya se superó el roomsLimit. Si por casualidad antes de esto no se creo ninguna room con una sola puerta
        // entonces seré la bossRoom.
        else if (closing && !templates.bossRoomSpawned) SpawnBossRoom();

        else if (!templates.treasureRoomSpawned && rooms[rand].name == room) SpawnTreasureRoom();

        // Si spawnee una room con una sola puerta y soy la primera en hacerlo entonces seré la bossRoom
        else if (!templates.bossRoomSpawned && rooms[rand].name == room && !bossRoom) SpawnBossRoom();

        
    }
    public void SpawnTreasureRoom()
    {
        templates.treasureRoomSpawned = true;
        treasureRoom = true;
        Debug.Log("SOY LA TREASURE ROOM");
        GameObject.Find("TreasureRoomImage").GetComponent<Transform>().position = transform.position;
    }
    public void SpawnBossRoom()
    {
        templates.bossRoomSpawned = true;
        bossRoom = true;
        Debug.Log("SOY LA BOSS ROOM");
        GameObject.Find("BossRoomImage").GetComponent<Transform>().position = transform.position;
    }
    // Espera a que no se generen más rooms para llamar a los roomConectors
    IEnumerator WaitForSpawnRoomConectors()
    {
        int localRoomsGenerated = templates.roomsGenerated;
        int lastLocalRoomsGenerated = -1;
        while (lastLocalRoomsGenerated != localRoomsGenerated)
        {
            lastLocalRoomsGenerated = localRoomsGenerated;
            yield return new WaitForSecondsRealtime(2f);
            localRoomsGenerated = templates.roomsGenerated;
        }
        if (spawnedClosedRoom) StopCoroutine(WaitForSpawnRoomConectors());
        yield return new WaitForSecondsRealtime(5f);
        if (templates.minCompleted) SpawnRoomConectors();
        else StartCoroutine(WaitForSpawnRoomConectors());
    }
    // Si no soy ni la bossRoom ni una closedRoom (no queremos conectarlas con habitaciones) entonces spawnea roomConectores
    private void SpawnRoomConectors()
    {
        if (!spawnedClosedRoom && !bossRoom && !treasureRoom) Instantiate(templates.roomConector, transform.position, Quaternion.identity);
    }
}
