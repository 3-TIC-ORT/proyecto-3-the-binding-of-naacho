using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCreator : MonoBehaviour
{
    public Sprite proj_sprite;

    public void createProjectile(string name, Vector2 position, Vector2 scale, bool enemy, Vector2 direction, int speed, Vector2 extraVelocity, float lifespan, float dp)
    {
        GameObject proj = new GameObject(name);
        Rigidbody2D proj_rb2D = proj.AddComponent<Rigidbody2D>();
        ProjectileScript proj_script = proj.AddComponent<ProjectileScript>();

        proj_script.TotalLifespan = lifespan;
        proj_script.Damage = dp;
        
        proj.transform.localScale = scale;
        proj_rb2D.gravityScale = 0;
        proj.AddComponent<BoxCollider2D>().isTrigger = true;

        proj_rb2D.velocity = direction * speed + extraVelocity;
        proj.transform.position = position;

        SpriteRenderer proj_spr = proj.AddComponent<SpriteRenderer>();

        proj.tag = (enemy) ? "Enemy" : "Projectile";

        proj_spr.sprite = proj_sprite;

        proj.layer = 8;
    }
}
