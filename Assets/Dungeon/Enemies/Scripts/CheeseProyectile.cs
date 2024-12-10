using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseProyectile : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    public Vector3 angle;
    public GameObject cheeseEnemy;
    private GameObject queso;
    private EnemyEnabler enemyEnabler;
    public BoxCollider2D rataCollider;
    private bool willGenerateCheese;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(speed * angle, ForceMode2D.Impulse);
        willGenerateCheese = Random.value < 0.2f;
        GameObject particle = ParticlesManager.Instance.CheeseBallParticle;
        if (willGenerateCheese)
        {
            StartCoroutine(CreateCheese());
            ParticlesManager.Instance.InstanceParticle(particle, transform.position, transform);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        string colTag = col.gameObject.tag;
        if (colTag == "Player" || colTag == "Room")
        {
            if (colTag == "Player")
            {
                col.gameObject.GetComponent<NaachoHeartSystem>().Damage();
            }
            if (willGenerateCheese)
            {
                queso.transform.position = transform.position;
                queso.SetActive(true);
                enemyEnabler.SetComponents(true);   
                if (rataCollider != null)
                {
                    Physics2D.IgnoreCollision(queso.GetComponent<BoxCollider2D>(), rataCollider);
                }
            }
            Destroy(gameObject);
        }
    }
    IEnumerator CreateCheese()
    {
        queso = Instantiate(cheeseEnemy, transform.position, Quaternion.identity, transform.parent);
        enemyEnabler = queso.GetComponent<EnemyEnabler>();
        enemyEnabler.GetComponentsReferences();
        enemyEnabler.SetComponents(true);
        yield return null;
        enemyEnabler.SetComponents(false);
        queso.SetActive(false);
    }
}
