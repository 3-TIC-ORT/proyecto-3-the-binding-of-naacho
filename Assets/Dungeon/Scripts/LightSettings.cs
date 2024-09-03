using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSettings : MonoBehaviour
{
    private Light2D _light;
    public float OcclusionCullingDistance;
    public Color treasureRoomColor;
    public Color bossRoomColor;
    public float specialRoomFalloff;
    //public float falloffStrength;
    //public float intensity;
    private GameObject player;
    void Start()
    {
        _light = GetComponent<Light2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position-player.transform.position).magnitude>25) _light.enabled = false;
        else _light.enabled = true;
    }
}
