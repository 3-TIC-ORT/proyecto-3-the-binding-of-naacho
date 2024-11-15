using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerOrdering : MonoBehaviour
{
    private SpriteRenderer SpRenderer;
    [SerializeField] private float Radius = 3.5f;
    private int baseLayer;

    // Start is called before the first frame update
    void Start()
    {
        SpRenderer = GetComponent<SpriteRenderer>();
        baseLayer = SpRenderer.sortingOrder;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] Collisions = Physics2D.OverlapCircleAll(transform.position, Radius, 0b1001000); 
        /* Layermask para layer de enemigos y decoraciones */

        //print($"{Collisions}, {Collisions.Length}");

        //print($"{Convert.ToString(0b1001 << 2, 2)}");

        if(Collisions.Length == 0) {
            SpRenderer.sortingOrder = baseLayer;
            return;
        }

        float oldDist = 99;
        SpriteRenderer closest = null;
        foreach(Collider2D other in Collisions) {
            SpriteRenderer otherSpR;
            if(!other.TryGetComponent<SpriteRenderer>(out otherSpR)) continue;
            if(other.gameObject.layer == 3 && !other.CompareTag("ItemHolder")) continue;
            float newDist = Vector3.Distance(other.transform.position, transform.position);
            if(newDist < oldDist) {
                oldDist = newDist;
                closest = otherSpR;
            }
        }

        if(closest != null) {
            //print($"{closest.transform.position.y}, {transform.position.y}");
            if(closest.transform.position.y < transform.position.y) {
                closest.sortingOrder = baseLayer + 1;
                foreach(Transform child in closest.transform) {
                    child.GetComponent<SpriteRenderer>().sortingOrder = closest.sortingOrder + 1;
                }
                SpRenderer.sortingOrder = baseLayer;
            } else {
                closest.sortingOrder = baseLayer;
                foreach(Transform child in closest.transform) {
                    child.GetComponent<SpriteRenderer>().sortingOrder = closest.sortingOrder - 1;
                }
                SpRenderer.sortingOrder = baseLayer + 1;
            }
            print($"{closest.sortingOrder}, {SpRenderer.sortingOrder}");
        }
    }

    // TODO: Check if lechuguitas animadas
}
