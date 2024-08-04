using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Sprite proj_sprite;

    public void createProjectile(string name, Vector2 position, Vector2 scale, bool enemy, Vector2 direction, int speed, Vector2 extraVelocity)
    {
        GameObject proj = new GameObject(name);
        Rigidbody2D proj_rb2D = proj.AddComponent<Rigidbody2D>();
        
        proj.transform.localScale = scale;

        proj.AddComponent<BoxCollider2D>().isTrigger = true;

        proj_rb2D.velocity = direction * speed + extraVelocity;
        proj.transform.position = position;

        SpriteRenderer proj_spr = proj.AddComponent<SpriteRenderer>();

        proj_spr.sprite = proj_sprite;
    }
    Vector2 getShootDir()
    {
        int horizontalMovement = 0;
        int verticalMovement = 0;

        if(Input.GetKey(KeyCode.LeftArrow))
            horizontalMovement = 1;
        else if(Input.GetKey(KeyCode.RightArrow))
            horizontalMovement = -1;
        
        if(Input.GetKey(KeyCode.UpArrow))
            verticalMovement = 1;
        else if(Input.GetKey(KeyCode.DownArrow))
            verticalMovement = -1;

        return new Vector2(horizontalMovement, verticalMovement);
    }

    void Shoot(Vector2 direction, Vector2 velocity) 
    {
        createProjectile("Naacho Projectile", transform.position, new Vector2(1, 1), false, direction, 10, velocity);
    }
    private void Update() {
        Vector2 shoorDir = getShootDir();
        if (shoorDir.x != 0 || shoorDir.y != 0)
            Shoot(shoorDir, Vector2.zero);
    }
}
