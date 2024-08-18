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

    public void Damage(float dp = .5f) {
        Amount -= dp;
    }

    public void Heal(float hp = .5f) {
        Amount = (Amount + hp <= 1f) ? Amount + hp : Amount;
    }
}

public class NaachoHeartSystem : MonoBehaviour
{
    public int startingLife = 3;
    List<Heart> Life;
    public float LifeAmount;
    public float LifePlaceholder;

    // Start is called before the first frame update
    void Start()
    {
        Life = new List<Heart>();
        for(int i = 0; startingLife > i; i++) {
            Life.Add(new Heart());
        }
    }

    float GetLifeAmount() {
        float amount = 0;
        foreach(Heart h in Life) {
            amount += h.Amount;
        }
        return amount;
    }

    // Update is called once per frame
    void Update()
    {
        LifeAmount = GetLifeAmount();
    }

    int Damage(float dp, int counter = 1) {
        /*if(Life[Life.Capacity-counter].Amount - dp >= 0) {
            Life[Life.Capacity-counter].Amount -= dp;
            return 0;
        }
        else if(Life[Life.Capacity-counter].Amount - dp/2 >= 0) {
            Life[Life.Capacity-counter].Amount -= dp/2;
            return Damage(dp/2, counter++);
        } else return 0;*/
        LifePlaceholder -= dp;
        if(LifePlaceholder < 0) {
            SceneManager.LoadScene("NaachoPrueba");
        }
        return 0;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Enemy")) {
            Damage(.5f);
        }
    }

}
