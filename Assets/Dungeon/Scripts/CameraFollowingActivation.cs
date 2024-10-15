using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraFollowingActivation : MonoBehaviour
{
    private CameraAim cameraTarget;
    public float HalfRoomXDistance;
    public float HalfRoomYDistance;
    private float upRayLongitude;
    private float downRayLongitude;
    public bool followHorizontally;
    public bool followVertically;
    private bool upCollided;
    private bool downCollided;
    private bool rightCollided;
    private bool leftCollided;
    Ray upRay;
    RaycastHit2D[] upColliders;
    Ray downRay;
    RaycastHit2D[] downColliders;
    Ray rightRay;
    RaycastHit2D[] rightColliders;
    Ray leftRay;
    RaycastHit2D[] leftColliders;
    [Tooltip("Es el tiempo que le toma centrar la c�mara")]
    public float transitionDuration;
    void Start()
    {
        // PORQUE LOS RAYS EMPIEZAN EN LAS PATAS DE NAACHO Y POR ESO NO TIENEN LA MISMA LONGITUD
        upRayLongitude = HalfRoomYDistance + 0.55f;
        downRayLongitude = HalfRoomYDistance - 0.55f;
        cameraTarget = GameObject.FindGameObjectWithTag("CameraAim").GetComponent<CameraAim>();
    }

    void Update()
    {
        if (GameManager.Instance.stop)
        {
            upCollided = true;
            downCollided = true;
            rightCollided = true;
            leftCollided = true;
            return;
        }
        UpdateRaycasts();
        UnlockAxis();
        if (!PlayerManager.Instance.cameraIsShaking) CorrectCamera();
    }
    void CorrectCamera()
    {
        if (TouchingWall(upColliders) && !upCollided && !cameraTargetIsNull())
        {
            float distance = GetCollisionDistance(upColliders, upRayLongitude);
            if (distance!=0)
            {
                cameraTarget.transform.DOMoveY(cameraTarget.transform.position.y-distance,transitionDuration);
                upCollided = true;
            }
        }
        else if (!TouchingWall(upColliders)) upCollided = false;
        if (TouchingWall(downColliders) && !downCollided && !cameraTargetIsNull())
        {
            float distance = GetCollisionDistance(downColliders, downRayLongitude);
            if (distance != 0)
            {
                cameraTarget.transform.DOMoveY(cameraTarget.transform.position.y + distance, transitionDuration);
                downCollided = true;
            }
        }
        else if (!TouchingWall(downColliders)) downCollided = false;
        if (TouchingWall(rightColliders) && !rightCollided && !cameraTargetIsNull())
        {
            float distance = GetCollisionDistance(rightColliders, HalfRoomXDistance);
            if (distance != 0)
            {
                cameraTarget.transform.DOMoveX(cameraTarget.transform.position.x-distance, transitionDuration);
                rightCollided = true;
            }
        }
        else if (!TouchingWall(rightColliders)) rightCollided = false;
        if (TouchingWall(leftColliders) && !leftCollided && !cameraTargetIsNull())
        {
            float distance = GetCollisionDistance(leftColliders, HalfRoomXDistance);
            if (distance != 0)
            {
                cameraTarget.transform.DOMoveX(cameraTarget.transform.position.x + distance, transitionDuration);
                leftCollided = true;
            }
        }
        else if (!TouchingWall(leftColliders)) leftCollided = false;
    }
    bool cameraTargetIsNull()
    {
        if (cameraTarget == null) return true;
        else return false;
    }
    float GetCollisionDistance(RaycastHit2D[] colliders, float rayMagnitude)
    {
        foreach (RaycastHit2D collider in colliders)
        {
            if (GameManager.Instance.stop) return 0f;
            else if (collider.collider.gameObject.CompareTag("Room") || collider.collider.gameObject.CompareTag("DoorCameraTrigger"))
            {
                if (rayMagnitude - collider.distance > 0.5f)
                {
                    Debug.Log(rayMagnitude - collider.distance);
                    return rayMagnitude - collider.distance;
                }
                else return 0;

            }
        }
        return 0f;
    }
    private void UnlockAxis()
    {
        if (!TouchingWall(upColliders) && !TouchingWall(downColliders) && !GameManager.Instance.stop) followVertically = true;
        else followVertically = false;
        if (!TouchingWall(rightColliders) && !TouchingWall(leftColliders) && !GameManager.Instance.stop) followHorizontally = true;
        else followHorizontally = false;
    }
    private bool TouchingWall(RaycastHit2D[] colliders)
    {
        foreach (RaycastHit2D collider in colliders)
        {
            if (collider.collider.gameObject.CompareTag("Room") || collider.collider.gameObject.CompareTag("DoorCameraTrigger")) return true;
        }
        return false;
    }
    private void UpdateRaycasts()
    {
        // Definir el color predeterminado
        Color rayColorUp = Color.green;
        Color rayColorDown = Color.green;
        Color rayColorRight = Color.green;
        Color rayColorLeft = Color.green;

        upRay = new Ray(transform.position + Vector3.down * 0.55f, Vector2.up);
        upColliders = Physics2D.RaycastAll(upRay.origin, upRay.direction, upRayLongitude);
        if (upColliders.Length > 1) rayColorUp = Color.blue;

        downRay = new Ray(transform.position + Vector3.down * 0.55f, Vector2.down);
        downColliders = Physics2D.RaycastAll(downRay.origin, downRay.direction, downRayLongitude);
        if (downColliders.Length > 1) rayColorDown = Color.blue;

        rightRay = new Ray(transform.position + Vector3.down * 0.55f, Vector2.right);
        rightColliders = Physics2D.RaycastAll(rightRay.origin, rightRay.direction, HalfRoomXDistance);
        if (rightColliders.Length > 1) rayColorRight = Color.blue;

        leftRay = new Ray(transform.position + Vector3.down * 0.55f, Vector2.left);
        leftColliders = Physics2D.RaycastAll(leftRay.origin, leftRay.direction, HalfRoomXDistance);
        if (leftColliders.Length > 1) rayColorLeft = Color.blue;

        // Dibujar los rayos con su color correspondiente
        Debug.DrawRay(upRay.origin, upRay.direction * upRayLongitude, rayColorUp);
        Debug.DrawRay(downRay.origin, downRay.direction * downRayLongitude, rayColorDown);
        Debug.DrawRay(rightRay.origin, rightRay.direction * HalfRoomXDistance, rayColorRight);
        Debug.DrawRay(leftRay.origin, leftRay.direction * HalfRoomXDistance, rayColorLeft);
    }
}

