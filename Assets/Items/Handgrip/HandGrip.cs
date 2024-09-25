using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrip : Item
{
    public override void onPickup()
    {
        naachoController.Damage += 0.7f;
        naachoController.FireRate -= 0.3f;
        Destroy(gameObject);
    }
}
