using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float Range;
    public float Damage = .5f;
    public string[] WhitelistedTags = {"Player", "Projectile", "SpawnPoint", "Untagged", "RoomConector", "BigRoomTrigger"}; // Tags the Projectile will pass through
    public bool isEnemy;
    public Vector3 InitialVelocity;

    [Tooltip("Frames para ignorar cuando spawnea el projectil")]
    [SerializeField] private int InitialIgnoreFrames = 4;
    private int lifespan = 0;

    private CircleCollider2D col;
    private Rigidbody2D rb2D;
    private Vector2 startingPos;

    protected virtual void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        InitialVelocity = rb2D.velocity;
        col = GetComponent<CircleCollider2D>();
        startingPos = transform.position;
        if(isEnemy) WhitelistedTags[0] = "Enemy";
    }

    void Update()
    {
        lifespan++;
        if(Vector2.Distance(startingPos, transform.position) >= Range) {
            onDestruction();
            Destroy(gameObject);
        }
        if(lifespan >= InitialIgnoreFrames) {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, col.radius/2);
            foreach(Collider2D collider in colliders) {
                if(WhitelistedTags.Contains(collider.tag)) {
                    continue;
                }
                onDestruction();
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(lifespan < InitialIgnoreFrames) {
            return;
        }
        foreach(string tag in WhitelistedTags)
            if(other.CompareTag(tag)) return;
        //print($"Collided with {other.tag}");
        if(!isEnemy && other.CompareTag("Enemy")) {
            Rigidbody2D enemrb;
            if(other.TryGetComponent<Rigidbody2D>(out enemrb)) {
                if(!other.GetComponent<Enemy>().isBoss && other.GetComponent<Enemy>().canRecieveKnockback) {
                    other.GetComponent<Enemy>().Damage(Damage);
                    other.GetComponent<Enemy>().hasKnockback = true;
                    StartCoroutine(ApplyKnockback(enemrb));
                }
            }
        }
        onDestruction();
        Destroy(gameObject, 0.1f);
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

    protected virtual void onDestruction() {}
}
