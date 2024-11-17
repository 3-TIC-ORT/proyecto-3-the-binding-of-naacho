using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTest : MonoBehaviour
{
    public NaachoHeartSystem naachoHeartSystem;
    public Sprite img;
    public float HeartSize = 0.3f;
    public float HeartOffset = 50;

    // Update is called once per frame
    public void UIUpdate(float life)
    {
        GameObject heartimg = null;

        foreach(Transform child in transform) {
            Destroy(child.gameObject);
        }

        if(naachoHeartSystem == null)
            naachoHeartSystem = GameObject.Find("Naacho").GetComponent<NaachoHeartSystem>();

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

            hrt.transform.SetParent(transform);
            heartimg = hrt;
        }
    }
}
