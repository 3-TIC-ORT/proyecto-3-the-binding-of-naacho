using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseEnemyGenerator : MonoBehaviour
{
    public GameObject cheese;
    public float cheeseGenerationTime;
    public void GenerateCheese()
    {
        Instantiate(cheese, transform.position, Quaternion.identity, transform.parent.transform);
    }
}
