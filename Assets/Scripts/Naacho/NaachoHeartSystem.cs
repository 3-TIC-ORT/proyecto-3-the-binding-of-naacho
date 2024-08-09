using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

enum HeartTypes
{
   Normal,
}

class Heart {
    public HeartTypes heartType = HeartTypes.Normal;
    public float Amount;

    public Heart(float amount = 1f, HeartTypes ht = HeartTypes.Normal) {
        heartType = ht;
        Amount = amount;
    }

    protected void Damage(float dp = .5f) {
        Amount -= dp;
    }

    protected void Heal(float hp = .5f) {
        Amount = (Amount + hp <= 1f) ? Amount + hp : Amount;
    }
}

public class NaachoHeartSystem : MonoBehaviour
{
    public int startingLife = 3;
    List<Heart> Life;
    public float LifeAmount;

    // Start is called before the first frame update
    void Start()
    {
        Life = new List<Heart>();
        for(int i = 0; startingLife > i; i++) {
            Life.Add(new Heart());
        }
    }

    // Update is called once per frame
    void Update()
    {
        LifeAmount = 0;
        for(int i = 0; Life.Capacity - 1 > i; i++) {
            LifeAmount += Life[i].Amount;
        }
    }
}
