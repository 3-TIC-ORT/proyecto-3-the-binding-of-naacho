using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemTest : Enemy
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        gameObject.layer = 8;
        defaultColor = SpRenderer.color;
    }

    // Update is called once per frame
    public override void Update()
    {
        Vector2 playerDir = (Vector2)(Player.transform.position - transform.position).normalized;
        rb2D.velocity = Speed * Time.deltaTime * playerDir;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

}
