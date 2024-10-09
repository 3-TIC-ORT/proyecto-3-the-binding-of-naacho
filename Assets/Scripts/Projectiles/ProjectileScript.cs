using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float Range;
    public float Damage = .5f;
    public string[] WhitelistedTags = {"Player", "Projectile", "SpawnPoint", "Untagged", "RoomConector"}; // Tags the Projectile will pass through

    private Vector2 startingPos;
    private float lifespan = 0;

    void Start() {
        startingPos = transform.position;
    }

    void Update()
    {
       lifespan += Time.deltaTime;
       if(Vector2.Distance(startingPos, transform.position) >= Range) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player")) {
            foreach(string tag in WhitelistedTags)
                if(other.CompareTag(tag))
                    return;

            if(!other.CompareTag("Enemy") && lifespan < 0.025f) return;

            print($"Collided with {other.tag}");
            Destroy(gameObject);
        }
    }
}
