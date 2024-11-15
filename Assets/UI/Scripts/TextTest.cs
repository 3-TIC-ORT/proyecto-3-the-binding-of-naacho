using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextTest : MonoBehaviour
{
    [SerializeField] private NaachoHeartSystem naachoHeartSystem;
    [SerializeField] private Image img;

    void Awake() {
        img = GetComponent<Image>();
    }
    // Update is called once per frame
    public void UIUpdate(float life)
    {
        GameObject heartimg = null;
        foreach(Transform child in transform) {
            Destroy(child.gameObject);
        }
        foreach(Heart heart in naachoHeartSystem.Life) {
            if(heart == null) continue;

            GameObject hrt = new GameObject("Heart");

            if(heartimg == null)
                hrt.transform.position = transform.position;
            else hrt.transform.position = heartimg.transform.position;

            hrt.transform.localScale = Vector3.one * 0.5f;
            Image image = hrt.AddComponent<Image>();
            image.sprite = img.sprite;
            image.color = new Color(img.color.r, img.color.g, img.color.b, heart.Amount);
            hrt.transform.position += Vector3.right * 50;
            hrt.transform.SetParent(transform);
            heartimg = hrt;
        }
    }
}
