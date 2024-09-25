using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextTest : MonoBehaviour
{
    [SerializeField] private NaachoHeartSystem naachoHeartSystem;
    private Image img;

    void Awake() {
        img = GetComponent<Image>();
    }
    // Update is called once per frame
    public void UIUpdate(float life)
    {
        img.color = new Color(0.1f * life, 1, 1);
    }
}
