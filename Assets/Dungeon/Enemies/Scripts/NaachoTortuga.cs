using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaachoTortuga : Enemy
{
    private Animator animator;
    private SpriteRenderer sr;
    public override void Update()
    {
        if (GameManager.Instance.stop) return;
        Vector2 playerDir = (Vector2)(Player.transform.position - transform.position).normalized;
        rb2D.velocity = Speed * Time.deltaTime * playerDir;
        CheckMainDirection(playerDir);
    }
    public override void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        Player = GameObject.Find("Naacho");
    }

    private void CheckMainDirection(Vector3 direction)
    {
        if (Mathf.Abs(direction.y)>Mathf.Abs(direction.x))
        {
            if (direction.y>0)
            {
                Debug.Log("Me muevo hacia arriba");
                animator.SetBool("up", true);
                animator.SetBool("down",false);
                animator.SetBool("horizontal", false);
            }
            else
            {
                Debug.Log("Me muevo hacia abajo");
                animator.SetBool("down", true);
                animator.SetBool("up", false);
                animator.SetBool("horizontal", false);
            }
        }
        else
        {
            if (direction.x>0)
            {
                Debug.Log("Me muevo hacia la derecha");
                sr.flipX = true;
                animator.SetBool("horizontal", true);
                animator.SetBool("up", false);
                animator.SetBool("down", false);

            }
            else
            {
                Debug.Log("Me muevo hacia la izquierda");
                sr.flipX = false;
                animator.SetBool("horizontal", true);
                animator.SetBool("up", false);
                animator.SetBool("down", false);
            }
        }
    }
}
