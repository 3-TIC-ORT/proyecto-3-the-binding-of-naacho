using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float TotalLifespan;
    private float lifespan = 0;

    // Update is called once per frame
    void Update()
    {
       lifespan += Time.deltaTime;
       if(lifespan >= TotalLifespan)
            Destroy(gameObject);
    }
}
