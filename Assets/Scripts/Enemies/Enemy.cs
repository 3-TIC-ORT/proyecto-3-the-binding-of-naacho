using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum ColliderType {
        Box,
        Capsule,
        Circle,
    }

    protected GameObject EnemyObj;

    public string Name;

    public float HealthPoints;
    public float DamagePoints;

    public uint Speed;

    protected SpriteRenderer SpRenderer;
    protected Color defaultColor;
    protected BoxCollider2D Col2D;
    protected Rigidbody2D rb2D;

    protected GameObject Player; 

    // Start is called before the first frame update
    public virtual void Start()
    {
        Player = GameObject.Find("Naacho");
        rb2D = GetComponent<Rigidbody2D>();
        Col2D = GetComponent<BoxCollider2D>();
        SpRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Damage(collision.GetComponent<ProjectileScript>().Damage);
        }
    }

    public virtual void Damage(float dp)
    {
        HealthPoints -= dp;
        StartCoroutine(VisualDamage());
        if (HealthPoints < 0) Destroy(gameObject);
    }
    public virtual IEnumerator VisualDamage()
    {
        SpRenderer.color = Color.red;

        while (SpRenderer.color.g + SpRenderer.color.b < 2f)
        {
            SpRenderer.color = new Color(SpRenderer.color.r, SpRenderer.color.g+0.05f, SpRenderer.color.b+0.05f);
            yield return null;
        }
        SpRenderer.color = defaultColor;
    }
}
