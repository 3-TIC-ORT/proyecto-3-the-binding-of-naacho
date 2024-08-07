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

    protected string Name;

    protected GameObject EnemyObj;

    protected float HealthPoints;
    protected float DamagePoints;

    protected uint Speed;

    protected ProjectileCreator projectileCreator;
    protected Sprite EnemySprite;
    protected SpriteRenderer SpRenderer;
    protected Collider2D Col2D;
    protected Rigidbody2D rb2D;

    Enemy(ColliderType Col2D, Sprite sprite, float hp = 3f, float dp = 0.5f, uint speed = 350, string name = "Enemy") {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
