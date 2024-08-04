using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaachoController : MonoBehaviour
{
    public int Speed;
    public float Friction;
    private Rigidbody2D rb2D;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float verticalMovement = Input.GetAxis("Vertical");
        float horizontalMovement = Input.GetAxis("Horizontal");
    
        if(verticalMovement != 0 || horizontalMovement != 0)
            rb2D.velocity = new Vector2(horizontalMovement, verticalMovement).normalized * Speed * Time.deltaTime;
        else
            rb2D.velocity *= Friction;
    }
}
