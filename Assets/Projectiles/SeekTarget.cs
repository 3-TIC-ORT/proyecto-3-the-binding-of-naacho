using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekTarget : MonoBehaviour
{
    public bool isEnemy;
    public int detectionRadius;
    public float force;
    [SerializeField] private GameObject Target;
    private Rigidbody2D rb2D;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        if(isEnemy)
            Target = GameObject.Find("Naacho");
        else {
            Target = GetClosestEnemy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Target == null) {
            Target = GetClosestEnemy();
            return;
        }

        Vector2 direction = (Target.transform.position - transform.position).normalized;

        rb2D.AddForce(direction * force);
    }

    private GameObject GetClosestEnemy() {
        GameObject closestEnemy = GameObject.FindGameObjectsWithTag("Enemy")
            .OrderBy(enemy => Vector2.Distance(enemy.transform.position, transform.position))
            .FirstOrDefault();
        if(Vector2.Distance(closestEnemy.transform.position, transform.position) < detectionRadius)
            return closestEnemy;
        else return null;
    }
}
