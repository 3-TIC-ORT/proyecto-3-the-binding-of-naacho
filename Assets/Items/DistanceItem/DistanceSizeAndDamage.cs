using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceSizeAndDamage : ProyectilModifier
{
    public float growingSize=0.06f;
    public float growingDamage=0.04f;
    public override void Start()
    {
        base.Start();
    }
    private void Update()
    {
        proyectilScript.Damage += growingDamage;
        transform.localScale = new Vector3 (transform.localScale.x+growingSize, transform.localScale.y+growingSize, 1f);
    }
}
