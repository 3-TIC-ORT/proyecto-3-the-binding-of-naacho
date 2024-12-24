using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternInitializer : MonoBehaviour
{
    public static ExternInitializer Instance { get; private set; }
    [Header("PREFABS #################################")]
    public GameObject blackHoleExtPrefab;
    public GameObject rayoExtPrefab;
    public GameObject iluminacionDivinaExtPrefab;
    [Header("STATS ###################################")]
    [Header("Iluminacion Divina")]
    public float IDmaxTimeAlive;
    public float IDdeathSpeed;
    public float IDradius;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
