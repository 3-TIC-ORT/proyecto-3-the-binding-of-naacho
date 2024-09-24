using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public bool isVertical = false;
    [Tooltip("Poner velocidad pensando que el juego estará relentizado un 90%")]
    public float cameraTransitionSpeed;
    private BoxCollider2D collider;
    private RoomTemplates templates;
    private GameObject player;
    private Transform cameraAimTransform;
    private CameraAim cameraAim;
    public float OcclusionCullingDistance;
    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        player = GameObject.FindGameObjectWithTag("Player");
        cameraAimTransform = GameObject.FindGameObjectWithTag("CameraAim").GetComponent<Transform>();
        cameraAim = GameObject.FindGameObjectWithTag("CameraAim").GetComponent<CameraAim>();
        // Si es una puerta vertical entonces rote la puerta 90°
        if (isVertical)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        transform.localScale = new Vector3(templates.horizontalDoorToDoorRoomArea.x-1, 1.6f, 1);
    }
    private void Update()
    {
        // Occlusion Culling. Si el jugador está lo suficientemente lejos desactive el collider para ahorrar recursos.
        if ((transform.position - player.transform.position).magnitude > OcclusionCullingDistance) collider.enabled = false;
        else collider.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Vector3 playerPos = col.gameObject.GetComponent<Transform>().position;
            GameManager.Instance.stop = true;
            if (isVertical) MoveVertically(playerPos);
            else MoveHorizontally(playerPos);
        }
        else if (col.gameObject.CompareTag("RoomConector")) StartCoroutine(CheckIfImNecesarry(col.gameObject));
    }
    
    
    private void MoveVertically(Vector3 playerPos)
    {
        float nextDoorDistance = templates.verticalDoorToDoorRoomArea.y + 0.1f;
        // Identificar si el jugador está a la abajo de este objeto (puerta) o no
        // y así mover el jugador para arriba o a la abajo (en este caso que es una puerta vertical).
        if (playerPos.y - transform.position.y < 0)
        {
            cameraAimTransform.DOMoveY(cameraAimTransform.position.y + nextDoorDistance, cameraTransitionSpeed).SetEase(Ease.Linear).onComplete = ManageCameraAndTime;
            playerPos.y += nextDoorDistance;
            player.GetComponent<Transform>().position = playerPos;
        }
        else
        {
            cameraAimTransform.DOMoveY(cameraAimTransform.position.y - nextDoorDistance, cameraTransitionSpeed).SetEase(Ease.Linear).onComplete = ManageCameraAndTime;
            playerPos.y -= nextDoorDistance;
            player.GetComponent<Transform>().position = playerPos;
        }
    }
    private void MoveHorizontally(Vector3 playerPos)
    {
        float nextDoorDistance = templates.horizontalDoorToDoorRoomArea.x + 0.1f;

        if (playerPos.x - transform.position.x < 0)
        {
            cameraAimTransform.DOMoveX(cameraAimTransform.position.x + nextDoorDistance, cameraTransitionSpeed).SetEase(Ease.Linear).onComplete = ManageCameraAndTime; 
            playerPos.x += nextDoorDistance;
            player.GetComponent<Transform>().position = playerPos;
        }
        else
        {
            cameraAimTransform.DOMoveX(cameraAimTransform.position.x - nextDoorDistance, cameraTransitionSpeed).SetEase(Ease.Linear).onComplete = ManageCameraAndTime;
            playerPos.x -= nextDoorDistance;
            player.GetComponent<Transform>().position = playerPos;
        }
    }
    // Mueve al jugador a la siguiente habitación. ¿Viste? que locura.
    private void ManageCameraAndTime()
    {
        if (GameManager.Instance.stop) GameManager.Instance.stop = false;
    }
    // Se llama cuando se colisiona con un roomConector. Si las dos habitaciones están conectadas entonces soy necesario y no me destruyo
    // En el caso contrario, me destruyo porque como no están conectadas y por consiguiente se van a conectar formando una habitación
    // grande, no debo existir :,(
    IEnumerator CheckIfImNecesarry(GameObject roomConector)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        RoomConector roomConectorSCR = roomConector.GetComponent<RoomConector>();
        if (!roomConectorSCR.bothRoomsAreConected) Destroy(gameObject);
    }
    List<GameObject> GetChildren(GameObject parent, bool filter, string tag)
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in parent.transform)
        {
            if (!filter)
            {
                children.Add(child.gameObject);
            }
            else if (filter && child.CompareTag(tag))
            {
                children.Add(child.gameObject);
            }
        }

        return children;
    }
}
