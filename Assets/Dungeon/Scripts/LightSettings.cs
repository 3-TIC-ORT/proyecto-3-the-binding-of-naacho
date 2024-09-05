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
        // Si el jugador está lo suficientemente lejos, desactiva el componente de light para ahorrar recursos.
        if ((transform.position-player.transform.position).magnitude>OcclusionCullingDistance) _light.enabled = false;
        else _light.enabled = true;
    }
}
