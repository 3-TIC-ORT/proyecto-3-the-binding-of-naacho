using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected enum ColliderType {
        Box,
        Capsule,
        Circle,
    }

    public GameObject EnemyObj;

    protected float HealthPoints;
    protected float DamagePoints;

    protected uint Speed;

    protected ProjectileCreator projectileCreator;
    protected Sprite EnemySprite;
    protected SpriteRenderer SpRenderer;
    protected Collider2D Col2D;
    protected Rigidbody2D rb2D;

    Enemy(ColliderType colType, Sprite sprite, float hp = 3f, float dp = 0.5f, uint speed = 350, string name = "Enemy") {
        HealthPoints = hp;
        DamagePoints = dp;
        Speed = speed;
        
        EnemyObj = new GameObject(name);
        projectileCreator = EnemyObj.AddComponent<ProjectileCreator>();

        SpRenderer = EnemyObj.AddComponent<SpriteRenderer>();
        SpRenderer.sprite = sprite;

        rb2D = EnemyObj.AddComponent<Rigidbody2D>();

        switch(colType) {
            case ColliderType.Box:
                Col2D = EnemyObj.AddComponent<BoxCollider2D>();
                break;
            case ColliderType.Capsule:
                Col2D = EnemyObj.AddComponent<CapsuleCollider2D>();
                break;
            case ColliderType.Circle:
                Col2D = EnemyObj.AddComponent<CircleCollider2D>();
                break;
        }
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }
}
