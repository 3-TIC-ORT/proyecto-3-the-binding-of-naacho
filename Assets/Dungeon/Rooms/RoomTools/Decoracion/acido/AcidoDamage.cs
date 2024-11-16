using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidoDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<NaachoHeartSystem>().Damage();
            StartCoroutine(col.gameObject.GetComponent<NaachoHeartSystem>().Iframes(GetComponent<Collider2D>()));
        }
    }
}
