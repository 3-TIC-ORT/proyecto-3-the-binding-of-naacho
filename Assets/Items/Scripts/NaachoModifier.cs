using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaachoModifier : MonoBehaviour
{
    public enum Modifiers
    {
        None,
        PushAway
    };
    protected NaachoController controller;
    protected NaachoHeartSystem heartSystem;
    public virtual void Start()
    {
        controller = GetComponent<NaachoController>();
        heartSystem = GetComponent<NaachoHeartSystem>();
    }
}
