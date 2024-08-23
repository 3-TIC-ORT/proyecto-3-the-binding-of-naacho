using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemTest : Enemy
{
    private Color defaultColor;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        gameObject.layer = 8;
        Col2D.size = Vector2.one * .9f;
        defaultColor = SpRenderer.color;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        Vector2 playerDir = (Vector2)(Player.transform.position - transform.position).normalized;
        rb2D.velocity = Speed * Time.deltaTime * playerDir;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    public override void Damage(float dp)
    {
        base.Damage(dp);
        StartCoroutine(VisualDamage());
    }

    IEnumerator VisualDamage() {
        SpRenderer.color = Color.red;

        while(SpRenderer.color.r > defaultColor.r) {
            SpRenderer.color = new Color(SpRenderer.color.r - .025f, defaultColor.g, defaultColor.b);
            yield return null;
        }
    }
}