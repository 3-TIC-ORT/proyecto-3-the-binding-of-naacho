using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraAim : MonoBehaviour
{
    private Transform playerPos;
    private CameraFollowingActivation cfa;
    public float followSpeed;
    private void Start()
    {
        cfa = GameObject.FindGameObjectWithTag("Player").GetComponent<CameraFollowingActivation>();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    void Update()
    {
        if (cfa.followVertically)
        {
            transform.DOMoveY(playerPos.position.y, followSpeed);
        }
        if (cfa.followHorizontally)
        {
            transform.DOMoveX(playerPos.position.x, followSpeed);
        }
    }
}
