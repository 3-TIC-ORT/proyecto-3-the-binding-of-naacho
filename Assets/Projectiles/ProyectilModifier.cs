using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilModifier : MonoBehaviour
{
    protected ProjectileScript proyectilScript;
    public virtual void Start()
    {
        proyectilScript = GetComponent<ProjectileScript>();
    }

    void Update()
    {
        
    }

    public virtual void OnDestroy()
    {
    }
}
