using UnityEngine;
using DG.Tweening;
abstract public class Item : MonoBehaviour
{
    public GameObject Naacho;
    private Transform transform;
    public float animationSpeed;
    public float heightVariation;
    public void Start() {
        animationSpeed = 1.5f;
        heightVariation = 0.6f;
        transform = GetComponent<Transform>();
        Naacho = GameObject.Find("Naacho");
        transform.DOMoveY(transform.position.y + heightVariation, animationSpeed).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
    private void Update()
    {
    }
    public abstract void onPickup();

}
