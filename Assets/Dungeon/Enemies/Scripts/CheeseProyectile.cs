using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseProyectile : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    public Vector3 angle;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(speed * angle * Time.deltaTime, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        string colTag = col.gameObject.tag;
        if (colTag == "Player" || colTag == "Room") OnDestroy();
    }
    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
