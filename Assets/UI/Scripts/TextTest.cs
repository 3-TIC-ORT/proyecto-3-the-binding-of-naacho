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
        GameObject heartimg= null;
        foreach(Heart heart in naachoHeartSystem.Life) {
            if(heart == null) continue;
            GameObject hrt = new GameObject("Heart");
            if(heartimg == null)
                hrt.transform.position = transform.position;
            else hrt.transform.position = heartimg.transform.position;
            hrt.transform.localScale = Vector3.one * 0.5f;
            hrt.AddComponent<Image>().sprite = img.sprite;
            hrt.transform.position += Vector3.right * 3;
            hrt.transform.SetParent(transform);
            heartimg = hrt;
        }
    }
}
