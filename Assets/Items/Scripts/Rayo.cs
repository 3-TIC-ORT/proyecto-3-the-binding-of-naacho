using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rayo : ProyectilModifier
{
    public GameObject rayoPrefab;
    [SerializeField] private float radius=5;
    public override void Start()
    {
        base.Start();
        rayoPrefab = ExternInitializer.Instance.rayoExtPrefab;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (rayoPrefab != null)
            {
                Transform nearestEnemy = GetNearestEnemy(col);
                if (nearestEnemy == null) return;
                Vector3 spawnPosition = col.gameObject.transform.position;
                GameObject rayoPrefabInstance = Instantiate(rayoPrefab, spawnPosition, Quaternion.identity, GameObject.FindGameObjectWithTag("GeneralContainer").transform);
                RayosExt rayExtScript = rayoPrefabInstance.GetComponent<RayosExt>();
                rayExtScript.damage = proyectilScript.Damage * 0.3f;
                rayExtScript.enemy = nearestEnemy.GetComponent<Enemy>();
                rayExtScript.points = new Transform[] { col.gameObject.transform, nearestEnemy };
                rayExtScript.lastPositions = new Vector3[] { col.gameObject.transform.position, nearestEnemy.position};
            }
        }
    }

    Transform GetNearestEnemy(Collider2D col)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(col.gameObject.transform.position, radius);
        foreach (Collider2D collider in colliders) if (collider.gameObject.CompareTag("Enemy") && collider!=col) return collider.GetComponent<Transform>();
        return null;
    }
}
