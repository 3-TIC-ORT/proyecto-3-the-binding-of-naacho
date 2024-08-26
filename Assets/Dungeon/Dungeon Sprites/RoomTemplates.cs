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
    [Header("Other")]
    public GameObject roomConector;
    public GameObject lWithSpawnPoint;
    [Tooltip
    (
        "Es el �rea entre la puerta de una room y la otra, siempre que ambas est�n horizontalmente alineadas. " +
        "No modifica su volumen al cambiarlo, esto es para los roomConectors. No cambiar si no se van a cambiar las rooms a mano. " 
    )]
    public Vector3Int horizontalDoorToDoorRoomArea;
    [Tooltip
    (
        "Es el �rea entre la puerta de una room y la otra, siempre que ambas est�n verticalmente alineadas. " +
        "No modifica su volumen al cambiarlo, esto es para los roomConectors. No cambiar si no se van a cambiar las rooms a mano. "
    )]
    public Vector3Int verticalDoorToDoorRoomArea;
    // La mitad de la longitud entre una room y otra, basicamente.

    public float centerBetweenHorizontalRooms;
    public float centerBetweenVerticaltalRooms;
    void Start()
    {
        grid = GameObject.Find("Grid");
        StartCoroutine(PreventClosing());
    }
    // Se fija que cuando no se generan m�s rooms que la roomMin haya sido respetado.
    IEnumerator PreventClosing()
    {
        int lastRoomsGenerated = -1;
        while (lastRoomsGenerated != roomsGenerated)
        {
            lastRoomsGenerated = roomsGenerated;
            yield return new WaitForSecondsRealtime(2.5f);
        }
        if (roomsGenerated <= roomsMin)
        {
            CreateRoom();
        }
        else if (roomsGenerated >= roomsMin && (!treasureRoomSpawned || !bossRoomSpawned))
        {
            SceneManager.LoadScene("Mazmorras testing");
        }
        else minCompleted = true;
        yield return new WaitForSecondsRealtime(1f);
    }
    // Si la mazmorra se autocerr�, cree una room en el tilemap m�s a la izquierda existente.
    private void CreateRoom()
    {
        Debug.Log("Se creo una room porque la mazmorra se cerr� sola :v");
        List<GameObject> tilemaps = GetChildren(grid);
        float lefterX = 1607;
        Vector2 lefterRoomPosition=Vector2.zero;
        foreach (GameObject tilemap in tilemaps)
        {
            List<GameObject> roomSpawners = GetChildren(tilemap);
            foreach (GameObject roomSpawnerGameObject in roomSpawners)
            {
                RoomSpawner roomSpawner = roomSpawnerGameObject.GetComponent<RoomSpawner>();
                if (!roomSpawner.spawnedClosedRoom && !roomSpawner.treasureRoom && !roomSpawner.bossRoom)
                {
                    Vector2 roomSpawnerGameObjectPosition = roomSpawnerGameObject.GetComponent<Transform>().position;
                    if (roomSpawnerGameObjectPosition.x < lefterX)
                    {
                        lefterX = roomSpawnerGameObjectPosition.x;
                        lefterRoomPosition = roomSpawnerGameObjectPosition;
                    }
                }
            }
            
        }
        List<GameObject> listLeftRooms = new List<GameObject>(leftRooms);
        Instantiate(lWithSpawnPoint, lefterRoomPosition + (Vector2.left*26), Quaternion.identity,grid.transform);
        StartCoroutine(PreventClosing());
    }

    List<GameObject> GetChildren(GameObject parent)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in parent.transform)
        {
            children.Add(child.gameObject);
        }
        return children;
    }
}
