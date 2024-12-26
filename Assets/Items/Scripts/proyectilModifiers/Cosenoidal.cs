using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cosenoidal : ProyectilModifier
{
    private float amplitude = 1.8f;
    private float frequency = 15;
    private float elapsedTime = 0;
    public override void Start()
    {
        base.Start();
    }
    private void FixedUpdate()
    {
        elapsedTime += Time.fixedDeltaTime;
        Vector2 perpendicularVelocity = Vector2.Perpendicular(proyectilScript.rb2D.velocity.normalized*Mathf.Cos(elapsedTime*frequency)*amplitude);
        proyectilScript.rb2D.velocity = proyectilScript.rb2D.velocity + perpendicularVelocity;   
    }
}
