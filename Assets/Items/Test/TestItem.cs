using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : Item
{
    public override void onPickup()
    {
        Naacho.GetComponent<NaachoController>().FireRate *=1.25f;
        Destroy(gameObject);
    }
}
