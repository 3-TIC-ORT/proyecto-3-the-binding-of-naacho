using UnityEngine;

public class HeartPickup : Pickup
{
    public override void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")) {
            NaachoHeartSystem naachoLife = other.gameObject.GetComponent<NaachoHeartSystem>();
            if(naachoLife.FindLastFullHeart() < naachoLife.GetMaxLife()){
                naachoLife.Heal(PickupAmount);
                Destroy(gameObject);
            }
        }
    }
}
