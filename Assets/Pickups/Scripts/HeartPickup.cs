using UnityEngine;

public class HeartPickup : Pickup
{
    bool indexInBounds(int idx, NaachoHeartSystem naachoHeartSystem) {
        return idx < naachoHeartSystem.GetMaxLife();
    }

    public override void OnCollisionEnter2D(Collision2D other) {
        if (!other.gameObject.CompareTag("Player")) return;

        NaachoHeartSystem naachoLife = other.gameObject.GetComponent<NaachoHeartSystem>();
        int lastHrtIdx = naachoLife.FindLastFullHeart();

        bool idxInBounds = indexInBounds(lastHrtIdx, naachoLife);
        bool hrtNotFull = naachoLife.Life[lastHrtIdx].NotIsFull();

        if ((idxInBounds && hrtNotFull) || (indexInBounds(lastHrtIdx+1, naachoLife) && naachoLife.Life[lastHrtIdx+1].NotIsFull()))
        {
            naachoLife.Heal(PickupAmount);
            Destroy(gameObject);
        }
    }
}
