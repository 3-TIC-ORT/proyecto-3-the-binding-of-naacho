using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCameraTrigger : MonoBehaviour
{
    public float OcclusionCullingDistance;
    public bool vertical;
    public GameObject mySelf;
    public bool doNotDestroyMe;
    private RoomTemplates templates;
  
    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(templates.centerBetweenHorizontalRooms * 2, 4), 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("SpawnPoint")) vertical = true;
        }
        if (vertical) transform.rotation = Quaternion.Euler(0, 0, 90);
        if (!doNotDestroyMe)
        {

            Transform generalContainer = GameObject.FindGameObjectWithTag("GeneralContainer").transform;
            GameObject doorCameraTrigger = Instantiate(mySelf, transform.position, Quaternion.identity, generalContainer);
            DoorCameraTrigger doorCameraTriggerScript = doorCameraTrigger.GetComponent<DoorCameraTrigger>();
            doorCameraTriggerScript.vertical= vertical;
            doorCameraTriggerScript.doNotDestroyMe = true;
            if (!doNotDestroyMe) Destroy(gameObject);
            transform.parent = GameObject.FindGameObjectWithTag("GeneralContainer").transform;
        }
    }
    private void Update()
    {
        if (PlayerManager.Instance != null) 
        {
            if ((transform.position - PlayerManager.Instance.transform.position).magnitude > OcclusionCullingDistance) GetComponent<BoxCollider2D>().enabled = false;
            else GetComponent<BoxCollider2D>().enabled = true;
        }
        else if (!GameManager.Instance.nachoNullPrinted)
        {
            Debug.LogWarning("Che macho, Naacho es null");
            GameManager.Instance.nachoNullPrinted = true;
        }
    }
}
