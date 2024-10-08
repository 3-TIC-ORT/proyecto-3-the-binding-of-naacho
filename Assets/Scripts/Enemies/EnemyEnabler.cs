using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnabler : MonoBehaviour
{
    private GameObject player;
    public float oclussionCullingDistance;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D _collider2D;
    public string[] componentsToDisable;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        if (player != null)
        {
            if ((transform.position - player.transform.position).magnitude > oclussionCullingDistance)
            {
                SetComponents(false);
            }
            else
            {
                SetComponents(true);
            }
        }
        else if (!GameManager.Instance.nachoNullPrinted)
        {
            Debug.LogWarning("Che macho, Naacho es null");
            GameManager.Instance.nachoNullPrinted = true;
        }
    }
    private void SetComponents(bool enabling)
    {
        sr.enabled = enabling;
        _collider2D.enabled = enabling;
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
