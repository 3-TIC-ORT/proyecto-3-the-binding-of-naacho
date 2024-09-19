using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DoorTrigger : MonoBehaviour
{
    public bool isVertical = false;
    public float lerpPositionDuration;
    private BoxCollider2D collider;
    private RoomTemplates templates;
    private GameObject player;
    private Transform cameraTransform;
    public float changeRoomSpeed;
    public float OcclusionCullingDistance;
    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        player = GameObject.FindGameObjectWithTag("Player");
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
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
            MoveToTheNextRoom(col.gameObject);
        }
        else if (col.gameObject.CompareTag("RoomConector")) StartCoroutine(CheckIfImNecesarry(col.gameObject));
    }
    
    // Mueve al jugador a la siguiente habitación. ¿Viste? que locura.
    private void MoveToTheNextRoom(GameObject player)
    {
        Vector3 playerPos = player.GetComponent<Transform>().position;
        if (isVertical) 
        {
            float nextDoorDistance = templates.verticalDoorToDoorRoomArea.y+0.1f;
            // Identificar si el jugador está a la abajo de este objeto (puerta) o no
            // y así mover el jugador para arriba o a la abajo (en este caso que es una puerta vertical).
            if (playerPos.y - transform.position.y<0)
            {
                playerPos.y += nextDoorDistance;
                player.GetComponent<Transform>().position = playerPos;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>().DOMoveY(cameraTransform.position.y + nextDoorDistance, changeRoomSpeed).SetEase(Ease.OutCubic);
            }
            else
            {
                playerPos.y -= nextDoorDistance;
                player.GetComponent<Transform>().position = playerPos;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>().DOMoveY(cameraTransform.position.y - nextDoorDistance, changeRoomSpeed).SetEase(Ease.OutCubic);
            }
        }
        else
        {
            float nextDoorDistance = templates.horizontalDoorToDoorRoomArea.x+0.1f;
            if (playerPos.x - transform.position.x < 0)
            {
                playerPos.x += nextDoorDistance;
                player.GetComponent<Transform>().position = playerPos;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>().DOMoveX(cameraTransform.position.x + nextDoorDistance, changeRoomSpeed).SetEase(Ease.OutCubic);

            }
            else
            {
                playerPos.x -= nextDoorDistance;
                player.GetComponent<Transform>().position = playerPos;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>().DOMoveX(cameraTransform.position.x - nextDoorDistance, changeRoomSpeed).SetEase(Ease.OutCubic);
            }
        }
    }
    // Mueve al jugador de una habitación a otra.
    // initialAxis: valor del eje (X si es horizontal, Y si es vertical) del jugador
    // finalAxis: valor del eje del otro lado de la puerta
    // duration: que tan rápido será la transición (segundos)
    // player: no lo se
    // isHorizontal: para saber si el eje es el eje X o Y.
    IEnumerator LerpPosition(float initialAxis, float finalAxis, float duration, GameObject player, bool isHorizontal)
    {
        float time=0;
        float currentAxis;
        Vector3 playerPos = player.GetComponent<Transform>().position;
        while (time<duration)
        {
            currentAxis = Mathf.Lerp(initialAxis, finalAxis, time / duration);
            if (isHorizontal) playerPos.x = currentAxis;
            else playerPos.y = currentAxis;
            player.GetComponent<Transform>().position = playerPos;

            time += Time.deltaTime;
            yield return null;
        }
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
