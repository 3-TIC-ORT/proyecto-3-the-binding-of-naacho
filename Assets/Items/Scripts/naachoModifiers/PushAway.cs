using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAway : NaachoModifier
{
    private GameObject pushAwayPrefab;
    private GameObject currentPushAwayInstance;
    public override void Start()
    {
        base.Start();
        pushAwayPrefab = ExternInitializer.Instance.pushAwayExtPrefab;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (currentPushAwayInstance==null && heartSystem != null && !heartSystem.dead && col.gameObject.CompareTag("Enemy"))
        {
            currentPushAwayInstance = Instantiate(pushAwayPrefab, transform.position, Quaternion.identity, GameObject.FindGameObjectWithTag("GeneralContainer").transform);
            
        }
    }
}
