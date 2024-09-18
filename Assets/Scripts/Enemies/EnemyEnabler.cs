using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnabler : MonoBehaviour
{
    Transform player;
    public float activationDistance;

    void Start()
    {
        player = GameObject.Find("Naacho").transform;
    }

    public bool InBox(Vector2 position) {
        return ((Vector2)transform.position - position).magnitude < activationDistance;
    }

    void setStatus(bool state) {
        GetComponent<Enemy>().enabled = state;
        GetComponent<SpriteRenderer>().enabled = state;
    }

    void Update()
    {
        if(InBox(player.transform.position)) {
            setStatus(true);
        } else {
            setStatus(false);
        }
        //print($"Player: {player.position}, bounds: ({transform.position.y + 9}, {transform.position.y - 9})");
    }
}
