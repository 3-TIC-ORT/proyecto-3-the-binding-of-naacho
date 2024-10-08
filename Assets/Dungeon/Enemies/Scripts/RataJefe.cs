using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RataJefe : Enemy
{
    private Animator animator;
    public override void Update()
    {
        base.Update();
        if (GameManager.Instance.stop) return;
        Vector2 playerDir = (Vector2)(Player.transform.position - transform.position).normalized;
        rb2D.velocity = Speed * Time.deltaTime * playerDir;
        CheckMainDirection(playerDir);
    }
    public override void Start()
    {
        base.Start();
        isBoss = true;
        animator = gameObject.GetComponent<Animator>();
    }
    // Ve donde se está moviendo, si mayormente para un eje o para el otro. En función de eso setea la animación correspondiente.
    private void CheckMainDirection(Vector3 direction)
    {
        //if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        //{
        //    if (direction.y > 0)
        //    {
        //        animator.SetBool("up", true);
        //        animator.SetBool("down", false);
        //        animator.SetBool("horizontal", false);
        //    }
        //    else
        //    {
        //        animator.SetBool("down", true);
        //        animator.SetBool("up", false);
        //        animator.SetBool("horizontal", false);
        //    }
        //}
        //else
        //{
        //    if (direction.x > 0)
        //    {
        //        SpRenderer.flipX = true;
        //        animator.SetBool("horizontal", true);
        //        animator.SetBool("up", false);
        //        animator.SetBool("down", false);

        //    }
        //    else
        //    {
        //        SpRenderer.flipX = false;
        //        animator.SetBool("horizontal", true);
        //        animator.SetBool("up", false);
        //        animator.SetBool("down", false);
        //    }
        //}
    }
}
