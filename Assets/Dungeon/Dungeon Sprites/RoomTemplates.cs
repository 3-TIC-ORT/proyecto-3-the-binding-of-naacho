using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
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
    void Start()
    {
        grid = GameObject.Find("Grid");
        StartCoroutine(PreventClosing());
    }

    IEnumerator PreventClosing()
    {
        int lastRoomsGenerated = -1;
        while (lastRoomsGenerated != roomsGenerated)
        {
            lastRoomsGenerated = roomsGenerated;
            yield return new WaitForSecondsRealtime(2f);
        }
        if (roomsGenerated < roomsMin)
        {
            CreateRoom();
        }
        else minCompleted = true;
        yield return new WaitForSecondsRealtime(1f);
    }
    private void CreateRoom()
    {
        Debug.Log("HOLAAA");
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
        Instantiate(listLeftRooms.Find(go => go.name == "L"), lefterRoomPosition + (Vector2.left*26), Quaternion.identity,grid.transform);
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
