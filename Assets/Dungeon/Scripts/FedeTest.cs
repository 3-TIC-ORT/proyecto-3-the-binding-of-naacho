using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
public class FedeTest : MonoBehaviour
{
    private void Start() {
        Debug.Log("PUPUPUI");
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("COLISIONNNN");
    }
}
