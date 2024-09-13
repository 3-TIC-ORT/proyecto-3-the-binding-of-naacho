using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnabler : MonoBehaviour
{
    Transform player;

    void Start()
    {
        player = GameObject.Find("Naacho").transform;
    }

    bool InBox(uint height, uint width, Vector2 position) {
        bool checkY = position.y < transform.position.y + height && position.y > transform.position.y - height;
        bool checkX = position.x < transform.position.x + width && position.x > transform.position.x - width;
        return  checkY && checkX;
    }

    void setStatus(bool state) {
        GetComponent<Enemy>().enabled = state;
        GetComponent<SpriteRenderer>().enabled = state;
    }

    void Update()
    {
        if(InBox(9, 9, player.transform.position)) {
            setStatus(true);
        } else {
            setStatus(false);
        }
        print($"Player: {player.position}, bounds: ({transform.position.y + 9}, {transform.position.y - 9})");
    }
}
