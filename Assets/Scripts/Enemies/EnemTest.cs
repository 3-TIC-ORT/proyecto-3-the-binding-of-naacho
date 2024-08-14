using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemTest : Enemy
{
    // Start is called before the first frame update
    public override void Start()
    {
        InitEnemy(Vector2.left, Vector2.one);
        base.Start();
        SpRenderer.color = new Color(.2f, .8f, .14f);
        SpRenderer.sprite = EnemySprite;
        EnemyObj.layer = 8;
        rb2D.freezeRotation = true;
        Col2D.size = Vector2.one * .9f;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        Vector2 playerDir = (Vector2)(Player.transform.position - EnemyObj.transform.position).normalized;
        rb2D.velocity = Speed * Time.deltaTime * playerDir;
    }
}