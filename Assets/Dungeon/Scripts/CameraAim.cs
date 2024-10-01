using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAim : MonoBehaviour
{
    private Transform playerPos;
    public bool followPlayer;
    
    private void Start()
    {
        followPlayer = true;
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    void Update()
    {
        if (GameManager.Instance.stop) followPlayer = false;
        if (followPlayer)
        {
            transform.position = playerPos.position;
        }
    }
}
