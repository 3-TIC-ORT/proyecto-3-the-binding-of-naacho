using UnityEngine;

abstract public class Item : MonoBehaviour
{
    public GameObject Naacho;

    public void Start() {
        Naacho = GameObject.Find("Naacho");
    }

    public abstract void onPickup();
}
