using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RataJefe : Enemy
{
    private Animator animator;
    private Transform playerPos;
    private Rigidbody2D rb;
    public float embestidaSpeed;
    public float embestidaDuration;
    public float finishEmbestidaDuration;
    public override void Update()
    {
        base.Update();
        if (GameManager.Instance.stop) return;
        Vector2 playerDir = (Vector2)(Player.transform.position - transform.position).normalized;
        CheckMainDirection(playerDir);
    }
    public override void Start()
    {
        base.Start();
        isBoss = true;
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Attack());
        //animator = gameObject.GetComponent<Animator>();
    }

    IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(2f);
            GetComponent<CheeseEnemyGenerator>().GenerateCheese();
            yield return new WaitForSecondsRealtime(2f);
            Vector2 AttackDirection = (playerPos.position - transform.position).normalized;
            rb.AddForce(AttackDirection * embestidaSpeed*Time.deltaTime, ForceMode2D.Impulse);
            yield return new WaitForSecondsRealtime(embestidaDuration);
            DOTween.To(() => rb.velocity, x => rb.velocity = x, Vector2.zero, finishEmbestidaDuration);
            yield return new WaitForSecondsRealtime(finishEmbestidaDuration);
        }
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
