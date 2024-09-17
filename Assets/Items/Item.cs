using UnityEngine;
using DG.Tweening;
abstract public class Item : MonoBehaviour
{
    public GameObject Naacho;
    private Transform transform;
    private SpriteRenderer spriteRenderer;
    [Header("Parámetros para la animación de flotar")]
    public float animationSpeed;
    public float heightVariation;
    [Header("Otros")]
    public float oclussionCulling;
    
    public void Start() {
        animationSpeed = 1.5f;
        heightVariation = 0.6f;
        transform = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Naacho = GameObject.Find("Naacho");
        transform.DOMoveY(transform.position.y + heightVariation, animationSpeed).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
    private void Update()
    {
        if ((Naacho.transform.position - transform.position).magnitude < oclussionCulling) spriteRenderer.enabled = true;
        else spriteRenderer.enabled = false;
    }
    public abstract void onPickup();

}
