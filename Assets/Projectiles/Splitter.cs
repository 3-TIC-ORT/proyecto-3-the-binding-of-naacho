using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : ProjectileScript
{
    public GameObject splittingPrefab;
    public float DamageSplitMult = 0.5f;
    public float RangeMult = 0.1f;
    public int childAmount = 3;

    float splitSpeed;

    override protected void Start() {
        base.Start();
        splitSpeed = (Mathf.Abs(InitialVelocity.x) > Mathf.Abs(InitialVelocity.y)) ? 
            InitialVelocity.x : InitialVelocity.y;
    }

    override protected void onDestruction() {
        for(int i = 0; i < childAmount; i++) {
            Vector3 direction = new Vector3(Random.Range(-1.1f, 1.1f), Random.Range(-1.1f, 1.1f)).normalized;
            Vector3 velocity = new Vector3(direction.x * splitSpeed, direction.y * splitSpeed);
            ProjectileCreator.createProjectile(
                    splittingPrefab, 
                    transform.position,
                    velocity,
                    Range,
                    Damage * DamageSplitMult
                    );
        }
    }
}
