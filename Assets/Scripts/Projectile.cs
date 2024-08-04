using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public string proj_name;
    public string proj_tag;

   private GameObject proj;


    public GameObject createProjectile(string name, Vector2 position, bool enemy, Vector2 direction, Vector2 velocity)
    {
        GameObject proj = new GameObject(name);
        Rigidbody2D proj_rb2D = proj.AddComponent<Rigidbody2D>();
        return proj;
    }
}
