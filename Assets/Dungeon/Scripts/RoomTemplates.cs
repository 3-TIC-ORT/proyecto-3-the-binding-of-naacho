using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
public class RoomTemplates : MonoBehaviour
{
    public int roomsGenerated = 0;
    public int roomsLimit; // No es preciso
    public int roomsMin;
    public bool minCompleted=false;
    public int treasureRoomsAmount;
    public int currentTreasureRooms=0;
    // Su nombre debería ser treasureRoomsSpawned (notar el plural de Rooms). 
    public bool treasureRoomSpawned = false;
    public bool bossRoomSpawned=false;
    private GameObject grid;
    [Header("Room List")]
    public GameObject[] downRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject closedRoom;
    [Header("Not Closing Room List")]
    public GameObject[] NTdownRooms;
    public GameObject[] NTtopRooms;
    public GameObject[] NTleftRooms;
    public GameObject[] NTrightRooms;
    public GameObject NTclosedRoom;
    [Header("Room Stuff")]
    public GameObject roomIcon;
    [Header("Enemies")]
    public GameObject[] BasicEnemies;
    [Header("Jefes")]
    public GameObject[] Jefes;
    [Header("Lights")]
    public GameObject doorLight;
    [Header("Items")]
    public GameObject itemHolder;
    public List<GameObject> items;
    public List<GameObject> specialItems;
    [Header("Other")]
    public GameObject roomConector;
    public GameObject LWithSpawnPoint;
    [Tooltip
    (
        "Es el área entre la puerta de una room y la otra, siempre que ambas estén horizontalmente alineadas. " +
        "No modifica su volumen al cambiarlo, esto es para los roomConectors. No cambiar si no se van a cambiar las rooms a mano. " 
    )]
    public Vector3Int horizontalDoorToDoorRoomArea;
    [Tooltip
    (
        "Es el área entre la puerta de una room y la otra, siempre que ambas estén verticalmente alineadas. " +
        "No modifica su volumen al cambiarlo, esto es para los roomConectors. No cambiar si no se van a cambiar las rooms a mano. "
    )]
    public Vector3Int verticalDoorToDoorRoomArea;
    // La mitad de la longitud entre una room y otra, basicamente.
    public float centerBetweenHorizontalRooms;
    public float centerBetweenVerticaltalRooms;
    public Vector2 insideRoomArea;
    
    void Start()
    {
        grid = GameObject.Find("Grid");
        StartCoroutine(PreventClosing());
    }
    // Se fija que cuando no se generan más rooms que la roomMin haya sido respetado.
    IEnumerator PreventClosing()
    {
        int lastRoomsGenerated = -1;
        while (lastRoomsGenerated != roomsGenerated)
        {
            lastRoomsGenerated = roomsGenerated;
            yield return new WaitForSecondsRealtime(1.5f);
        }
        if (roomsGenerated <= roomsMin)
        {
            CreateRoom();
        }
        // Si la generación de mazmorras paró y no se creo la treasure o la boss room, reinicia la escena. 
        // Perdón, pero fue vencido por este problema :'v
        else if (!treasureRoomSpawned || !bossRoomSpawned)
        {
            SceneManager.LoadScene("Mazmorras testing");
        }
        else minCompleted = true;
        yield return new WaitForSecondsRealtime(1f);
    }
    // Si la mazmorra se autocerró, cree una room en el tilemap más a la izquierda existente.
    private void CreateRoom()
    {
        Debug.Log("Se creo una room porque la mazmorra se cerró sola :v");
        List<GameObject> tilemaps = GetChildren(grid, false,"");
        // Coordenada X más a la izquierda (Por defecto es 1607, el cumple de Felipe ¿Daniel? Doval Ferrari<3)
        float lefterX = 1607;
        // Vector 2 más a la izquierda (es redundante con lo de arriba, ya se)
        Vector2 lefterRoomPosition=Vector2.zero;
        foreach (GameObject tilemap in tilemaps)
        {
            // Agarrá todos los spawnPoints del tilemap iterado (podría cambiarle el nombre pero ya estoy comentando el código, y no de manera placentera)
            List<GameObject> roomSpawners = GetChildren(tilemap, true, "SpawnPoint");
            foreach (GameObject roomSpawnerGameObject in roomSpawners)
            {
                // Componente roomSpawner de cada SpawnPoint
                RoomSpawner roomSpawner = roomSpawnerGameObject.GetComponent<RoomSpawner>();
                // No queremos crear una habitación al lado de una bossRoom o Treasure room (solo se conectan a otra única habitación) 
                if (!roomSpawner.spawnedClosedRoom && !roomSpawner.treasureRoom && !roomSpawner.bossRoom)
                {
                    // X del spawnPoint
                    Vector2 roomSpawnerGameObjectPosition = roomSpawnerGameObject.GetComponent<Transform>().position;
                    // Si es el que está más a la izquierda hasta ahora, entonces cambia el valor de lefterRoomPosition
                    if (roomSpawnerGameObjectPosition.x < lefterX)
                    {
                        lefterX = roomSpawnerGameObjectPosition.x;
                        lefterRoomPosition = roomSpawnerGameObjectPosition;
                    }
                }
            }
            
        }
        List<GameObject> listLeftRooms = new List<GameObject>(leftRooms);
        // Instanciá una nueva habitación a la derecha de la habitaicón más a la izquierda.
        Instantiate(LWithSpawnPoint, lefterRoomPosition + (Vector2.left*26), Quaternion.identity,grid.transform);
        StartCoroutine(PreventClosing());
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
            else if (filter &&  child.CompareTag(tag))
            {
                children.Add(child.gameObject);
            }
        }
        return children;
    }
}
