using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float Range;
    public float Damage = .5f;
    public string[] WhitelistedTags = {"Player", "Projectile", "SpawnPoint", "Untagged", "RoomConector", "BigRoomTrigger"}; // Tags the Projectile will pass through
    public bool isEnemy;
    public Rigidbody2D rb2D;

    private Vector2 startingPos;
    private float lifespan = 0;

    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        startingPos = transform.position;
        if(isEnemy) WhitelistedTags[0] = "Enemy";
    }

    void Update()
    {
        lifespan += Time.deltaTime;
        if(Vector2.Distance(startingPos, transform.position) >= Range) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        foreach(string tag in WhitelistedTags)
            if(other.CompareTag(tag)) return;
        //print($"Collided with {other.tag}");
        if(!isEnemy && other.CompareTag("Enemy")) {
            Rigidbody2D enemrb;
            if(other.TryGetComponent<Rigidbody2D>(out enemrb)) {
                if(!other.GetComponent<Enemy>().isBoss && other.GetComponent<Enemy>().canRecieveKnockback)
                StartCoroutine(ApplyKnockback(enemrb));
            }
        }
        Destroy(gameObject);
        
        return;
    }

    private IEnumerator ApplyKnockback(Rigidbody2D enemrb) {
        print($"{enemrb.velocity}");
        enemrb.GetComponent<Enemy>().hasKnockback = true;
        enemrb.AddForce(rb2D.velocity * 50, ForceMode2D.Force);
        print($"{enemrb.velocity}");
        yield return new WaitForSeconds(0.2f);
        print($"{enemrb.velocity}");
        enemrb.GetComponent<Enemy>().hasKnockback = false;
        print($"{enemrb.velocity}");
    }
}
