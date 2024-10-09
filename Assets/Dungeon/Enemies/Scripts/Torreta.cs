using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torreta : Enemy
{
    public float fireRate;
    private float timeCounter;
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private float range;
    [SerializeField] private float shotSpeed;

    public override void Start()
    {
        base.Start();
        range = ProjectilePrefab.GetComponent<ProjectileScript>().Range;
    }

    public override void Update()
    {
        base.Update();
        timeCounter += Time.deltaTime;
        if(timeCounter >= (1/fireRate) && Vector2.Distance(transform.position, Player.transform.position) < range) {
            Shoot(Player.transform.position);
            timeCounter = 0;
        }
    }

    private void Shoot(Vector2 Target) {
        Vector2 direction = -((Vector2)transform.position - Target).normalized;
        Vector2 velocity = shotSpeed * direction;
        ProjectileCreator.Instance.createProjectile(
                ProjectilePrefab,
                transform.position,
                velocity,
                range,
                DamagePoints
                );
    }
}
