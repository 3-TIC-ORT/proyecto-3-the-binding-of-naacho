using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleExt : MonoBehaviour
{
    [SerializeField] float attractionForce;
    [SerializeField] float attractionRadius;
    public Enemy impactedEnemy;
    private SpriteRenderer sr;
    [SerializeField] Material blackHoleMaterial;
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
        blackHoleMaterial = sr.material;
        blackHoleMaterial.SetFloat(timerID, 0.26f);
        blackHoleMaterial.SetFloat(currentAlphaID, 1);
        blackHoleMaterial.SetColor(colorID, ringColor);
    }

    void Update()
    {

        blackHoleMaterial.SetFloat(timerID, blackHoleMaterial.GetFloat(timerID) + Time.deltaTime * animationSpeed);
        blackHoleMaterial.SetFloat(currentAlphaID, blackHoleMaterial.GetFloat(currentAlphaID) - Time.deltaTime * (animationSpeed / alphaDelay));
        if (blackHoleMaterial.GetFloat(timerID)>1)
        {
            foreach (Enemy enemy in enemiesAttracted) 
            {
                enemy.effects.theMostAttractiveBlackHole = null;
            }
            Destroy(gameObject);
            //Debug.Log("Terminé");
            //ResetBlackHoleMaterial();
        }
    }
    private void FixedUpdate()
    {
        AttractEnemies();
    }


    void ResetBlackHoleMaterial()
    {
        Destroy(gameObject);
        blackHoleMaterial.SetFloat(currentAlphaID, 1);
        blackHoleMaterial.SetColor(colorID, ringColor);
        blackHoleMaterial.SetFloat(timerID, 0.26f);
    }

    void AttractEnemies()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attractionRadius);
        foreach (Collider2D collider in colliders) 
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = collider.gameObject.GetComponent<Enemy>();
                if (enemy!=null && !enemy.isBoss && enemy.canRecieveKnockback)
                {
                    if (enemy!=null && enemy.effects!=null && (enemy.effects.theMostAttractiveBlackHole == this || enemy.effects.theMostAttractiveBlackHole == null))
                    {
                        enemy.effects.theMostAttractiveBlackHole = this;
                        if (!enemiesAttracted.Contains(enemy)) enemiesAttracted.Add(enemy);
                        Vector2 direction = (transform.position - collider.gameObject.transform.position).normalized;
                        //collider.gameObject.GetComponent<Rigidbody2D>().velocity += direction * attractionForce ;
                        collider.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * attractionForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
                    }
                }
            }
        }
    }
}
