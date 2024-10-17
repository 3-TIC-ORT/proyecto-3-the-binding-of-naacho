using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnabler : MonoBehaviour
{
    private GameObject player;
    public float oclussionCullingDistance;
    private Enemy enemy;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D _collider2D;
    public List<string> componentsToDisable;
    public bool enabled;
    private void Start()
    {
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        SetComponents(false);
    }
    private void Update()
    {
        
    }
    
    public void SetComponents(bool enabling)
    {
        enabled = enabling;
        enemy.enabled = enabling;
        sr.enabled = enabling;
        foreach (string component in componentsToDisable)
        {
            // Agarrar el tipo de componente
            Type type = Type.GetType(component);

            if (type != null)
            {
                // Obtener el componente de este gameObject
                Component comp = GetComponent(type);

                if (comp != null)
                {
                    // Agarrar la propiedad enabled del tipo de componente
                    var enabledProperty = type.GetProperty("enabled");
                    if (enabledProperty != null)
                    {
                        // Desactivar el componente
                        enabledProperty.SetValue(comp, enabling);
                    }
                    
                }
            }
        }
    }
}
