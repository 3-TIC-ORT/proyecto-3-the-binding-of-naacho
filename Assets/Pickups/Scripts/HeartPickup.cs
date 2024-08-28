using UnityEngine;

public class HeartPickup : Pickup
{
    public override void OnCollisionEnter2D(Collision2D other) {
        if (!other.gameObject.CompareTag("Player")) return;

        NaachoHeartSystem naachoLife = other.gameObject.GetComponent<NaachoHeartSystem>();
        int lastHrtIdx = naachoLife.FindLastFullHeart();
        if (lastHrtIdx < naachoLife.GetMaxLife() && naachoLife.GetLifeAmontIdx(lastHrtIdx) < 1f)
        {
            naachoLife.Heal(PickupAmount);
            Destroy(gameObject);
        }
    }
}
