using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
}

public class NaachoHeartSystem : MonoBehaviour
{
    public int startingLife = 3;
    Heart[] Life;
    public float LifeAmount;

    // Start is called before the first frame update
    void Start()
    {
        Life = new Heart[12];
        for(int i = 0; startingLife > i; i++) {
            Life[i] = new Heart();
        }
    }

    float GetLifeAmount() {
        float amount = 0;
        foreach(Heart h in Life) {
            if(h == null) continue;
            amount += h.Amount;
        }
        return amount;
    }

    // Update is called once per frame
    void Update()
    {
        LifeAmount = GetLifeAmount();
    }

    int FindLastFullHeart() {
        int heartIdx = Life.Length-1;
        while(heartIdx >= 0) {
            if(Life[heartIdx] == null) --heartIdx;
            else if(Life[heartIdx].Amount <= 0 ) --heartIdx;
            else return heartIdx;
        }

        return -1;
    }

    void Damage(float dp = .5f) {
        int heartIdx = FindLastFullHeart();
        if(heartIdx == -1 || Life[heartIdx].Amount <= 0) 
            SceneManager.LoadScene("NaachoPrueba");
        Heart hrt = Life[heartIdx];
        if(hrt.Amount - dp >= 0) {
            hrt.Amount -= dp;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Enemy")) {
            Damage(.5f);
        }
    }

}
