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
                sr.enabled = false;
                _collider2D.enabled = false;
            }
            else
            {
                sr.enabled = true;
                _collider2D.enabled = true;
            }
        }
        else if (!GameManager.Instance.nachoNullPrinted)
        {
            Debug.LogWarning("Che macho, Naacho es null");
            GameManager.Instance.nachoNullPrinted = true;
        }
    }
}
