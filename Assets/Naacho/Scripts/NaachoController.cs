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
    [SerializeField] private Vector2 HeadOffset;
    private Vector3 headPos;

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
        headPos = animatorHead.transform.localPosition;
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.stop)
        {
            rb2D.velocity = Vector2.zero;
            animatorBody.SetBool("Idle", true);
            animatorBody.SetBool("Vertical", false);
            animatorBody.SetBool("Strafe", false);
            return;
        }

        Vector2 movement = getMovement().normalized;

        if (movement != Vector2.zero)
        {
            rb2D.velocity = Speed * movement;
            animatorBody.SetBool("Idle", false);
            animatorBody.SetBool("Vertical", Mathf.Abs(movement.y) > 0);
            animatorBody.SetBool("Strafe", false);
            if(Mathf.Abs(movement.y) <= 0){
                animatorBody.SetBool("Vertical", false);
                animatorBody.SetBool("Strafe", Mathf.Abs(movement.x) > 0);
            }
            animatorBody.GetComponent<SpriteRenderer>().flipX = movement.x < 0;

        } else {
            rb2D.velocity *= Friction;

            animatorBody.SetBool("Idle", true);
            animatorBody.SetBool("Vertical", false);
            animatorBody.SetBool("Strafe", false);
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
            animatorHead.transform.localPosition = headPos;
        } else if(verticalMovement == 1) {
            animatorHead.GetComponent<SpriteRenderer>().flipX = false;
            animatorHead.SetBool("Abajo", false);
            animatorHead.SetBool("Arriba", true);
            animatorHead.SetBool("Costado", false);
            animatorHead.transform.localPosition = headPos;
        } else if(Mathf.Abs(horizontalMovement) == 1) {
            animatorHead.GetComponent<SpriteRenderer>().flipX = false;
            animatorHead.SetBool("Abajo", false);
            animatorHead.SetBool("Arriba", false);
            animatorHead.SetBool("Costado", true);
            if(horizontalMovement < 0) {
                animatorHead.GetComponent<SpriteRenderer>().flipX = true;
                animatorHead.transform.localPosition = headPos;
            } else {
                animatorHead.transform.localPosition = headPos + (Vector3)HeadOffset;
            }
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
            animatorBody.SetBool("Vertical", false);
            animatorBody.SetBool("Strafe", false);
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
        }

        if(!isFiring) {
            if(movement == Vector2.zero) {
                StartCoroutine(ShootIdle());
                return;
            }
            animatorHead.enabled = Mathf.Abs(movement.x) + Mathf.Abs(movement.y) > 0;
            if(movement.y > 0) {
                animatorHead.SetBool("Arriba", true);
                animatorHead.SetBool("Abajo", false);
                animatorHead.SetBool("Costado", false);
            } else if(movement.y < 0) {
                animatorHead.SetBool("Arriba", false);
                animatorHead.SetBool("Abajo", true);
                animatorHead.SetBool("Costado", false);
            } else if(movement.x != 0) {
                animatorHead.SetBool("Arriba", false);
                animatorHead.SetBool("Abajo", false);
                animatorHead.SetBool("Costado", true);
            } else {
                animatorHead.SetBool("Arriba", false);
                animatorHead.SetBool("Abajo", true);
                animatorHead.SetBool("Costado", false);
            }
            if(movement.x > 0 && animatorHead.GetBool("Costado")) {
                print(movement.x);
                animatorHead.transform.localPosition = headPos + (Vector3)HeadOffset;
            } else {
                animatorHead.GetComponent<SpriteRenderer>().flipX = movement.x < 0 && animatorHead.GetBool("Costado");
                animatorHead.transform.localPosition = headPos;
            }
        }
    }

    IEnumerator ShootIdle() {
        animatorHead.GetComponent<SpriteRenderer>().flipX = false;
        animatorHead.SetBool("Abajo", true);
        animatorHead.SetBool("Arriba", false);
        animatorHead.SetBool("Costado", false);
        animatorHead.transform.localPosition = headPos;

        yield return null;
        animatorHead.enabled = false;
    }
}
