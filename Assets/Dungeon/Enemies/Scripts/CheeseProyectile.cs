using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseProyectile : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    public Vector3 angle;
    public GameObject cheeseEnemy;
    public BoxCollider2D rataCollider;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(speed * angle, ForceMode2D.Impulse);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        string colTag = col.gameObject.tag;
        if (colTag == "Player" || colTag == "Room")
        {
            if (colTag == "Player")
            {
                col.gameObject.GetComponent<NaachoHeartSystem>().Damage(1);
                Destroy(gameObject);
            }
            else if (Random.value < 0.2f)
            {
                GameObject queso = Instantiate(cheeseEnemy, transform.position, Quaternion.identity, transform.parent);
                EnemyEnabler enemyEnabler = queso.GetComponent<EnemyEnabler>();
                enemyEnabler.GetComponentsReferences();
                enemyEnabler.SetComponents(true);
                Physics2D.IgnoreCollision(queso.GetComponent<BoxCollider2D>(), rataCollider);
                Destroy(gameObject);
            }
            Destroy(gameObject);
        }
    }
}
