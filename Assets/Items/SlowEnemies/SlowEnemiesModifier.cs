using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEnemiesModifier : ProyectilModifier
{
    public override void Start()
    {
        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Enemy enemyCom = col.gameObject.GetComponent<Enemy>();
            if (!enemyCom.effects.isSlowed)
            {
                StartCoroutine(ChangeEnemySpeed(enemyCom));
                enemyCom.effects.isSlowed = true;
            }

        }
    }
    IEnumerator ChangeEnemySpeed(Enemy enemyCom)
    {
        enemyCom.Speed /= 2;
        yield return new WaitForSecondsRealtime(2f);
        enemyCom.Speed *= 2;
        enemyCom.effects.isSlowed = false;
    }
}
