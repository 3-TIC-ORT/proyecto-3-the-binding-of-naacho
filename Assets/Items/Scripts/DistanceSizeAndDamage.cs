using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceSizeAndDamage : ProyectilModifier
{
    public float growingSize=0.06f;
    public float growingDamage=0.07f;
    public override void Start()
    {
        base.Start();
    }
    private void Update()
    {
        proyectilScript.Damage += growingDamage;
        transform.localScale += new Vector3 (growingSize, growingSize, 1f);
    }
}
