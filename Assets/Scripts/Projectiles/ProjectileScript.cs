using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float TotalLifespan;
    public float Damage = .5f;
    private float lifespan = 0;

    // Update is called once per frame
    void Update()
    {
       lifespan += Time.deltaTime;
       if(lifespan >= TotalLifespan)
            Destroy(gameObject);
    }
}
