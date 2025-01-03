using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IluminacionDivina : ProyectilModifier
{
    private bool isSpecial;
    private GameObject iluminacionDivinaExtPrefab;
    public override void Start()
    {
        base.Start();
        
        iluminacionDivinaExtPrefab = ExternInitializer.Instance.iluminacionDivinaExtPrefab;
        isSpecial = Random.value > 0.9f;
        if (isSpecial)
        {
            spriteRenderer.material = ExternInitializer.Instance.iluDivMaterial;
            transform.localScale *= 2;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (isSpecial && col.gameObject.CompareTag("Enemy"))
        {
            IluminacionDivinaExt instance = (Instantiate(iluminacionDivinaExtPrefab, col.gameObject.transform.position, Quaternion.identity, GameObject.FindGameObjectWithTag("GeneralContainer").transform)).GetComponent<IluminacionDivinaExt>();
            instance.initialPos = col.gameObject.transform.position;
            instance.proyectilDamage = proyectilScript.Damage;
        }
    }
}
