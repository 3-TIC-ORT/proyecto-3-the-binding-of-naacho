using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class IluminacionDivinaExt : MonoBehaviour
{
    public Vector3 initialPos;
    public float proyectilDamage;
    private bool isDisappearing;
    private float timeAlive;
    private float maxTimeAlive;
    private float deathSpeed;
    List<Enemy> enemiesAffected = new List<Enemy>();
    private float radius;
    void Start()
    {
        radius = ExternInitializer.Instance.IDradius;
        maxTimeAlive = ExternInitializer.Instance.IDmaxTimeAlive;
        deathSpeed = ExternInitializer.Instance.IDdeathSpeed;
        transform.position = initialPos;
    }

    void Update()
    {
        if (timeAlive < maxTimeAlive)
        {
            timeAlive += Time.deltaTime;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.CompareTag("Enemy"))
                {
                    Enemy enemy = collider.gameObject.GetComponent<Enemy>();
                    if (!enemiesAffected.Contains(enemy))
                    {
                        enemiesAffected.Add(enemy);
                        enemy.Damage(proyectilDamage * 10);
                    }
                }
            }
        }
        else if (!isDisappearing) Dissapear();
    }

    void Dissapear()
    {
        isDisappearing = true;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.DOColor(new Color(sr.color.r, sr.color.g, sr.color.b,0), deathSpeed).onComplete+=()=> { Destroy(gameObject); };
    }
}
