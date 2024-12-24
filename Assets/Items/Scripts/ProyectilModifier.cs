using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProyectilModifier : MonoBehaviour
{
    public enum Modifiers {
        None,
        FollowEnemies,
        DistanceSizeAndDamage,
        DividingProjectile,
        SlowEnemiesModifier,
        BouncingModifier,
        Cosenoidal,
        BlackHole,
        Rayo,
        IluminacionDivina
    };

    protected ProjectileScript proyectilScript;
    protected NaachoController controller;
    protected SpriteRenderer spriteRenderer;
    protected CircleCollider2D m_collider;
    public virtual void Start()
    {
        proyectilScript = GetComponent<ProjectileScript>();
        controller = PlayerManager.Instance.GetComponent<NaachoController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        m_collider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        
    }

    public virtual void OnDestroy()
    {
    }
}
