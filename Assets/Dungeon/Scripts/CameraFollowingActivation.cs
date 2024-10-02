using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowingActivation : MonoBehaviour
{
    private CameraAim cameraTarget;
    public float HalfRoomXDistance;
    public float HalfRoomYDistance;
    public bool followHorizontally;
    public bool followVertically;
    void Start()
    {
        
        cameraTarget = GameObject.FindGameObjectWithTag("CameraAim").GetComponent<CameraAim>();
    }

    void Update()
    {
        Ray upRay = new Ray(transform.position, Vector2.up);
        Ray downRay = new Ray(transform.position, Vector2.down);
        Ray rightRay = new Ray(transform.position, Vector2.right);
        Ray leftRay = new Ray(transform.position, Vector2.left);
        Debug.DrawRay(upRay.origin, upRay.direction*HalfRoomYDistance);
        Debug.DrawRay(downRay.origin, downRay.direction*HalfRoomYDistance);
        Debug.DrawRay(rightRay.origin, rightRay.direction*HalfRoomXDistance);
        Debug.DrawRay(leftRay.origin, leftRay.direction*HalfRoomXDistance);

    }
}
