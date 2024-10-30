using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DividingProjectile : ProyectilModifier
{
    [SerializeField] protected int FramesUntilDivision = 20;
    [SerializeField] protected uint DivisionAmount = 3;
    [SerializeField] protected Object DivisionProjectile; 
    [SerializeField] protected Rigidbody2D rb2D; 
    [SerializeField] private int counter = 0;

    public override void Start() {
        base.Start();

        DivisionProjectile = Resources.Load(
                "Prefabs/NaachoDefaultProjectile"
                );
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update() {
        counter++;
        if(FramesUntilDivision <= counter) {
            float direction = Mathf.Atan2(rb2D.velocity.x, rb2D.velocity.y);
            direction *= Mathf.Rad2Deg;
            direction += 170 / DivisionAmount;
            direction -= 90;
            for(int i = 0; i < DivisionAmount; i++) {
                GameObject proj = ProjectileCreator.createProjectile(
                        (GameObject) DivisionProjectile,
                        transform.position,
                        new Vector2(
                            Mathf.Cos(direction * Mathf.Deg2Rad), 
                            -Mathf.Sin(direction * Mathf.Deg2Rad)
                        ) * rb2D.velocity.magnitude,
                        5, 0.3f,
                        true
                        );
                direction -= 170 / DivisionAmount;
                print(proj.GetComponent<Rigidbody2D>().velocity);
            }
            Destroy(gameObject);
        }
    }
}
