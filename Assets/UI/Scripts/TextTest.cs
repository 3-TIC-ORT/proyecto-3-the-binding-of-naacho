using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTest : MonoBehaviour
{
    [SerializeField] private NaachoHeartSystem naachoHeartSystem;
    private Image img;

    void Awake() {
        img = GetComponent<Image>();
    }

    void Start() {
        if(naachoHeartSystem == null) {
            naachoHeartSystem = GameObject.Find("Naacho").GetComponent<NaachoHeartSystem>();
        }
    }
    // Update is called once per frame
    public void UIUpdate(float life)
    {
        img.color = new Color(0.1f * life, 1, 1, 0.6f);
    }
}
