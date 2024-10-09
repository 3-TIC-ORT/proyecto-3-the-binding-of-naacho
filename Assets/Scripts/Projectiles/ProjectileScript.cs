using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float Range;
    public float Damage = .5f;
    public string[] WhitelistedTags = {"Player", "Projectile", "SpawnPoint", "Untagged", "RoomConector"}; // Tags the Projectile will pass through
    public bool isEnemy;

    private Vector2 startingPos;
    private float lifespan = 0;

    void Start() {
        startingPos = transform.position;
        if(isEnemy) WhitelistedTags[0] = "Enemy";
    }

    void Update()
    {
        lifespan += Time.deltaTime;
        if(Vector2.Distance(startingPos, transform.position) >= Range) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.gameObject.CompareTag(WhitelistedTags[0])) { /* Idx 0 is meant to be either the player or enemy */
            foreach(string tag in WhitelistedTags)
                if(other.CompareTag(tag))
                    return;

            print($"Collided with {other.tag}");
            Destroy(gameObject);
        }
        return;
    }
}
