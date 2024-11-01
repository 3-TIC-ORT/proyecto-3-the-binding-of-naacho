using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    public static ParticlesManager Instance { get; private set; }
    public GameObject onDeathParticlesPrefab;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstanceDeathParticle(Vector2 spawnPosition)
    {
        Instantiate(onDeathParticlesPrefab, spawnPosition, Quaternion.identity);
    }
}
