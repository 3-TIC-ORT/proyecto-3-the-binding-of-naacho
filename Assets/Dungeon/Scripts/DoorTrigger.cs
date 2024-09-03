using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public bool isVertical = false;
    private void Start()
    {
        if (isVertical)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
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
            if (playerPos.y - transform.position.y<0) playerPos.y += 12;
            else playerPos.y -= 12;
        }
        else
        {
            if (playerPos.x - transform.position.x < 0) playerPos.x += 12;
            else playerPos.x -= 12;
        }
        player.GetComponent<Transform>().position = playerPos;
    }
    IEnumerator CheckIfImNecesarry(GameObject roomConector)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        RoomConector roomConectorSCR = roomConector.GetComponent<RoomConector>();
        if (!roomConectorSCR.bothRoomsAreConected) Destroy(gameObject);
    }
}
