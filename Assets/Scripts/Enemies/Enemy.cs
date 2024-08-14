using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
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

    protected ProjectileCreator projectileCreator;
    public Sprite EnemySprite;
    protected SpriteRenderer SpRenderer;
    protected BoxCollider2D Col2D;
    protected Rigidbody2D rb2D;

    protected GameObject Player; 

    public void InitEnemy(Vector2 pos, Vector2 scale) {
        EnemyObj = new GameObject(Name);
        projectileCreator = EnemyObj.AddComponent<ProjectileCreator>();

        SpRenderer = EnemyObj.AddComponent<SpriteRenderer>();
        SpRenderer.sprite = EnemySprite;

        rb2D = EnemyObj.AddComponent<Rigidbody2D>();
        rb2D.gravityScale = 0;

        Col2D = EnemyObj.AddComponent<BoxCollider2D>();

        EnemyObj.transform.position = pos;

        EnemyObj.tag = "Enemy";

        EnemyObj.transform.localScale = scale;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        Player = GameObject.Find("Naacho");
    }

    // Update is called once per frame
    public virtual void Update()
    {
    }

    public virtual void EnemyTriggerEnter2D(Collision2D other) {
        
    }
}
