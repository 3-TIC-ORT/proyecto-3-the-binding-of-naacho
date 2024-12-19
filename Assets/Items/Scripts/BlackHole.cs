using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : ProyectilModifier
{
    public GameObject blackHolePrefab;
    public override void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Instantiate(blackHolePrefab,transform.position,Quaternion.identity, GameObject.FindGameObjectWithTag("GeneralContainer").transform);
        }
    }
}
