using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilModifier : MonoBehaviour
{
    protected ProjectileScript proyectilScript;
    protected NaachoController controller;
    public virtual void Start()
    {
        proyectilScript = GetComponent<ProjectileScript>();
        controller = PlayerManager.Instance.GetComponent<NaachoController>();
    }

    void Update()
    {
        
    }

    public virtual void OnDestroy()
    {
    }
}
