using UnityEngine;

public class HeartPickup : Pickup
{
    public NaachoHeartSystem naachoHeartSystem;
    bool IndexInBounds(int idx) {
        return idx < naachoHeartSystem.GetMaxLife();
    }

    bool canBePickedUp(int lastHrtIdx) {
        bool idxInBounds = IndexInBounds(lastHrtIdx);
        bool hrtNotFull = naachoHeartSystem.Life[lastHrtIdx].NotIsFull();
        return (idxInBounds && hrtNotFull) || (IndexInBounds(lastHrtIdx+1) && naachoHeartSystem.Life[lastHrtIdx+1].NotIsFull());
    }

    public override void OnCollisionEnter2D(Collision2D other) {
        if (!other.gameObject.CompareTag("Player")) return;

        naachoHeartSystem = other.gameObject.GetComponent<NaachoHeartSystem>();
        int lastHrtIdx = naachoHeartSystem.FindLastFullHeart();


        if (canBePickedUp(lastHrtIdx))
        {
            naachoHeartSystem.Heal(PickupAmount);
            Destroy(gameObject);
        }
    }
}
