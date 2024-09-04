using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public bool isVertical = false;
    public float lerpPositionDuration;
    private BoxCollider2D collider;
    private GameObject player;
    public float OcclusionCullingDistance;
    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (isVertical)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }
    private void Update()
    {
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
    private void MoveToTheNextRoom(GameObject player)
    {
        Vector3 playerPos = player.GetComponent<Transform>().position;
        if (isVertical) 
        {
            if (playerPos.y - transform.position.y<0)
            {
                StartCoroutine(LerpPosition(playerPos.y, playerPos.y + 12, lerpPositionDuration, player, false));
            }
            else
            {
                StartCoroutine(LerpPosition(playerPos.y, playerPos.y - 12, lerpPositionDuration, player, false));
            }
        }
        else
        {
            if (playerPos.x - transform.position.x < 0)
            {
                StartCoroutine(LerpPosition(playerPos.x, playerPos.x + 12, lerpPositionDuration, player, true));
            }
            else
            {
                StartCoroutine(LerpPosition(playerPos.x, playerPos.x - 12, lerpPositionDuration, player, true));
            }
        }
    }
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
    IEnumerator CheckIfImNecesarry(GameObject roomConector)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        RoomConector roomConectorSCR = roomConector.GetComponent<RoomConector>();
        if (!roomConectorSCR.bothRoomsAreConected) Destroy(gameObject);
    }
}
