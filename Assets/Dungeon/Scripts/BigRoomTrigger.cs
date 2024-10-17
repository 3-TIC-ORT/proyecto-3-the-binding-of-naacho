using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigRoomTrigger : MonoBehaviour
{
    private RoomConector roomConector;

    void Start()
    {
        roomConector = transform.parent.GetComponent<RoomConector>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("DoorCameraTrigger") && roomConector.doorsDestroyed)
        {
            Destroy(col.gameObject);
        }
    }
}
