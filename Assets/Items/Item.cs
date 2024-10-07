using UnityEngine;
using DG.Tweening;
using TMPro;
abstract public class Item : MonoBehaviour
{
    protected GameObject Naacho;
    private SpriteRenderer spriteRenderer;
    protected NaachoController naachoController;
    protected NaachoHeartSystem naachoHeartSystem;

    [Header("Parámetros para la animación de flotar")]
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
    public abstract void onPickup();

}
