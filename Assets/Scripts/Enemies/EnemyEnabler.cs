using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnabler : MonoBehaviour
{
    Transform player;

    void Start()
    {
        player = GetComponent<Enemy>().Player.transform;
    }

    bool InBox(uint height, uint width, Vector2 position) {
        bool checkY = position.y < height && position.y > -height;
        bool checkX = position.x < width && position.x > -width;
        return  checkY && checkX;
    }

    void setStatus(bool state) {
        GetComponent<Enemy>().enabled = state;
        GetComponent<SpriteRenderer>().enabled = state;
    }

    void Update()
    {
        if(InBox(3, 3, transform.position)) {
            setStatus(true);
        } else {
            setStatus(false);
        }
    }
}
