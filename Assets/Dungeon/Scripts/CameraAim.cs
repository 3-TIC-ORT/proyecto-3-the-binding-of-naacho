using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAim : MonoBehaviour
{
    private Transform playerPos;
    private CameraFollowingActivation cfa;
    
    private void Start()
    {
        cfa = GameObject.FindGameObjectWithTag("Player").GetComponent<CameraFollowingActivation>();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    void Update()
    {
        if (cfa.followVertically)
        {
            transform.position = new Vector3(transform.position.x,playerPos.position.y,transform.position.z);
        }
        if (cfa.followHorizontally)
        {
            transform.position = new Vector3(playerPos.position.x, transform.position.y, transform.position.z);
        }
    }
}
