using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCreator : MonoBehaviour
{
    public GameObject createProjectile(GameObject prefab, Vector2 position, Vector2 velocity, float range, float dp)
    {
        GameObject proj = Instantiate(prefab, position, Quaternion.identity);
        Rigidbody2D proj_rb2D = proj.GetComponent<Rigidbody2D>();
        ProjectileScript proj_script = proj.GetComponent<ProjectileScript>();
        SpriteRenderer proj_spr = proj.GetComponent<SpriteRenderer>();

        proj_rb2D.gravityScale = 0;

        proj_rb2D.velocity = velocity;
        proj.transform.position = position;

        proj_script.Range = range;
        proj_script.Damage = dp;

        proj.GetComponent<Collider2D>().isTrigger = true;

        return proj;
    }
}
