using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaachoController : MonoBehaviour
{
    public int Speed;
    public float Friction;

    public GameObject ProjectilePrefab;
    public float shotSpray;
    public float shootSpeed;
    public float shootDelay;
    public float ProjectileLifespan;
    public float Damage;
    private float ShootTimeCounter = 0;

    private ProjectileCreator ProjectileScript;
    private Rigidbody2D rb2D;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        ProjectileScript = GetComponent<ProjectileCreator>();
        animator = GetComponent<Animator>();
    }

    Vector2 getMovement() 
    {
        int horizontalMovement = 0;
        int verticalMovement = 0;

        if(Input.GetKey(KeyCode.D))
            horizontalMovement = 1;
        else if(Input.GetKey(KeyCode.A))
            horizontalMovement = -1;
        
        if(Input.GetKey(KeyCode.W))
            verticalMovement = 1;
        else if(Input.GetKey(KeyCode.S))
            verticalMovement = -1;

        return new Vector2(horizontalMovement, verticalMovement);
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
        ProjectileScript.createProjectile(
            ProjectilePrefab,
            transform.position, 
            velocity, 
            ProjectileLifespan,
            Damage
        );
    }

    // Update is called once per frame
    void Update()
    {
        ShootTimeCounter += Time.deltaTime;

        Vector2 movement = getMovement().normalized;
        if (movement.x != 0 || movement.y != 0)
        {
            rb2D.velocity = Speed * Time.deltaTime * movement;
            animator.SetFloat("DirY", movement.y);
            animator.SetFloat("DirX", movement.x);
            animator.SetBool("Idle", false);
        }
        else
        {
            rb2D.velocity *= Friction;
            animator.SetFloat("DirY", 0);
            animator.SetFloat("DirX", 0);
            animator.SetBool("Idle", true);
        }

        Vector2 ShootDir = getShootDir();
        if (ShootDir.x != 0 || ShootDir.y != 0)
        {
            if (ShootTimeCounter >= shootDelay)
            {
                Shoot(ShootDir * shootSpeed + rb2D.velocity/4);
                ShootTimeCounter = 0;
            }

        }
    }
}
