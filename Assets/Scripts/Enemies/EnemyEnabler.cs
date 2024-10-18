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
    public bool enemyEnabled;
    private void Start()
    {
       GetComponentsReferences();
    }
    public void GetComponentsReferences()
    {
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void SetComponents(bool enabling)
    {
        enemyEnabled = enabling;
        enemy.enabled = enabling;   
        sr.enabled = enabling;
        foreach (string component in componentsToDisable)
        {
            Type typeA = Type.GetType("UnityEngine." + component + ", UnityEngine");
            Type typeB = Type.GetType(component);
            if (typeA != null) EnableComponent(typeA, enabling);
            else if (typeB!=null) EnableComponent(typeB, enabling);
            else Debug.LogWarning("No se encontró el tipo: " + component);
        }
    }
    private void EnableComponent(Type type, bool enabling)
    {
        // Agarrar el componente de este gameObject
        Component comp = GetComponent(type);

        if (comp != null)
        {
            // Agarrar la propiedad enabled del tipo de componente
            var enabledProperty = type.GetProperty("enabled");
            if (enabledProperty != null)
            {
                // Desactivar o activar el componente
                enabledProperty.SetValue(comp, enabling);
            }
        }
    }
}
