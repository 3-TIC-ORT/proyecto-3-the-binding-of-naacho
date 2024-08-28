using UnityEngine;

// Abstract is like a template
public abstract class Pickup : MonoBehaviour
{
    public uint PickupAmount;
    public virtual void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")) {
            Destroy(gameObject);
        }
    }
}
