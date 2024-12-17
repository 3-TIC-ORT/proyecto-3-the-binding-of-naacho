using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingModifier : ProyectilModifier
{
    public float rayLength = 0.1f;
    private Rigidbody2D rb;
    public override void Start()
    {
        base.Start();
        proyectilScript.dontDestroyWhenCollidedWall = true;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        SimulateBounce();
    }
    void SimulateBounce()
    {
        Vector2 direction = rb.velocity.normalized;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, rayLength);
        RaycastHit2D? hit = CollidedWithWall(hits);
        if (hit.HasValue)
        {
            Vector2 reflectedVelocity = Vector2.Reflect(rb.velocity, hit.Value.normal);
            rb.velocity = reflectedVelocity;
            //transform.position += (Vector3)(hit.Value.point + hit.Value.normal * 0.05f);
        }
    }
    RaycastHit2D? CollidedWithWall(RaycastHit2D[] colliders)
    {
        foreach (RaycastHit2D collider in colliders) 
        {
            if (collider.collider.gameObject.CompareTag("Room")) Debug.Log("DDIDIDIDIDID");
            if (collider.collider.gameObject.CompareTag("Room")) return collider;
        }
        return null;
    }
}
