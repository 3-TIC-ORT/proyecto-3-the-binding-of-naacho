using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaachoTortuga : Enemy
{
    public override void Update()
    {
        if (GameManager.Instance.stop) return;
        Vector2 playerDir = (Vector2)(Player.transform.position - transform.position).normalized;
        rb2D.velocity = Speed * Time.deltaTime * playerDir;
        CheckMainDirection(playerDir);
    }

    private void CheckMainDirection(Vector3 direction)
    {
        if (Mathf.Abs(direction.y)>Mathf.Abs(direction.x))
        {
            if (direction.y>0)
            {
                Debug.Log("Me muevo hacia arriba");
            }
            else
            {
                Debug.Log("Me muevo hacia abajo");
            }
        }
        else
        {
            if (direction.x>0)
            {
                Debug.Log("Me muevo hacia la derecha");
            }
            else
            {
                Debug.Log("Me muevo hacia la izquierda");
            }
        }
    }
}
