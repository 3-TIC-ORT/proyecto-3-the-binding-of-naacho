using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lechuguitau: Enemy
{
    private Animator animator;
    public override void Start()
    {
        base.Start();
        animator = gameObject.GetComponent<Animator>();
    }
    public override void Update()
    {
        base.Update();
        if (GameManager.Instance.stop) return;  
        Vector2 playerDir = (Vector2)(Player.transform.position - transform.position).normalized;
        rb2D.velocity = Speed * Time.deltaTime * playerDir;
        animator.SetBool("isMoving", true);
        CheckMainDirection(playerDir);
    }

    private void CheckMainDirection(Vector3 direction)
    {
        if (direction.x > 0)
        {
            SpRenderer.flipX = false;
        }
        else
        {
            SpRenderer.flipX = true;
        }
    }
}
