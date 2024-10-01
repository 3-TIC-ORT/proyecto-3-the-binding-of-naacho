using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LechugaGrande : Enemy
{
    private Animator animator;
    [SerializeField] private GameObject Lechuguita;
    [SerializeField] private int LechugitaOffspring;

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
        CheckMainDirection(playerDir);
    }

    private void CheckMainDirection(Vector3 direction)
    {
        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", true);
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

    public override void OnDeath()
    {
        for(int i = 0; i < LechugitaOffspring; i++) {
            Vector3 lechuguitaPos = new Vector3(transform.position.x + Random.Range(-100, 100)/100, transform.position.y + Random.Range(-100, 100)/100);
            Instantiate(Lechuguita, lechuguitaPos, Quaternion.identity);
        }
    }
}
