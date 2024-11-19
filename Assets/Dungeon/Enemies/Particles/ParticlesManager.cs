using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    public static ParticlesManager Instance { get; private set; }
    public GameObject onDeathParticlesPrefab;
    public GameObject CheeseBallParticle;
    public GameObject onPickUpItemParticle;
    public GameObject generalContainer;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }
    void Start()
    {
        generalContainer = GameObject.FindGameObjectWithTag("GeneralContainer");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstanceParticle(GameObject particle ,Vector2 position, Transform parent)
    {
        Instantiate(particle, position, Quaternion.identity, parent);
    }
}
