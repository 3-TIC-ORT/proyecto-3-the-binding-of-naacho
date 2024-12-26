using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using Unity.VisualScripting;

public class Item : MonoBehaviour
{
    public ProyectilModifier.Modifiers proyectilModifier;
    public NaachoModifier.Modifiers naachoModifier;
    public GameObject[] allIncompatibleItems;
    public bool changeProyectilColor;
    protected GameObject Naacho;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    protected NaachoController naachoController;
    protected NaachoHeartSystem naachoHeartSystem;

    [Header("Parámetros para cambiar las stats de Naacho")]
    public int speedVariation;
    public float shotSprayVariation;
    public float shootSpeedVariation;
    public float fireRateVariation;
    public float rangeVariation;
    public float damageVariation;

    [Header("Parámetros para la animación de flotar")]
    public float animationSpeed;
    public float heightVariation;

    [Header("Otros")]
    public float oclussionCulling;
    public float textOclussionCulling;
    public string itemExplanation;
    private TextMeshProUGUI text;
    private Transform childTransfrom;
    
    public void Start() {
        animationSpeed = 1.5f;
        heightVariation = 0.6f;
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = 3.5f;
        childTransfrom = transform.GetChild(0).GetComponent<Transform>();  
        text = GameObject.Find("ItemExplanation").GetComponent<TextMeshProUGUI>();
        text.text = itemExplanation;
        Naacho = GameObject.Find("Naacho");
        naachoController = Naacho.GetComponent<NaachoController>();
        naachoHeartSystem = Naacho.GetComponent<NaachoHeartSystem>();
        childTransfrom.DOMoveY(childTransfrom.position.y + heightVariation, animationSpeed).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
    private void Update()
    {
        if (Naacho!=null) 
        {
            if ((Naacho.transform.position - transform.position).magnitude < oclussionCulling) spriteRenderer.enabled = true;
            else spriteRenderer.enabled = false;
        }
        else if (!GameManager.Instance.nachoNullPrinted)
        {
            Debug.LogWarning("Che macho, Naacho es null");
            GameManager.Instance.nachoNullPrinted = true;
        }


    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            text.text = itemExplanation;
            text.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (text!=null) text.enabled = false;
        }
    }
    public virtual void onPickup()
    {
        if (proyectilModifier != ProyectilModifier.Modifiers.None)
        {
            ProjectileCreator.modifiers.Add(Enum.GetName(
                        typeof(ProyectilModifier.Modifiers),
                        proyectilModifier
                        ));
            if (changeProyectilColor) ChangeProyectilColor();
        }
        if (naachoModifier != NaachoModifier.Modifiers.None)
        {
            string name = Enum.GetName(typeof(NaachoModifier.Modifiers), naachoModifier);
            Type type = Type.GetType(name);
            PlayerManager.Instance.gameObject.AddComponent(type);
        }
        ChangeNaachoStats();
        
        Destroy(gameObject);
    }
    public virtual void ChangeNaachoStats()
    {
        naachoController.Speed += speedVariation;
        naachoController.shotSpray += shotSprayVariation;
        naachoController.shootSpeed += shootSpeedVariation;
        naachoController.FireRate += fireRateVariation;
        naachoController.Range += rangeVariation;
        naachoController.Damage += damageVariation;
    }
    public virtual void ChangeProyectilColor()
    {
        float randomValueR = UnityEngine.Random.Range(0f, 1f);
        float randomValueG = UnityEngine.Random.Range(0f, 1f);
        float randomValueB = UnityEngine.Random.Range(0f, 1f);

        ProjectileCreator.proyectilColor = new Color(randomValueR, randomValueG, randomValueB);
        
    }

}
