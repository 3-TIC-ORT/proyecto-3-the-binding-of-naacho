using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RataJefe : Enemy
{
    private Animator animator;
    private Transform playerPos;
    private Rigidbody2D rb;
    private int[] attacksIdentifications = { 0, 1, 2 };
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
    private bool isThrowingBalls;
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
        yield return new WaitForSecondsRealtime(delayBetweenAttacks * (HealthPoints / maxHealth));
        int rand = Random.Range(0, attacksIdentifications.Length);
        if (rand == 0) StartCoroutine(Embestida());
        else if (rand == 1) StartCoroutine(CreateCheeseEnemy());
        else if (rand == 2) StartCoroutine(CheeseBallsAttack());
    }
    IEnumerator Embestida()
    {
        Vector2 AttackDirection = (playerPos.position - transform.position).normalized;
        rb.AddForce(AttackDirection * embestidaSpeed * (2 - HealthPoints / maxHealth) * Time.deltaTime, ForceMode2D.Impulse);

        yield return new WaitForSecondsRealtime(embestidaDuration * (HealthPoints / maxHealth));

        DOTween.To(() => rb.velocity, x => rb.velocity = x, Vector2.zero, finishEmbestidaDuration* (HealthPoints / maxHealth));

        yield return new WaitForSecondsRealtime(finishEmbestidaDuration);

        StartCoroutine(Attack());
    }
    IEnumerator CreateCheeseEnemy()
    {
        GetComponent<CheeseEnemyGenerator>().GenerateCheese(GetComponent<BoxCollider2D>());
        yield return null;
        StartCoroutine(Attack());
    }
    IEnumerator CheeseBallsAttack()
    {
        Vector2 configuration = ballsConfiguration[Random.Range(0,ballsConfiguration.Length)];
        GenerateBalls((int)configuration.x, configuration.y);
        yield return null;
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
