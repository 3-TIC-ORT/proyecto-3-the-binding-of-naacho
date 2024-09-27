using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaachoTortuga : Enemy
{
    private Animator animator;
    public override void Update()
    {
        if (GameManager.Instance.stop) return;
        Vector2 playerDir = (Vector2)(Player.transform.position - transform.position).normalized;
        rb2D.velocity = Speed * Time.deltaTime * playerDir;
        CheckMainDirection(playerDir);
    }
    public override void Start()
    {
        base.Start();
        animator = gameObject.GetComponent<Animator>();
    }
    // Ve donde se est� moviendo, si mayormente para un eje o para el otro. En funci�n de eso setea la animaci�n correspondiente.
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
                SpRenderer.flipX = true;
                animator.SetBool("horizontal", true);
                animator.SetBool("up", false);
                animator.SetBool("down", false);

            }
            else
            {
                Debug.Log("Me muevo hacia la izquierda");
                SpRenderer.flipX = false;
                animator.SetBool("horizontal", true);
                animator.SetBool("up", false);
                animator.SetBool("down", false);
            }
        }
    }
}
