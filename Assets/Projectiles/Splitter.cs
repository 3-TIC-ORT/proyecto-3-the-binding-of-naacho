using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : MonoBehaviour
{
    public GameObject splittingPrefab;
    public float damageSplitMult = 0.5f;
    public int childAmount = 3;

    float splitSpeed;
    float range;
    float damage;

    void Start() {
        Vector3 velocity = GetComponent<Rigidbody2D>().velocity;

        splitSpeed = (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y)) ? velocity.x : velocity.y;

        range = GetComponent<ProjectileScript>().Range;
        damage = GetComponent<ProjectileScript>().Damage;
    }

    void OnDestroy() {
        for(int i = 0; i < childAmount; i++) {
            Vector3 direction = new Vector3(Random.Range(-1.1f, 1.1f), Random.Range(-1.1f, 1.1f)).normalized;
            Vector3 velocity = new Vector3(direction.x * splitSpeed, direction.y * splitSpeed);
            ProjectileCreator.createProjectile(
                    splittingPrefab, 
                    transform.position,
                    velocity,
                    range,
                    damage * damageSplitMult
                    );
        }
    }
}
