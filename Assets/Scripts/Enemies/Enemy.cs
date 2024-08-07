using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected float HealthPoints;
    protected float DamagePoints;

    protected uint Speed;

    protected ProjectileCreator projectileCreator;
    protected Sprite EnemySprite;
    protected Collider2D Col2D;
    protected Rigidbody2D rb2D;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
