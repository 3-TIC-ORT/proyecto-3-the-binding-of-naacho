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
    public float distanceFromWalls;
    public bool activated=false;
    public override void Start()
    {
        base.Start();
        playerPos = Player.GetComponent<Transform>().position;  
    }
    public override void Update()
    {
        base.Update();
        if (GameManager.Instance.stop) return;
        if (!activated) StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        activated = true;
        while (true) 
        {
            Debug.Log("ASD");
            yield return new WaitForSecondsRealtime(timeBetweenWalks);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadio);
            bool playerDetected = false;
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.CompareTag("Player"))
                {
                    Vector2 playerDir = ((Vector2)Player.transform.position - (Vector2)transform.position).normalized;
                    rb2D.velocity= playerDir*Speed*Time.deltaTime;
                    playerDetected = true;
                }
            }
            if (!playerDetected)
            {
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                rb2D.velocity = randomDirection*Speed*Time.deltaTime;
            }
            yield return new WaitForSeconds(walkDuration);
            rb2D.velocity=Vector2.zero;
        }
    }
    private Vector2 GetRandomDirection()
    {
        float magnitude = 0.01f;
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        while (true)
        {
            Ray ray = new Ray(transform.position, randomDirection);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction,magnitude);
            foreach (RaycastHit2D hit in hits)
            {  
                //if (hit.collider.gameObject.CompareTag("Room"))
                //{
                //    if (magnitude)
                //}
            }
        }
    }
}
