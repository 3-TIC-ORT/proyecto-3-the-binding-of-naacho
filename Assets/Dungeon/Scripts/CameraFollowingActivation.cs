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
    [Tooltip("Es el tiempo que le toma centrar la cámara")]
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
        CorrectCamera();
    }
    void CorrectCamera()
    {
        if (TouchingWall(upColliders) && !upCollided)
        {
            if (cameraTarget != null)
            {
                cameraTarget.transform.DOMoveY(cameraTarget.transform.position.y - GetCollisionDistance(upColliders, upRayLongitude), transitionDuration);
            }
            upCollided = true;
        }
        else if (!TouchingWall(upColliders)) upCollided = false;
        if (TouchingWall(downColliders) && !downCollided)
        {
            if (cameraTarget != null)
            {
                cameraTarget.transform.DOMoveY(cameraTarget.transform.position.y + GetCollisionDistance(downColliders, downRayLongitude), transitionDuration);
            }
            downCollided = true;
        }
        else if (!TouchingWall(downColliders)) downCollided = false;
        if (TouchingWall(rightColliders) && !rightCollided)
        {
            if (cameraTarget != null)
            {
                cameraTarget.transform.DOMoveX(cameraTarget.transform.position.x - GetCollisionDistance(rightColliders, HalfRoomXDistance), transitionDuration);
            }
            rightCollided = true;
        }
        else if (!TouchingWall(rightColliders)) rightCollided = false;
        if (TouchingWall(leftColliders) && !leftCollided)
        {
            if (cameraTarget != null)
            {
                cameraTarget.transform.DOMoveX(cameraTarget.transform.position.x + GetCollisionDistance(leftColliders, HalfRoomXDistance), transitionDuration);
            }
            leftCollided = true;
        }
        else if (!TouchingWall(leftColliders)) leftCollided = false;
    }
    float GetCollisionDistance(RaycastHit2D[] colliders, float rayMagnitude)
    {
        foreach (RaycastHit2D collider in colliders)
        {
            if (GameManager.Instance.stop) return 0f;
            else if (collider.collider!=null && collider.collider.gameObject.CompareTag("Room") || collider.collider.gameObject.CompareTag("DoorCameraTrigger")) return rayMagnitude-collider.distance;
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
        // SE LE RESTAN 0.7 UNIDADES ABAJO PARA EMPIECEN EN LAS PATAS DE NAACHO. CONSIDERAR ESTO PARA PROBLEMAS EN EL FUTURO.
        upRay = new Ray(transform.position+Vector3.down*0.55f, Vector2.up);
        upColliders = Physics2D.RaycastAll(upRay.origin, upRay.direction, upRayLongitude);
        downRay = new Ray(transform.position + Vector3.down * 0.55f, Vector2.down);
        downColliders = Physics2D.RaycastAll(downRay.origin, downRay.direction, downRayLongitude);
        rightRay = new Ray(transform.position + Vector3.down * 0.55f, Vector2.right);
        rightColliders = Physics2D.RaycastAll(rightRay.origin, rightRay.direction, HalfRoomXDistance);
        leftRay = new Ray(transform.position + Vector3.down * 0.55f, Vector2.left);
        leftColliders = Physics2D.RaycastAll(leftRay.origin, leftRay.direction, HalfRoomXDistance);
        Debug.DrawRay(upRay.origin, upRay.direction * upRayLongitude, Color.green);
        Debug.DrawRay(downRay.origin, downRay.direction * downRayLongitude, Color.green);
        Debug.DrawRay(rightRay.origin, rightRay.direction * HalfRoomXDistance, Color.green);
        Debug.DrawRay(leftRay.origin, leftRay.direction * HalfRoomXDistance, Color.green);
    }
}
