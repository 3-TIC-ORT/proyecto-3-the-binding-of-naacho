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
        RaycastHit2D[] upColliders = Physics2D.RaycastAll(upRay.origin, upRay.direction, HalfRoomYDistance);
        Ray downRay = new Ray(transform.position, Vector2.down);
        RaycastHit2D[] downColliders = Physics2D.RaycastAll(downRay.origin, downRay.direction, HalfRoomYDistance);
        Ray rightRay = new Ray(transform.position, Vector2.right);
        RaycastHit2D[] rightColliders = Physics2D.RaycastAll(rightRay.origin, rightRay.direction, HalfRoomXDistance);
        Ray leftRay = new Ray(transform.position, Vector2.left);
        RaycastHit2D[] leftColliders = Physics2D.RaycastAll(leftRay.origin, leftRay.direction, HalfRoomXDistance);

        if (!TouchingWall(upColliders) && !TouchingWall(downColliders) && !GameManager.Instance.stop) followVertically = true;
        else followVertically = false;
        if (!TouchingWall(rightColliders) && !TouchingWall(leftColliders) && !GameManager.Instance.stop) followHorizontally = true;
        else followHorizontally = false;
        Debug.DrawRay(upRay.origin, upRay.direction*HalfRoomYDistance,Color.red);
        Debug.DrawRay(downRay.origin, downRay.direction*HalfRoomYDistance, Color.red);
        Debug.DrawRay(rightRay.origin, rightRay.direction*HalfRoomXDistance, Color.red);
        Debug.DrawRay(leftRay.origin, leftRay.direction*HalfRoomXDistance, Color.red);
    }

    private bool TouchingWall(RaycastHit2D[] colliders)
    {
        foreach (RaycastHit2D collider in colliders)
        {
            if (collider.collider.gameObject.CompareTag("Room")) return true;
        }
        return false;
    }
}
