using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class Item : MonoBehaviour
{
    public string modifier;
    public bool changeProyectilColor;
    protected GameObject Naacho;
    private SpriteRenderer spriteRenderer;
    protected NaachoController naachoController;
    protected NaachoHeartSystem naachoHeartSystem;

    [Header("Par�metros para la animaci�n de flotar")]
    public float animationSpeed;
    public float heightVariation;

    [Header("Otros")]
    public float oclussionCulling;
    public float textOclussionCulling;
    public string itemExplanation;
    private TextMeshProUGUI text;
    
    public void Start() {
        animationSpeed = 1.5f;
        heightVariation = 0.6f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        text = GameObject.Find("ItemExplanation").GetComponent<TextMeshProUGUI>();
        text.text = itemExplanation;
        Naacho = GameObject.Find("Naacho");
        naachoController = Naacho.GetComponent<NaachoController>();
        naachoHeartSystem = Naacho.GetComponent<NaachoHeartSystem>();
        transform.DOMoveY(transform.position.y + heightVariation, animationSpeed).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
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
            text.enabled = false;
        }
    }
    public virtual void onPickup()
    {
        if (modifier!=null)
        {
            ProjectileCreator.modifiers.Add(modifier);
            if (changeProyectilColor) ChangeProyectilColor();
            Destroy(gameObject);
        }
    }
    public virtual void ChangeProyectilColor()
    {
        float randomValueR = UnityEngine.Random.Range(0f, 1f);
        float randomValueG = UnityEngine.Random.Range(0f, 1f);
        float randomValueB = UnityEngine.Random.Range(0f, 1f);

        ProjectileCreator.proyectilColor = new Color(randomValueR, randomValueG, randomValueB);
        
    }

}
