using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public string proj_name;
    public string proj_tag;

    private GameObject proj;

    public GameObject createProjectile(string name, Vector2 position, Vector2 scale, bool enemy, Vector2 direction, int speed, Vector2 extraVelocity)
    {
        GameObject proj = new GameObject(name);
        Rigidbody2D proj_rb2D = proj.AddComponent<Rigidbody2D>();
        
        proj.transform.localScale = scale;

        proj.AddComponent<BoxCollider2D>();

        proj_rb2D.velocity = direction * speed + extraVelocity;
        proj.transform.position = position;

        return proj;
    }
}
