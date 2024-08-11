using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

internal class EnemTest : Enemy
{
    public EnemTest(Sprite sprite, float hp = 3f, float dp = 0.5f, uint speed = 350, string name = "Enemy") : base(sprite, hp, dp, speed, name) 
    {
    }
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        SpRenderer.color = new Color(.2f, .8f, .14f);
        EnemyObj.layer = 0;
        rb2D.freezeRotation = true;
        Col2D.isTrigger = false;
        Col2D.size = Vector2.one * .9f;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        Vector2 playerDir = (Vector2)(EnemyObj.transform.position - Player.transform.position).normalized;
        rb2D.velocity = -Speed * Time.deltaTime * playerDir;
    }
}

public class EnemyTest : MonoBehaviour 
{
    public Sprite enemySprite;
    public int EnemyAmount = 0;
    List<EnemTest> enemy;
    void Start() {
        enemy = new List<EnemTest>();
        for(int i = 0; i < EnemyAmount; i++) {
            enemy.Add(new EnemTest(enemySprite, speed: 150));
            enemy[i].InitEnemy(Enemy.ColliderType.Box, new Vector2(i, -i), new Vector2(0.75f, 0.75f));
            enemy[i].Start();
        }
    }

    void Update() {
        for(int i = 0; i < EnemyAmount; i++) {
            enemy[i].Update();
        }
    }
}