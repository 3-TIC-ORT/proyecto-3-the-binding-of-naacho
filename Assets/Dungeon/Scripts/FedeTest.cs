using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
public class FedeTest : MonoBehaviour
{
    public Vector3 paraDondeMeMuevo; 
    private void Start() {
        Debug.Log("PUPUPUI");
        transform.Translate(paraDondeMeMuevo);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("COLISIONNNNEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE   ");
    }
}
