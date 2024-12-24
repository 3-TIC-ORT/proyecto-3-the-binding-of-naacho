using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : ProyectilModifier
{
    public GameObject blackHolePrefab;
    public override void Start()
    {
        base.Start();
        blackHolePrefab = ExternInitializer.Instance.blackHoleExtPrefab;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (blackHolePrefab != null) 
            {         
                Vector3 spawnPosition = col.gameObject.transform.position;  
                GameObject blackHoleInstance = Instantiate(blackHolePrefab,spawnPosition,Quaternion.identity, GameObject.FindGameObjectWithTag("GeneralContainer").transform);
                blackHoleInstance.GetComponent<BlackHoleExt>().impactedEnemy = col.gameObject.GetComponent<Enemy>();
            }
        }
    }
}
