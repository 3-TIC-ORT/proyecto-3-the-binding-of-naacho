using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextTest : MonoBehaviour
{
    TMP_Text Text;
    public GameObject Player;
    NaachoHeartSystem naachoHeartSystem;
    // Start is called before the first frame update
    void Start()
    {
        Text = gameObject.GetComponent<TMP_Text>();
        naachoHeartSystem = Player.GetComponent<NaachoHeartSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Text.text = $"Hp: {naachoHeartSystem.GetLifeAmount()}";
    }
}
