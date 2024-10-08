using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected GameObject EnemyObj;

    public string Name;

    public float HealthPoints;
    public float DamagePoints;

    public uint Speed;

    protected SpriteRenderer SpRenderer;
    protected Color defaultColor;
    protected BoxCollider2D Col2D;
    protected Rigidbody2D rb2D;
    protected bool isBoss;
    protected GameObject Player;
    protected float maxHealth;
    // Es para leerlo desde otros scripts. No se usa.
    public float readableMaxHealth;

    // Start is called before the first frame update
    public virtual void Start()
    {
        Player = GameObject.Find("Naacho");
        rb2D = GetComponent<Rigidbody2D>();
        Col2D = GetComponent<BoxCollider2D>();
        SpRenderer = GetComponent<SpriteRenderer>();
        defaultColor = SpRenderer.color;
        isBoss = false;
        maxHealth = HealthPoints;
        readableMaxHealth = maxHealth;
    }

    // Update is called once per frame
    public virtual void Update() {
        if (Player != null) Player.GetComponent<DoorDisabler>().isFighting = true;
        else if (!GameManager.Instance.nachoNullPrinted)
        {
            Debug.LogWarning("Che macho, Naacho es null");
            GameManager.Instance.nachoNullPrinted = true;
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile") && !collision.GetComponent<ProjectileScript>().isEnemy)
        {
            Damage(collision.GetComponent<ProjectileScript>().Damage);
        }
    }

    public virtual void Damage(float dp)
    {
        HealthPoints -= dp;
        StartCoroutine(VisualDamage());
        if (HealthPoints <= 0) {
            OnDeath();
            if (isBoss) Destroy(GameObject.Find("BossBar"));
            Destroy(gameObject);
        }
    }

    public virtual void OnDeath() {}

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
