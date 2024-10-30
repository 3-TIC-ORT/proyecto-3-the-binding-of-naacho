using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LechugaGrande : Enemy
{
    private Animator animator;
    [SerializeField] private GameObject Lechuguita;
    [SerializeField] private int LechugitaOffspring;
    [SerializeField] private int LechuguitaDelay;

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

    public override IEnumerator OnDeath()
    {
        yield return StartCoroutine(SpawnLechuguitas(LechugitaOffspring, LechuguitaDelay));
        yield return base.OnDeath();
    }

    private IEnumerator SpawnLechuguitas(int amount, int frameDelay) {
        for(; amount > 0; amount--) {
            Vector3 lechuguitaPos = new Vector3(transform.position.x
                + Random.Range(-100, 100)
                / 50, transform.position.y + Random.Range(-100, 100)/50);

            Instantiate(Lechuguita, lechuguitaPos, Quaternion.identity);
            for(int f = 0; f < frameDelay; f++) {
                yield return null;
            }
        }
    }

    public override void Damage(float dp)
    {
        base.Damage(dp);
        if(Random.Range(0, 100) < 5) {
            Vector3 lechuguitaPos = new Vector3(transform.position.x
                + Random.Range(-100, 100)
                / 50, transform.position.y + Random.Range(-100, 100)/50);

            Instantiate(Lechuguita, lechuguitaPos, Quaternion.identity);
        }
    }
}
