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
    public float FireRate; // Amount of projectiles per second
    public float Range;
    public float Damage;
    private float ShootTimeCounter = 0;
    private bool isFiring;

    [SerializeField] Vector2 ShootingOffset;
    private ProjectileCreator ProjectileScript;
    private Rigidbody2D rb2D;
    [SerializeField] private Animator animatorBody;
    [SerializeField] private Animator animatorHead;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animatorBody = GetComponent<Animator>();
        foreach(Transform child in transform) {
            if(child.name == "Cabeza") {
                animatorHead = child.gameObject.GetComponent<Animator>();
                break;
            }
        }
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.stop)
        {
            rb2D.velocity = Vector2.zero;
            animatorBody.SetBool("Idle", true);
            animatorBody.SetFloat("DirY", 0);
            animatorBody.SetFloat("DirX", 0);
            return;
        }

        Vector2 movement = getMovement().normalized;

        if (movement != Vector2.zero)
        {
            rb2D.velocity = Speed * movement;
            animatorBody.SetFloat("DirY", movement.y);
            animatorBody.SetFloat("DirX", movement.x);
            animatorBody.SetBool("Idle", false);

            if(!isFiring) {
                animatorHead.enabled = Mathf.Abs(movement.x) + Mathf.Abs(movement.y) > 0;
                animatorHead.SetBool("Arriba", movement.y > 0);
                animatorHead.SetBool("Abajo", movement.y < 0);
                animatorHead.SetBool("Costado", movement.x != 0);
                animatorHead.GetComponent<SpriteRenderer>().flipX = movement.x < 0;
                print(Mathf.Abs(movement.x) + Mathf.Abs(movement.y) > 0);
                print(movement.y > 0);
                print(movement.y < 0);
                print(movement.x != 0);
            }
        }
        else 
        {
            rb2D.velocity *= Friction;

            animatorBody.SetBool("Idle", true);
            animatorBody.SetFloat("DirY", 0);
            animatorBody.SetFloat("DirX", 0);
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

        if(verticalMovement == -1) {
            animatorHead.GetComponent<SpriteRenderer>().flipX = false;
            animatorHead.SetBool("Abajo", true);
            animatorHead.SetBool("Arriba", false);
            animatorHead.SetBool("Costado", false);
        } else if(verticalMovement == 1) {
            animatorHead.GetComponent<SpriteRenderer>().flipX = false;
            animatorHead.SetBool("Abajo", false);
            animatorHead.SetBool("Arriba", true);
            animatorHead.SetBool("Costado", false);
        } else if(Mathf.Abs(horizontalMovement) == 1) {
            animatorHead.GetComponent<SpriteRenderer>().flipX = false;
            animatorHead.SetBool("Abajo", false);
            animatorHead.SetBool("Arriba", false);
            animatorHead.SetBool("Costado", true);
            print(horizontalMovement);
            if(horizontalMovement < 0)
                animatorHead.GetComponent<SpriteRenderer>().flipX = true;
            else
                animatorHead.GetComponent<SpriteRenderer>().flipX = false;
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
            animatorBody.SetBool("Idle", true);
            animatorBody.SetFloat("DirY", 0);
            animatorBody.SetFloat("DirX", 0);
            return;
        }
        ShootTimeCounter += Time.deltaTime;

        Vector2 movement = getMovement().normalized;

        Vector2 ShootDir = getShootDir();
        if (ShootDir.x != 0 || ShootDir.y != 0)
        {
            isFiring = true;
            animatorHead.enabled = true;
            if (ShootTimeCounter >= 1/FireRate)
            {
                Shoot(ShootDir * shootSpeed + rb2D.velocity/4);
                ShootTimeCounter = 0;
            }

        } else {
            isFiring = false;
            if(getMovement() == Vector2.zero)
                StartCoroutine(ShootIdle());
        }
    }

    IEnumerator ShootIdle() {
        animatorHead.GetComponent<SpriteRenderer>().flipX = false;
        animatorHead.SetBool("Abajo", true);
        animatorHead.SetBool("Arriba", false);
        animatorHead.SetBool("Costado", false);

        yield return null;
        animatorHead.enabled = false;
    }
}
