using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaachoSkater : Enemy
{
    private Vector2[] directions = {new Vector2(-1,1), new Vector2(1,1), new Vector2(1,-1), new Vector2(-1,-1) };
    private CircleCollider2D _collider;
    private Vector2 playerPos;
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
        if (!alejarseDeNacho)
        {
            int rand = Random.Range(0, directions.Length);
            Vector2 direction = directions[rand];
            rb2D.AddForce(direction * Speed * Time.deltaTime, ForceMode2D.Impulse);
        }
        else
        {
            Vector2 newDirection = Vector2.zero;
            if (transform.position.x < playerPos.x) newDirection.x = -1;
            else newDirection.x = 1;
            if (transform.position.y < playerPos.y) newDirection.y = -1;
            else newDirection.y = 1;
            rb2D.velocity = Vector2.zero;
            rb2D.AddForce(newDirection * Speed * Time.deltaTime, ForceMode2D.Impulse);
        }

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
        if (rb2D.velocity==Vector2.zero && !GameManager.Instance.stop) StartMovement(false);
        base.Update();
        if (GameManager.Instance.stop) return;
    }
}
