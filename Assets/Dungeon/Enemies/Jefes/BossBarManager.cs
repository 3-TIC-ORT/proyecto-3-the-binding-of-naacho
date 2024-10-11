using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossBarManager : MonoBehaviour
{
    private List<int> IDs = new List<int>();
    private RoomTemplates templates;
    private float totalMaxHealth;
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    }

    // Update is called once per frame
    void Update()
    {
        List<GameObject> enemiesGroupPrefab = GetChildren(gameObject, false, "");
        foreach (GameObject enemieGroup in enemiesGroupPrefab)
        {
            List<GameObject> enemies = GetChildren(enemieGroup, false, "");
            if (enemies.Count>0) 
            {
                Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, templates.insideRoomArea, 0);
                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject.CompareTag("Player")) ControlBossBar(enemies);
                }
            }
            else Destroy(GameObject.Find("BossBar"));

        }
    }

    void ControlBossBar(List<GameObject> enemies)
    {
        Image rellenoBossBar = GameObject.Find("Relleno").GetComponent<Image>();
        rellenoBossBar.color = new Color(1, 1, 1, 1);
        GameObject.Find("Borde").GetComponent<Image>().color = new Color(1, 1, 1, 1);
        float generalHealth = 0;
        foreach (GameObject enemy in enemies)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (!IDs.Contains(enemy.GetInstanceID()))
            {
                if (enemyComponent.HealthPoints > 0 && enemyComponent.readableMaxHealth > 0)
                { 
                    totalMaxHealth += enemyComponent.readableMaxHealth; 
                    IDs.Add(enemy.GetInstanceID());
                }
            }
            if (enemyComponent.HealthPoints > 0 && enemyComponent.readableMaxHealth > 0)
            {
                generalHealth += enemyComponent.HealthPoints;
            }
        }
        rellenoBossBar.fillAmount = generalHealth / totalMaxHealth;
    }

    List<GameObject> GetChildren(GameObject parent, bool filter, string tag)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in parent.transform)
        {
            if (!filter)
            {
                children.Add(child.gameObject);
            }
            else if (filter && child.CompareTag(tag))
            {
                children.Add(child.gameObject);
            }
        }
        return children;
    }
}
