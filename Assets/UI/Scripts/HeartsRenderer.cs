using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartsRenderer : MonoBehaviour
{
    [SerializeField] private GameObject HeartPrefab;

    public void UpdateHearts(Heart[] Life)
    {
        foreach(Transform child in transform) {
            Destroy(child.gameObject);
        }

        int HeartCounter = 0;
        foreach(Heart heart in Life) {
            GameObject UIHeart = Instantiate(HeartPrefab);

            UIHeart.transform.SetParent(transform, false);
            UIHeart.transform.position = new Vector2(HeartCounter, 0) * UIHeart.transform.position.x;
        }
    }
}
