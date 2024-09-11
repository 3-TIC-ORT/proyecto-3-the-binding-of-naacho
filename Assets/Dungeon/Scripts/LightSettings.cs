using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSettings : MonoBehaviour
{
    private Light2D _light;
    [Tooltip
        ("Sirve para rotar la luz 90�. Fue hecha para las luces de los roomConectors, pero tal vez se podr�a usar para otra cosa." +
        "Si no se busca hacer nada de este estilo, dejar en false")
    ]
    public bool isVertical;
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
        if (isVertical) transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    // Update is called once per frame
    void Update()
    {
        // Si el jugador est� lo suficientemente lejos, desactiva el componente de light para ahorrar recursos.
        if ((transform.position-player.transform.position).magnitude>OcclusionCullingDistance) _light.enabled = false;
        else _light.enabled = true;
    }
}
