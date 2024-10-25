using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemies : ProyectilModifier
{
    public float radiusToFollow=2.5f;
    private Rigidbody2D rb;
    private bool isFollowing;
    private Transform targetTrans;
    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFollowing) 
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radiusToFollow);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.CompareTag("Enemy"))
                {
                    isFollowing = true;
                    targetTrans = collider.GetComponent<Transform>();
                }
            }
        }
        else
        {
            if (targetTrans == null)
            {
                isFollowing = false;
                return;
            }
            Vector2 newDirection = (targetTrans.position - transform.position).normalized;
            rb.velocity = newDirection * proyectilScript.InitialVelocity.magnitude;

        }
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}
