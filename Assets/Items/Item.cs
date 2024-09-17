using UnityEngine;
using DG.Tweening;
using TMPro;
abstract public class Item : MonoBehaviour
{
    public GameObject Naacho;
    private Transform transform;
    private SpriteRenderer spriteRenderer;
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
        transform = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        text = GameObject.Find("ItemExplanation").GetComponent<TextMeshProUGUI>();
        text.text = itemExplanation;
        Naacho = GameObject.Find("Naacho");
        transform.DOMoveY(transform.position.y + heightVariation, animationSpeed).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
    private void Update()
    {
        if ((Naacho.transform.position - transform.position).magnitude < oclussionCulling) spriteRenderer.enabled = true;
        else spriteRenderer.enabled = false;

       
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
