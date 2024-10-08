using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseEnemyGenerator : MonoBehaviour
{
    public GameObject cheese;
    public float cheeseGenerationTime;
    void Start()
    {
        StartCoroutine(GenerateCheese());
    }

    IEnumerator GenerateCheese()
    {
        yield return new WaitForSecondsRealtime(3f);
        Instantiate(cheese, transform.position, Quaternion.identity, transform.parent.transform);
        StartCoroutine(GenerateCheese());
    }
}
