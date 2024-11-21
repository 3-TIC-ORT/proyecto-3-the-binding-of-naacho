using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTest : MonoBehaviour
{
    public NaachoHeartSystem naachoHeartSystem;
    public Sprite img;
    public float HeartSize = 1f;
    public float HeartOffset = 50;

    void Start() {
        UIUpdate();
    }

    void Update() {
        UIUpdate();
    }

    public void UIUpdate()
    {
        GameObject heartimg = null;

        foreach(Transform child in transform) {
            Destroy(child.gameObject);
        }

        if(naachoHeartSystem == null)
            naachoHeartSystem = GameObject.Find("Naacho").GetComponent<NaachoHeartSystem>();

        if(naachoHeartSystem == null || naachoHeartSystem.Life == null) {
            Debug.LogWarning("No se encontró vida, esto es normal la si se recarga la escena");
            return;
        }
        foreach(Heart heart in naachoHeartSystem.Life) {
            if(heart == null || heart.Amount <= 0) continue;

            GameObject hrt = new GameObject("Heart");

            if(heartimg == null)
                hrt.transform.position = transform.position;
            else {
                hrt.transform.position = heartimg.transform.position;
                hrt.transform.position += Vector3.right * HeartOffset;
            }

            hrt.transform.localScale = Vector3.one * HeartSize;
            Image image = hrt.AddComponent<Image>();
            image.sprite = img;

            image.type = Image.Type.Filled;
            image.fillMethod = Image.FillMethod.Horizontal;
            image.fillAmount = heart.Amount;

            RectTransform transf = image.GetComponent<RectTransform>();
            transf.anchorMax = Vector2.up;
            transf.anchorMin = Vector2.up;

            hrt.transform.SetParent(transform);
            heartimg = hrt;
        }
    }
}
