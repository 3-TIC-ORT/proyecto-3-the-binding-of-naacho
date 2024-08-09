using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public int roomsGenerated = 0;
    public int roomsLimit; // No es preciso
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
        
    }

   
    void Update()
    {
        
    }
}
