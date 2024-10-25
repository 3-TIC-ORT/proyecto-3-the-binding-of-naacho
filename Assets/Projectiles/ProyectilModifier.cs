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
        ChangeProyectilColor();
    }

    public virtual void ChangeProyectilColor()
    {
        float randomValue = Random.Range(0f, 1f);
        int RGB = Random.Range(0, 2+1);
        if (RGB == 0) spriteRenderer.color = new Color(randomValue, spriteRenderer.color.g, spriteRenderer.color.b);
        else if (RGB == 1) spriteRenderer.color = new Color(spriteRenderer.color.r, randomValue, spriteRenderer.color.b);
        else spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, randomValue);
    }
    void Update()
    {
        
    }

    public virtual void OnDestroy()
    {
    }
}
