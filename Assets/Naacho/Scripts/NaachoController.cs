using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaachoController : MonoBehaviour
{
    public int Speed;
    public float Friction;
    bool haveToMove;
    public GameObject ProjectilePrefab;
    public float shotSpray;
    public float shootSpeed;
    public float FireRate; // Amount of projectiles per second
    public float Range;
    public float Damage;
    private float ShootTimeCounter = 0;

    [SerializeField] Vector2 ShootingOffset;
    private ProjectileCreator ProjectileScript;
    private Rigidbody2D rb2D;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.stop)
        {
            rb2D.velocity = Vector2.zero;
            animator.SetBool("Idle", true);
            animator.SetFloat("DirY", 0);
            animator.SetFloat("DirX", 0);
            return;
        }

        Vector2 movement = getMovement().normalized;

        if (movement != Vector2.zero)
        {
            rb2D.velocity = Speed * movement;
            animator.SetFloat("DirY", movement.y);
            animator.SetFloat("DirX", movement.x);
            animator.SetBool("Idle", false);
        }
        else 
        {
            rb2D.velocity *= Friction;

            animator.SetBool("Idle", true);
            animator.SetFloat("DirY", 0);
            animator.SetFloat("DirX", 0);
            if (rb2D.velocity.magnitude < 0.1f)
            {
                rb2D.velocity = Vector2.zero;
            }
        }
    }
    Vector2 getMovement() 
    {
        int horizontalMovement = 0;
        int verticalMovement = 0;

        if(Input.GetKey(KeyCode.D))
            horizontalMovement += 1;
        if(Input.GetKey(KeyCode.A))
            horizontalMovement -= 1;
        
        if(Input.GetKey(KeyCode.W))
            verticalMovement += 1;
        if(Input.GetKey(KeyCode.S))
            verticalMovement -= 1;

        return new Vector2(horizontalMovement, verticalMovement);
    }
    bool IHaveToMove()
    {
        if (getMovement() != Vector2.zero) return true;
        else return false;
    }
    Vector2 getShootDir()
    {
        float horizontalMovement = 0;
        float verticalMovement = 0;

        if(Input.GetKey(KeyCode.LeftArrow)) {
            horizontalMovement = -1;
            verticalMovement = Random.Range(-shotSpray, shotSpray);
        } else if(Input.GetKey(KeyCode.RightArrow)) {
            horizontalMovement = 1;
            verticalMovement = Random.Range(-shotSpray, shotSpray);
        } else if(Input.GetKey(KeyCode.UpArrow)) {
            horizontalMovement = Random.Range(-shotSpray, shotSpray);
            verticalMovement = 1;
        }
        else if(Input.GetKey(KeyCode.DownArrow)) {
            horizontalMovement = Random.Range(-shotSpray, shotSpray);
            verticalMovement = -1;
        }
        return new Vector2(horizontalMovement, verticalMovement);
    }

    void Shoot(Vector2 velocity)
    {
        ProjectileCreator.createProjectile(
            ProjectilePrefab,
            transform.position + (Vector3)ShootingOffset, 
            velocity, 
            Range,
            Damage
        );
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.stop) {
            rb2D.velocity = Vector2.zero;
            animator.SetBool("Idle", true);
            animator.SetFloat("DirY", 0);
            animator.SetFloat("DirX", 0);
            return;
        }
        ShootTimeCounter += Time.deltaTime;

        Vector2 movement = getMovement().normalized;
        if (IHaveToMove()) haveToMove = true;

        Vector2 ShootDir = getShootDir();
        if (ShootDir.x != 0 || ShootDir.y != 0)
        {
            if (ShootTimeCounter >= 1/FireRate)
            {
                Shoot(ShootDir * shootSpeed + rb2D.velocity/4);
                ShootTimeCounter = 0;
            }

        }
    }
}
