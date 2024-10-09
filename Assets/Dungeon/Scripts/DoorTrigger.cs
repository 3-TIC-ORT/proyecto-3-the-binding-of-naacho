using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public bool isVertical = false;
    [Tooltip("Poner velocidad pensando que el juego estar� relentizado un 90%")]
    public float cameraTransitionSpeed;
    private BoxCollider2D _collider;
    private RoomTemplates templates;
    private GameObject player;
    private Transform cameraAimTransform;
    private CameraAim cameraAim;
    public float OcclusionCullingDistance;
    public float subtractSize;
    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        player = GameObject.FindGameObjectWithTag("Player");
        cameraAimTransform = GameObject.FindGameObjectWithTag("CameraAim").GetComponent<Transform>();
        cameraAim = GameObject.FindGameObjectWithTag("CameraAim").GetComponent<CameraAim>();
        // Si es una puerta vertical entonces rote la puerta 90�
        if (isVertical)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        transform.localScale = new Vector3(templates.horizontalDoorToDoorRoomArea.x-subtractSize, 1.6f, 1);
    }
    private void Update()
    {
        if (player != null)
        {
            // Occlusion Culling. Si el jugador est� lo suficientemente lejos desactive el collider para ahorrar recursos.
            if ((transform.position - player.transform.position).magnitude > OcclusionCullingDistance) _collider.enabled = false;
            else _collider.enabled = true;
        }
        else if (!GameManager.Instance.nachoNullPrinted)
        {
            Debug.LogWarning("Che macho, Naacho es null");
            GameManager.Instance.nachoNullPrinted = true;
        }
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
    
    // Mueve a al jugador verticalmente cambiando la posici�n de manera brusca. Antes de esto, stop del GameManager fue seteado a false para que
    // CameraTarger no se moviera. Despu�s se usa tweening para animar el Transform de cameraTarget. 
    // Una vez que termin� de animarse
    private void MoveVertically(Vector3 playerPos)
    {
        float nextDoorDistance = templates.verticalDoorToDoorRoomArea.y + 1f;
        float nextRoomCenter = templates.centerBetweenVerticaltalRooms * 2;
        // Identificar si el jugador est� a la abajo de este objeto (puerta) o no
        // y as� mover el jugador para arriba o a la abajo (en este caso que es una puerta vertical).
        if (playerPos.y - transform.position.y < 0)
        {
            cameraAimTransform.DOMoveX(transform.position.x, cameraTransitionSpeed).SetEase(Ease.Linear);
            cameraAimTransform.DOMoveY(cameraAimTransform.position.y + nextRoomCenter, cameraTransitionSpeed).SetEase(Ease.Linear).onComplete = () => { StartCoroutine(ManageCameraAndTime(cameraAim.pauseTime));};
            playerPos.y += nextDoorDistance;
            player.GetComponent<Transform>().position = playerPos;
        }
        else
        {
            cameraAimTransform.DOMoveX(transform.position.x, cameraTransitionSpeed).SetEase(Ease.Linear);
            cameraAimTransform.DOMoveY(cameraAimTransform.position.y - nextRoomCenter, cameraTransitionSpeed).SetEase(Ease.Linear).onComplete = () => { StartCoroutine(ManageCameraAndTime(cameraAim.pauseTime)); };
            playerPos.y -= nextDoorDistance;
            player.GetComponent<Transform>().position = playerPos;
        }
    }
    private void MoveHorizontally(Vector3 playerPos)
    {
        float nextDoorDistance = templates.horizontalDoorToDoorRoomArea.x + 1f;
        float nextRoomCenter = templates.centerBetweenHorizontalRooms * 2;
        if (playerPos.x - transform.position.x < 0)
        {   
            cameraAimTransform.DOLocalMoveY(transform.position.y,cameraTransitionSpeed).SetEase(Ease.Linear);
            cameraAimTransform.DOMoveX(cameraAimTransform.position.x + nextRoomCenter, cameraTransitionSpeed).SetEase(Ease.Linear).onComplete = () => { StartCoroutine(ManageCameraAndTime(cameraAim.pauseTime)); };
            playerPos.x += nextDoorDistance;
            player.GetComponent<Transform>().position = playerPos;
        }
        else
        {
            cameraAimTransform.DOLocalMoveY(transform.position.y, cameraTransitionSpeed).SetEase(Ease.Linear);
            cameraAimTransform.DOMoveX(cameraAimTransform.position.x - nextRoomCenter, cameraTransitionSpeed).SetEase(Ease.Linear).onComplete = () => { StartCoroutine(ManageCameraAndTime(cameraAim.pauseTime)); };
            playerPos.x -= nextDoorDistance;
            player.GetComponent<Transform>().position = playerPos;
        }
    }
    // Mueve al jugador a la siguiente habitaci�n. �Viste? que locura.
    IEnumerator ManageCameraAndTime(float timeToWait)
    {
        yield return new WaitForSecondsRealtime(timeToWait);
        if (GameManager.Instance.stop) GameManager.Instance.stop = false;
    }
    // Se llama cuando se colisiona con un roomConector. Si las dos habitaciones est�n conectadas entonces soy necesario y no me destruyo
    // En el caso contrario, me destruyo porque como no est�n conectadas y por consiguiente se van a conectar formando una habitaci�n
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
