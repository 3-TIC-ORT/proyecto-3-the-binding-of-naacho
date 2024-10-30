using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseEnemieScript : Enemy
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
        
    }
    public void FixedUpdate()
    {
        if (GameManager.Instance.stop) return;
        Vector2 playerDir = (Vector2)(Player.transform.position - transform.position).normalized;
        rb2D.velocity = Speed * playerDir;
        rb2D.AddForce(Speed * playerDir * Time.deltaTime, ForceMode2D.Force);
        CheckMainDirection(playerDir);
    }

    private void CheckMainDirection(Vector3 direction)
    {
        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (direction.y > 0)
            {
                //Debug.Log("Me muevo hacia arriba");
                animator.SetBool("up", true);
                animator.SetBool("down", false);
                animator.SetBool("horizontal", false);
            }
            else
            {
                //Debug.Log("Me muevo hacia abajo");
                animator.SetBool("down", true);
                animator.SetBool("up", false);
                animator.SetBool("horizontal", false);
            }
        }
        else
        {
            if (direction.x > 0)
            {
                //Debug.Log("Me muevo hacia la derecha");
                SpRenderer.flipX = false;
                animator.SetBool("horizontal", true);
                animator.SetBool("up", false);
                animator.SetBool("down", false);

            }
            else
            {
                //Debug.Log("Me muevo hacia la izquierda");
                SpRenderer.flipX = true;
                animator.SetBool("horizontal", true);
                animator.SetBool("up", false);
                animator.SetBool("down", false);
            }
        }
    }
}
