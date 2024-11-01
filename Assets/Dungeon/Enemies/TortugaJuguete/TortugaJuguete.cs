using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TortugaJuguete : Enemy
{
    private Vector2 playerPos;
    public float detectionRadio;
    [Tooltip("Cada cuanto tiempo se pone a caminar")]
    public float timeBetweenWalks;
    public float walkDuration;
    [Tooltip("No es exacto")]
    public float maxDistanceFromWalls;
    public bool activated=false;

    private Animator animator;
    private Vector2 facingDirection;
    public override void Start()
    {
        base.Start();
        playerPos = Player.GetComponent<Transform>().position;
        animator = GetComponent<Animator>();
    }
    public override void Update()
    {
        base.Update();
        if (GameManager.Instance.stop) return;
        if (!activated) StartCoroutine(Move());
        if (!hasKnockback) CheckMainDirection(rb2D.velocity.normalized);
    }
    public void FixedUpdate()
    {
        if (GameManager.Instance.stop) return;
    }

    IEnumerator Move()
    {
        activated = true;
        while (true) 
        {
            yield return new WaitForSecondsRealtime(timeBetweenWalks);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadio);
            bool playerDetected = false;
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.CompareTag("Player"))
                {
                    Vector2 playerDir = ((Vector2)Player.transform.position - (Vector2)transform.position).normalized;
                    rb2D.velocity= playerDir*Speed;
                    playerDetected = true;
                }
            }
            if (!playerDetected)
            {
                Vector2 randomDirection = GetRandomDirection();
                rb2D.velocity = randomDirection*Speed;
            }
            canRecieveKnockback = false;
            yield return new WaitForSeconds(walkDuration);
            rb2D.velocity=Vector2.zero;
            canRecieveKnockback = true;
        }
    }
    private Vector2 GetRandomDirection()
    {
        maxDistanceFromWalls = Speed * walkDuration;
        float magnitude = 0.01f;
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        while (true)
        {
            Ray ray = new Ray(transform.position, randomDirection);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction,magnitude);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Room"))
                {
                    if (magnitude >= maxDistanceFromWalls) return randomDirection;
                    else
                    {
                        randomDirection = Random.insideUnitCircle.normalized;
                        magnitude = 0.01f;
                    }
                }
            }
            magnitude+=0.5f;
        }
    }
    private void CheckMainDirection(Vector3 direction)
    {
        if (direction==Vector3.zero)
        {
            animator.SetBool("up", false);
            animator.SetBool("down", false);
            animator.SetBool("horizontal", false);
        }
        else if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (direction.y > 0)
            {
                facingDirection = Vector2.up;
                animator.SetBool("up", true);
                animator.SetBool("down", false);
                animator.SetBool("horizontal", false);
            }
            else
            {
                facingDirection= Vector2.down;
                animator.SetBool("down", true);
                animator.SetBool("up", false);
                animator.SetBool("horizontal", false);
            }
        }
        else
        {
            if (direction.x > 0)
            {
                facingDirection = Vector2.right;
                SpRenderer.flipX = false;
                animator.SetBool("horizontal", true);
                animator.SetBool("up", false);
                animator.SetBool("down", false);

            }
            else
            {
                SpRenderer.flipX = true;
                facingDirection = Vector2.left;
                animator.SetBool("horizontal", true);
                animator.SetBool("up", false);
                animator.SetBool("down", false);
            }
        }
    }
}
