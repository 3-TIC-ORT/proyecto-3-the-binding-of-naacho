using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTest : MonoBehaviour
{
    [SerializeField] private NaachoHeartSystem naachoHeartSystem;
    [SerializeField] private Sprite img;

    // Update is called once per frame
    public void UIUpdate(float life)
    {
        GameObject heartimg = null;

        foreach(Transform child in transform) {
            Destroy(child.gameObject);
        }

        foreach(Heart heart in naachoHeartSystem.Life) {
            if(heart == null || heart.Amount <= 0) continue;

            GameObject hrt = new GameObject("Heart");

            if(heartimg == null)
                hrt.transform.position = transform.position;
            else {
                hrt.transform.position = heartimg.transform.position;
                hrt.transform.position += Vector3.right * 50;
            }

            hrt.transform.localScale = Vector3.one * 0.5f;
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
