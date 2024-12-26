using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAwayExt : MonoBehaviour
{
    [SerializeField] float repulsionForce;
    [SerializeField] float repulsionRadius;
    private SpriteRenderer sr;
    [SerializeField] Material pushAwayMaterial;
    [SerializeField] float animationSpeed;
    [SerializeField] float alphaDelay;
    [SerializeField] Color ringColor;
    [SerializeField] string timerID;
    [SerializeField] string colorID;
    [SerializeField] string currentAlphaID;
    List<Enemy> enemiesAttracted = new List<Enemy>();
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        pushAwayMaterial = sr.material;
        pushAwayMaterial.SetFloat(timerID, 0.26f);
        pushAwayMaterial.SetFloat(currentAlphaID, 1);
        pushAwayMaterial.SetColor(colorID, ringColor);
    }

    void Update()
    {
        pushAwayMaterial.SetFloat(timerID, pushAwayMaterial.GetFloat(timerID) + Time.deltaTime * animationSpeed);
        pushAwayMaterial.SetFloat(currentAlphaID, pushAwayMaterial.GetFloat(currentAlphaID) - Time.deltaTime * (animationSpeed / alphaDelay));
        if (pushAwayMaterial.GetFloat(timerID) > 1)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        RepelEnemies();
    }
    void RepelEnemies()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, repulsionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = collider.gameObject.GetComponent<Enemy>();
                if (enemy != null && !enemy.isBoss && enemy.canRecieveKnockback && !PlayerManager.Instance.GetComponent<NaachoHeartSystem>().dead)
                {
                    Vector2 direction = (collider.gameObject.transform.position - transform.position).normalized;
                    collider.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * repulsionForce * Time.fixedDeltaTime, ForceMode2D.Impulse);   
                }
            }
        }
    }
}
