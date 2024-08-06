using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaachoController : MonoBehaviour
{
    public int Speed;
    public float Friction;
    public float shootDelay;
    public float ProjectileLifespan;
    private float ShootTimeCounter = 0;
    private ProjectileCreator ProjectileScript;
    private Rigidbody2D rb2D;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        ProjectileScript = gameObject.GetComponent<ProjectileCreator>();
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
        int horizontalMovement = 0;
        int verticalMovement = 0;

        if(Input.GetKey(KeyCode.LeftArrow)) {
            horizontalMovement = -1;
            verticalMovement = 0;
        }
        else if(Input.GetKey(KeyCode.RightArrow)) {
            horizontalMovement = 1;
            verticalMovement = 0;
        } else if(Input.GetKey(KeyCode.UpArrow)) {
            horizontalMovement = 0;
            verticalMovement = 1;
        }
        else if(Input.GetKey(KeyCode.DownArrow)) {
            horizontalMovement = 0;
            verticalMovement = -1;
        }
        return new Vector2(horizontalMovement, verticalMovement);
    }

    void Shoot(Vector2 direction, Vector2 velocity, int speed, Vector2 size)
    {
        ProjectileScript.createProjectile("Naacho Projectile", transform.position, size, false, direction.normalized, speed, velocity, ProjectileLifespan);
    }

    // Update is called once per frame
    void Update()
    {
        ShootTimeCounter += Time.deltaTime;

        Vector2 movement = getMovement().normalized;
        if(movement.x != 0 || movement.y != 0)
            rb2D.velocity = movement * Speed * Time.deltaTime;
        else
            rb2D.velocity *= Friction * Time.deltaTime;
        
        Vector2 shoorDir = getShootDir();
        if (shoorDir.x != 0 || shoorDir.y != 0)
        {
            if (ShootTimeCounter >= shootDelay)
            {
                Shoot(shoorDir, rb2D.velocity, 10, new Vector2(0.5f, 0.5f));
                ShootTimeCounter = 0;
            }

        }
    }
}
