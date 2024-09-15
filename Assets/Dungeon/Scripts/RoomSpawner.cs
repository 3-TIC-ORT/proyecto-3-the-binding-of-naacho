using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class RoomSpawner : MonoBehaviour
{
    private GameObject grid;
    public int openingDirection;
    // 1 Down door     2 Top door        3  Left door       4 Right door
    private RoomTemplates templates;
    private TilemapMerger merger;
    private Tilemap targetTilemap;

    private bool roomConectorsSpawned;
    public bool spawned = false;
    public bool spawnedClosedRoom=false;
    public bool bossRoom = false;
    public bool treasureRoom = false;

    public TileBase holeTile;
    public TileBase itemHolderTile;
    void Start()
    {
        grid = GameObject.Find("Grid");
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        merger = GameObject.FindGameObjectWithTag("Rooms").GetComponent<TilemapMerger>();
        targetTilemap = GameObject.Find("Entry Room").GetComponent<Tilemap>();
        Invoke("Spawn", 0.1f);
    }
    private void Update()
    {
        if (merger.tilemapsMerged && !roomConectorsSpawned)
        {
            if (!spawnedClosedRoom && !treasureRoom && !bossRoom)
            {
                roomConectorsSpawned = true;
                SpawnRoomConectors();
            }
        }
        if (bossRoom && merger.tilemapsMerged)
        {
            List<GameObject> enemiesPrefab = GetChildren(gameObject, false, "");
            if (enemiesPrefab.Count == 0) SpawnHole();
        }
        else if (treasureRoom && merger.tilemapsMerged)
        {
            SpawnItem();
        }
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
                    SpawnEnemies();
                }
                else if (openingDirection == 2)
                {
                    int rand = Random.Range(0, templates.NTtopRooms.Length);
                    Instantiate(templates.NTtopRooms[rand], transform.position, Quaternion.identity, grid.transform);
                    SpawnEnemies();
                }
                else if (openingDirection == 3)
                {
                    int rand = Random.Range(0, templates.NTleftRooms.Length);
                    Instantiate(templates.NTleftRooms[rand], transform.position, Quaternion.identity, grid.transform);
                    SpawnEnemies();
                }
                else if (openingDirection == 4)
                {
                    int rand = Random.Range(0, templates.NTrightRooms.Length);
                    Instantiate(templates.NTrightRooms[rand], transform.position, Quaternion.identity, grid.transform);
                    SpawnEnemies();
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
                    GameObject newRoom = Instantiate(templates.downRooms[rand], transform.position, Quaternion.identity, grid.transform);
                    SpawnSpecialRoom(rand,templates.downRooms,"D",false, newRoom);
                }
                else if (openingDirection == 2)
                {
                    int rand = Random.Range(0, templates.topRooms.Length);
                    GameObject newRoom = Instantiate(templates.topRooms[rand], transform.position, Quaternion.identity, grid.transform);
                    SpawnSpecialRoom(rand, templates.topRooms, "T",false, newRoom);
                }
                else if (openingDirection == 3)
                {
                    int rand = Random.Range(0, templates.leftRooms.Length);
                    GameObject newRoom = Instantiate(templates.leftRooms[rand], transform.position, Quaternion.identity, grid.transform);
                    SpawnSpecialRoom(rand, templates.leftRooms, "L",false, newRoom);
                }
                else if (openingDirection == 4)
                {
                    int rand = Random.Range(0, templates.rightRooms.Length);
                    GameObject newRoom = Instantiate(templates.rightRooms[rand], transform.position, Quaternion.identity, grid.transform);
                    SpawnSpecialRoom(rand, templates.rightRooms, "R", false, newRoom);
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
                    GameObject newRoom = Instantiate(downRoom, transform.position, Quaternion.identity, grid.transform);
                    SpawnSpecialRoom(1607, templates.downRooms, "D", true, newRoom);

                }
                else if (openingDirection == 2)
                {
                    List<GameObject> upList = new List<GameObject>(templates.topRooms);
                    GameObject upRoom = upList.Find(go => go.name == "T");
                    GameObject newRoom = Instantiate(upRoom, transform.position, Quaternion.identity, grid.transform);
                    SpawnSpecialRoom(1607, templates.downRooms, "D", true, newRoom);
                }
                else if (openingDirection == 3)
                {
                    List<GameObject> leftList = new List<GameObject>(templates.leftRooms);
                    GameObject leftRoom = leftList.Find(go => go.name == "L");
                    GameObject newRoom = Instantiate(leftRoom, transform.position, Quaternion.identity, grid.transform);
                    SpawnSpecialRoom(1607, templates.downRooms, "D", true, newRoom);
                }
                else if (openingDirection == 4)
                {
                    List<GameObject> rightList = new List<GameObject>(templates.rightRooms);
                    GameObject rightRoom = rightList.Find(go => go.name == "R");
                    GameObject newRoom = Instantiate(rightRoom, transform.position, Quaternion.identity, grid.transform);
                    SpawnSpecialRoom(1607, templates.downRooms, "D", true, newRoom);
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
                // Puede ocurrir que haya un spawnPoint spawned y que otros dos spawnPoints que se acaban de generar
                // hayan aparecido arriba del primero spawnPoint.
                bool noSpawnedOne = true;
                // Ver que no haya ningún spawnPoint que ya haya spawneado una room.
                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject.GetComponent<RoomSpawner>().spawned == true) noSpawnedOne = false;
                }
                if (noSpawnedOne)
                {
                    grid = GameObject.Find("Grid");
                    templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();

                    Instantiate(templates.closedRoom, transform.position, Quaternion.identity, grid.transform);
                    spawnedClosedRoom = true;
                    col.GetComponent<RoomSpawner>().spawnedClosedRoom = true;
                    col.GetComponent<RoomSpawner>().spawned = true;
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
        if (col.gameObject.CompareTag("RoomConector") && (spawnedClosedRoom || bossRoom || treasureRoom))
        {
            Destroy(col.gameObject);
        }
    }
    // Su nombre lo dice. Sin comentarios.
    public void SpawnEnemies()
    {
        int rand = Random.Range(0, templates.BasicEnemies.Length);
        Instantiate(templates.BasicEnemies[rand], transform.position, Quaternion.identity, transform);
    }

    // NO siempre crea una BossRoom
    public void SpawnSpecialRoom(int rand,GameObject[] rooms, string room, bool closing, GameObject newRoom)
    {
        if (closing && !templates.treasureRoomSpawned) SpawnTreasureRoom(newRoom);

        // closing es true cuando ya se superó el roomsLimit. Si por casualidad antes de esto no se creo ninguna room con una sola puerta
        // entonces seré la bossRoom.
        else if (closing && !templates.bossRoomSpawned) SpawnBossRoom(newRoom);

        else if (!templates.treasureRoomSpawned && rooms[rand].name == room) SpawnTreasureRoom(newRoom);

        // Si spawnee una room con una sola puerta y soy la primera en hacerlo entonces seré la bossRoom
        else if (!templates.bossRoomSpawned && rooms[rand].name == room && !bossRoom) SpawnBossRoom(newRoom);
        else SpawnEnemies();


    }
    public void SpawnTreasureRoom(GameObject room)
    {
        templates.treasureRoomSpawned = true;
        treasureRoom = true;
        Debug.Log("SOY LA TREASURE ROOM");
        GameObject.Find("TreasureRoomImage").GetComponent<Transform>().position = transform.position;
        // Como es una SpecialRoom, le cambiamos el color para que se vea extravagante
        SetLighting(true,room);
    }
    public void SpawnItem()
    {
        targetTilemap.SetTile(targetTilemap.WorldToCell(transform.position), itemHolderTile);
    }
    public void SpawnBossRoom(GameObject room)
    {
        SpawnEnemies();
        templates.bossRoomSpawned = true;
        bossRoom = true;
        Debug.Log("SOY LA BOSS ROOM");
        GameObject.Find("BossRoomImage").GetComponent<Transform>().position = transform.position;
        SetLighting(false,room);
    }

    private void SpawnHole()
    {
        targetTilemap.SetTile(targetTilemap.WorldToCell(transform.position), holeTile);
    }

    // Cambia la iluminación para los Treasure y Boss rooms
    private void SetLighting(bool forTreasureRoom, GameObject room)
    {
        Vector2 doorLightPosition = DecidePosition();
        Quaternion doorLightRotation = DecideRotation();

        GameObject doorLight = Instantiate(templates.doorLight, doorLightPosition, doorLightRotation,room.transform);
        List<GameObject> lightList = GetChildren(room, true, "RoomLight");
        foreach (GameObject _light in lightList)
        {
            Light2D lightComponent = _light.GetComponent<Light2D>();
            LightSettings settings = _light.GetComponent<LightSettings>();
            Color treasureRoomColor = settings.treasureRoomColor;
            Color bossRoomColor = settings.bossRoomColor;
            if (forTreasureRoom) lightComponent.color = treasureRoomColor;
            else lightComponent.color = bossRoomColor;
        }
    }
    // Las siguientes funciones son para establecer las doorLights
    private Vector2 DecidePosition()
    {
        // Esto calcula la distancia que hay entre el centro de una habitación y la puerta de la siguiente habitación
        float verticalDistance = (templates.verticalDoorToDoorRoomArea.y / 2 + templates.centerBetweenVerticaltalRooms)-0.5f;
        float horizontalDistance = (templates.horizontalDoorToDoorRoomArea.x / 2 + templates.centerBetweenHorizontalRooms)-0.5f;
        if (openingDirection == 1) return (Vector2)transform.position + Vector2.down * verticalDistance;
        else if (openingDirection==2) return (Vector2)transform.position + Vector2.up * verticalDistance;
        else if (openingDirection==3) return (Vector2)transform.position + Vector2.left * horizontalDistance;
        else return (Vector2)transform.position + Vector2.right * horizontalDistance;
    }
    private Quaternion DecideRotation()
    {
        if (openingDirection == 1) return Quaternion.Euler(0, 0, 180);
        else if (openingDirection == 2) return Quaternion.Euler(0, 0, 0);
        else if (openingDirection == 3) return Quaternion.Euler(0, 0, 90);
        else return Quaternion.Euler(0, 0, -90);
    }
    // Si no soy ni la bossRoom ni una closedRoom (no queremos conectarlas con habitaciones) entonces spawnea roomConectores
    private void SpawnRoomConectors()
    {
        if (!spawnedClosedRoom && !bossRoom && !treasureRoom) Instantiate(templates.roomConector, transform.position, Quaternion.identity);
    }
    // Devuelve la lista de hijos de un objeto. Podes ponerle un filtro por su Tag si queres.
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
