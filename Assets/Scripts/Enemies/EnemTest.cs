using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemTest : Enemy
{
    // Update is called once per frame
    public override void Update()
    {
        if(GameManager.Instance.stop) return;
        Vector2 playerDir = (Vector2)(Player.transform.position - transform.position).normalized;
        rb2D.velocity = Speed * Time.deltaTime * playerDir;
    }
}
