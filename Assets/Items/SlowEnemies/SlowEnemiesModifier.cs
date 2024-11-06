using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEnemiesModifier : ProyectilModifier
{
    public float timer=0;
    public float maxTime = 2;
    public override void Start()
    {
        base.Start();
        proyectilScript.dontDestroyWhenCollided = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            proyectilScript.dontDestroyWhenDistance = true;
            spriteRenderer.enabled = false;
            m_collider.enabled = false;
            proyectilScript.enabled = false;
            Enemy enemyCom = col.gameObject.GetComponent<Enemy>();
            if (!enemyCom.effects.isSlowed)
            {
                StartCoroutine(ChangeEnemySpeed(enemyCom));
                enemyCom.effects.isSlowed = true;
            }
            else timer -= 2f;

        }
    }
    IEnumerator ChangeEnemySpeed(Enemy enemyCom)
    {
        SpriteRenderer enemySR = enemyCom.gameObject.GetComponent<SpriteRenderer>();
        timer = 0;
        maxTime = 2;
        enemyCom.Speed /= 2;
        while (timer < maxTime)
        {
            if (!enemyCom.hasKnockback)
            {
                enemySR.color = Color.yellow;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        if (enemyCom!=null)
        {
            enemyCom.Speed *= 2;
            enemySR.color = enemyCom.defaultColor;
            enemyCom.effects.isSlowed = false;
        }
        yield return null;
        Destroy(gameObject);
    }
}
