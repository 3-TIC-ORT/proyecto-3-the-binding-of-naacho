using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RataJefe : Enemy
{
    private Animator animator;
    private AnimatorStateInfo animStateInfo;
    private Transform playerPos;
    private Rigidbody2D rb;
    private int[] attacksIdentifications = { 0, 1, 2 };
    [Tooltip("Tener en cuenta que si el valor es muy bajo entonces las animaciones podrían romperse")]
    public float delayBetweenAttacks;
    [Header("Embestida Stuff")]
    public float embestidaSpeed;
    public float embestidaDuration;
    public float finishEmbestidaDuration;
    [Header("Cheese Ball Stuff")]
    [Tooltip("El eje X representa la cantidad de cheeseBalls a generar. El eje Y representa la variación del ángulo")]
    public Vector2[] ballsConfiguration;
    public GameObject cheeseBall;
    public float cheeseBallSpeed;
    public float timeBeforeFight;
    private bool isThrowingBalls;

    public override void Update()
    {
        base.Update();
        animator = GetComponent<Animator>();
        if (GameManager.Instance.stop) return;
        Vector2 playerDir = (Vector2)(Player.transform.position - transform.position).normalized;
        CheckMainDirection(playerDir);
        animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
    }
    public override void Start()
    {
        base.Start();
        canRecieveKnockback = false;
        isBoss = true;
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine("Attack", timeBeforeFight);
        //animator = gameObject.GetComponent<Animator>();
    }

    IEnumerator Attack()
    {
        while (!animStateInfo.IsName("idle"))
        {
            yield return null;
        }
        AdjustColliderWithSprite();
        //yield return new WaitForSecondsRealtime(delayBetweenAttacks * (HealthPoints / maxHealth));
        yield return new WaitForSecondsRealtime(delayBetweenAttacks);
        int rand = Random.Range(0, attacksIdentifications.Length);
        if (rand == 0) StartCoroutine(Embestida());
        else if (rand == 1) StartCoroutine(CreateCheeseEnemy());
        else if (rand == 2) StartCoroutine(CheeseBallsAttack());
    }
    IEnumerator Embestida()
    {
        animator.SetTrigger("EmbestidaEmpezar");
        while (!animStateInfo.IsName("embestida"))
        {
            yield return null;
        }
        AdjustColliderWithSprite();
        Vector2 AttackDirection = (playerPos.position - transform.position).normalized;
        //rb.AddForce(AttackDirection * embestidaSpeed * (2 - HealthPoints / maxHealth), ForceMode2D.Impulse);
        rb.AddForce(AttackDirection * embestidaSpeed, ForceMode2D.Impulse);

        yield return new WaitForSecondsRealtime(embestidaDuration);

        animator.SetTrigger("EmbestidaFinalizar");
        DOTween.To(() => rb.velocity, x => rb.velocity = x, Vector2.zero, finishEmbestidaDuration).onComplete=()=> 
        {
            StartCoroutine(Attack()); 
        };
    }
    IEnumerator CreateCheeseEnemy()
    {
        animator.SetTrigger("Escupir");
        while(!animStateInfo.IsName("escupirFinalizar"))
        {
            yield return null;
        }
        GetComponent<CheeseEnemyGenerator>().GenerateCheese(GetComponent<BoxCollider2D>());
        StartCoroutine(Attack());
    }
    IEnumerator CheeseBallsAttack()
    {
        animator.SetTrigger("Jump");
        Vector2 configuration = ballsConfiguration[Random.Range(0,ballsConfiguration.Length)];
        while(!animStateInfo.IsName("jumpFinalizar"))
        {
            yield return null;
        }
        GenerateBalls((int)configuration.x, configuration.y);
        yield return new WaitForSecondsRealtime(1.5f);
        StartCoroutine(Attack());
            
    }
    void GenerateBalls(int ballsAmount,float angleVariation)
    {
        int ballsGenerated = 0;
        while (ballsGenerated <= ballsAmount)
        {
            float angle = ((360 / ballsAmount) * ballsGenerated + angleVariation) * Mathf.Deg2Rad;
            float X = Mathf.Cos(angle) * Mathf.Rad2Deg;
            float Y = Mathf.Sin(angle) * Mathf.Rad2Deg;
            Vector3 vectorAngle = new Vector3(X, Y, 0).normalized;
            GameObject proyectil = Instantiate(cheeseBall, transform.position, Quaternion.identity,transform.parent);
            CheeseProyectile proyectilScript = proyectil.GetComponent<CheeseProyectile>();
            proyectilScript.angle = vectorAngle;
            proyectilScript.speed = cheeseBallSpeed;
            proyectilScript.rataCollider=GetComponent<BoxCollider2D>();
            ballsGenerated++;
        }
    }

    private void AdjustColliderWithSprite()
    {
        if (SpRenderer != null && Col2D!=null)
        {
            Col2D.size=SpRenderer.bounds.size;
            Col2D.offset = SpRenderer.bounds.center - transform.position;
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
