using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int EnemyAmount;
    public GameObject EnemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        for(int enemy = 0; enemy < EnemyAmount; ++enemy) {
            Instantiate(EnemyPrefab);
        }
    }
}
