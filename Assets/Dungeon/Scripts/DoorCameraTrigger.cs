using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCameraTrigger : MonoBehaviour
{
    public float OcclusionCullingDistance;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("DoorCameraTrigger"))
        {
            int myID = gameObject.GetInstanceID();
            int colliderID = col.gameObject.GetInstanceID();
            if (myID < colliderID) Destroy(col.gameObject);
            else if (myID != colliderID) Destroy(gameObject);
        }
    }
    private void Start()
    {
        transform.parent = GameObject.FindGameObjectWithTag("GeneralContainer").transform;
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
