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
    public bool dontDestroyWhenCollided;
    public bool dontDestroyWhenDistance;
    [Tooltip("Frames para ignorar cuando spawnea el projectil")]
    [SerializeField] private int InitialIgnoreFrames = 4;
    private int lifespan = 0;

    public CircleCollider2D col;
    public Rigidbody2D rb2D;
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
        if (!dontDestroyWhenDistance)
        {
            if(Vector2.Distance(startingPos, transform.position) >= Range) {
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
                other.GetComponent<Enemy>().Damage(Damage);
                if(!other.GetComponent<Enemy>().isBoss && other.GetComponent<Enemy>().canRecieveKnockback) {
                    other.GetComponent<Enemy>().hasKnockback = true;
                    StartCoroutine(ApplyKnockback(enemrb));
                }
            }
        }
        if (!dontDestroyWhenCollided)
        {
            onDestruction();
            Destroy(gameObject, 0.1f);
        }
    }

    private IEnumerator ApplyKnockback(Rigidbody2D enemrb) {
        //print($"{enemrb.velocity}");
        enemrb.GetComponent<Enemy>().hasKnockback = true;
        enemrb.AddForce(rb2D.velocity * 50, ForceMode2D.Force);
        //print($"{enemrb.velocity}");
        yield return new WaitForSeconds(0.2f);
        //print($"{enemrb.velocity}");
        enemrb.GetComponent<Enemy>().hasKnockback = false;
        //print($"{enemrb.velocity}");
    }

    protected virtual void onDestruction() {}
}
