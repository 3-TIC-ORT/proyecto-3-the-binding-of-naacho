using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilModifier : MonoBehaviour
{
    protected ProjectileScript proyectilScript;
    protected NaachoController controller;
    protected SpriteRenderer spriteRenderer;
    public virtual void Start()
    {
        proyectilScript = GetComponent<ProjectileScript>();
        controller = PlayerManager.Instance.GetComponent<NaachoController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        
    }

    public virtual void OnDestroy()
    {
    }
}
