using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class Enemy// : MonoBehaviour
{
    public enum ColliderType {
        Box,
        Capsule,
        Circle,
    }

    public GameObject EnemyObj;

    public string Name;

    protected float HealthPoints;
    protected float DamagePoints;

    protected uint Speed;

    protected ProjectileCreator projectileCreator;
    protected Sprite EnemySprite;
    protected SpriteRenderer SpRenderer;
    protected BoxCollider2D Col2D;
    protected Rigidbody2D rb2D;

    protected GameObject Player; 

    public Enemy(Sprite sprite, float hp = 3f, float dp = 0.5f, uint speed = 350, string name = "Enemy") {
        Name = name;
        HealthPoints = hp;
        DamagePoints = dp;
        Speed = speed;
        EnemySprite = sprite;
    }

    public void InitEnemy(ColliderType colType, Vector2 pos, Vector2 scale) {
        EnemyObj = new GameObject(Name);
        projectileCreator = EnemyObj.AddComponent<ProjectileCreator>();

        SpRenderer = EnemyObj.AddComponent<SpriteRenderer>();
        SpRenderer.sprite = EnemySprite;

        rb2D = EnemyObj.AddComponent<Rigidbody2D>();

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
}
