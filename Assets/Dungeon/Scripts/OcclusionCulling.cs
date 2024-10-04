using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionCulling : MonoBehaviour
{
    private Transform playerPos;
    private BoxCollider2D _collider;
    public float occlusionCulling;
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        playerPos =GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - playerPos.transform.position).magnitude > occlusionCulling) _collider.enabled = false;
        else _collider.enabled = true;
    }
}
