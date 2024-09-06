using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : Item
{
    public override void onPickup()
    {
        Naacho.GetComponent<NaachoController>().shootDelay /= 1.5f;
        Destroy(gameObject);
    }
}
