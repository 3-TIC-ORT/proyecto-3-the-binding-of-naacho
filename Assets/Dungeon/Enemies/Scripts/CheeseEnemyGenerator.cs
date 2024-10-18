using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseEnemyGenerator : MonoBehaviour
{
    public GameObject cheese;
    public void GenerateCheese(Collider2D col)
    {
        GameObject cheeseInstantiated=Instantiate(cheese, transform.position, Quaternion.identity, transform.parent.transform);
        Physics2D.IgnoreCollision(cheeseInstantiated.GetComponent<BoxCollider2D>(),col);
        cheeseInstantiated.GetComponent<EnemyEnabler>().SetComponents(true);
    }
}
