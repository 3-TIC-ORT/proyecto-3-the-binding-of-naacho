using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float Range;
    public float Damage = .5f;
    public string[] WhitelistedTags = {"Player", "Projectile", "SpawnPoint", "Untagged", "RoomConector"}; // Tags the Projectile will pass through

    private Vector2 startingPos;
    private Vector2 prevFrameDist;
    private float lifespan = 0;
    private Rigidbody2D rb2D;
    private float distanceTraveled = 0;


    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        startingPos = transform.position;
        prevFrameDist = transform.position;
    }

    void Update()
    {
       lifespan += Time.deltaTime;
       distanceTraveled += Vector2.Distance(transform.position, prevFrameDist);
       if(distanceTraveled >= Range) Destroy(gameObject);
       prevFrameDist = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player")) {
            foreach(string tag in WhitelistedTags)
                if(other.CompareTag(tag))
                    return;

            if(!other.CompareTag("Enemy") && lifespan < 0.025f) return;

            Destroy(gameObject);
        }
    }
}
