using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaachoSkater : Enemy
{
    private Vector2[] directions = {new Vector2(-1,1), new Vector2(1,1), new Vector2(1,-1), new Vector2(-1,-1) };
    private CircleCollider2D _collider;
    private Vector2 playerPos;

    private float speedModifier;
    public override void Start()
    {
        base.Start();
        playerPos = Player.GetComponent<Transform>().position;
        canRecieveKnockback = false;
        _collider = GetComponent<CircleCollider2D>();
        Vector2 playerDir = (Vector2)(Player.transform.position - transform.position).normalized;
    }
    private void StartMovement(bool alejarseDeNacho)
    {
        speedModifier = effects.isSlowed ? 0.5f : 1;
        Vector2 direction;
        if (!alejarseDeNacho)
        {
            int rand = Random.Range(0, directions.Length);
            direction = directions[rand];
        }
        else
        {
            direction = Vector2.zero;
            if (transform.position.x < playerPos.x) direction.x = -1;
            else direction.x = 1;
            if (transform.position.y < playerPos.y) direction.y = -1;
            else direction.y = 1;
        }
        rb2D.velocity=Vector2.zero;
        rb2D.AddForce(direction*Speed*speedModifier,ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            StartMovement(true);
        }
    }
    public override void Update()
    {
        base.Update();
        if (GameManager.Instance.stop) return;
        float targetSpeedModifier = effects.isSlowed? 0.5f:1;
        if (speedModifier!=targetSpeedModifier)
        {
            speedModifier=targetSpeedModifier;
            rb2D.velocity*=speedModifier;
        }
        if (rb2D.velocity==Vector2.zero && !GameManager.Instance.stop) StartMovement(false);
    }
}
