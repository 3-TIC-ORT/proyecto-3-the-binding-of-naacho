using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEnemiesModifier : ProyectilModifier
{
    public float timer=0;
    public float maxTime = 1.4f;
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
            else enemyCom.effects.timeSlowed -= 2f;

        }
        else if (col.gameObject.CompareTag("Room"))
        {
            Destroy(gameObject);
        }
    }
    IEnumerator ChangeEnemySpeed(Enemy enemyCom)
    {
        SpriteRenderer enemySR = enemyCom.gameObject.GetComponent<SpriteRenderer>();
        enemyCom.Speed /= 2;
        while (enemyCom.effects.timeSlowed < maxTime)
        {
            if (!enemyCom.hasKnockback)
            {
                enemySR.color = Color.yellow;
            }
            enemyCom.effects.timeSlowed += Time.deltaTime;
            yield return null;
        }
        if (enemyCom!=null)
        {
            enemyCom.Speed *= 2;
            enemySR.color = enemyCom.defaultColor;
            enemyCom.effects.isSlowed = false;
            enemyCom.effects.timeSlowed = 0;
        }
        yield return null;
        Destroy(gameObject);
    }
}
