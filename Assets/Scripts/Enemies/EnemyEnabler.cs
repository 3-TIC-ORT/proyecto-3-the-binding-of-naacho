using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnabler : MonoBehaviour
{
    private GameObject player;
    public float oclussionCullingDistance;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D collider2D;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        if ((transform.position - player.transform.position).magnitude > oclussionCullingDistance)
        {
            sr.enabled = false;
            collider2D.enabled = false;
        }
        else
        {
            sr.enabled = true;
            collider2D.enabled = true;
        }
    }
}
