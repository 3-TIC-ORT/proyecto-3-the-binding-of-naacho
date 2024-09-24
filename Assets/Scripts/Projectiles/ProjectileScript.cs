using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float TotalLifespan;
    public float Damage = .5f;
    private float lifespan = 0;
    [SerializeField] private string[] WhitelistedTags = {"Player", "Projectile", "SpawnPoint", "Untagged", "RoomConector"}; // Tags the Projectile will pass through

    // Update is called once per frame
    void Update()
    {
       lifespan += Time.deltaTime;
       if(lifespan >= TotalLifespan)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player")) {
            foreach(string tag in WhitelistedTags)
                if(other.CompareTag(tag))
                    return;

            if(!other.CompareTag("Enemy") && lifespan < 0.05f) return;

            print($"Collided with {other.tag}, {lifespan}");
            Destroy(gameObject);
        }
    }
}
