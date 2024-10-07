using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum HeartTypes
{
   Normal,
}

public class Heart {
    public HeartTypes heartType = HeartTypes.Normal;
    public float Amount;

    public bool NotIsFull() {
        return Amount < 1f;
    }

    public Heart(float amount = 1f, HeartTypes ht = HeartTypes.Normal) {
        heartType = ht;
        Amount = amount;
    }

    public bool isEmpty(){
        return Amount == 0;
    }
}

public class NaachoHeartSystem : MonoBehaviour
{
    public int startingLife = 3;
    public Heart[] Life;
    [SerializeField] private TextTest heartsRenderer;
    public const int MAX_LIFE = 12;
    public float LifeAmount;

    public int GetMaxLife() {
        int heartIdx = 0;
        for(;Life[heartIdx] != null && heartIdx < Life.Length; heartIdx++) {
            if(Life[heartIdx] == null) return heartIdx;
        }
        return heartIdx;
    }

    // Start is called before the first frame update
    void Start()
    {
        Life = new Heart[MAX_LIFE];
        for(int i = 0; startingLife > i; i++) {
            Life[i] = new Heart();
        }
        heartsRenderer = GameObject.Find("Life").GetComponent<TextTest>();
        LifeAmount = GetLifeAmount();

        heartsRenderer.UIUpdate(LifeAmount);
    }

    public float GetLifeAmount() {
        float amount = 0;
        foreach(Heart h in Life) {
            if(h == null) continue;
            amount += h.Amount;
        }
        return amount;
    }

    public float GetLifeAmontIdx(int idx) {
        return Life[idx].Amount;
    }

    // Update is called once per frame
    void UpdateLife()
    {
        LifeAmount = GetLifeAmount();
        heartsRenderer.UIUpdate(LifeAmount);
    }

    public int FindLastFullHeart() {
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
        if (heartIdx == -1 || Life[heartIdx].Amount <= 0) Death();

        Heart hrt = Life[heartIdx];
        if(hrt.Amount - dp >= 0) {
            hrt.Amount -= dp;
        }
        UpdateLife();
    }
    void Death()
    {
        Scene scene = SceneManager.GetActiveScene();
        Destroy(gameObject); // Despu�s no hay que destruirlo, si no dejarlo quieto con la animaci�n de muerto :v
        GameManager.Instance.stop = true;
        GameManager.Instance.Fade(true, false);
        SceneManager.LoadScene(scene.name);
    }

    public void Heal(float hp = .5f) {
        int heartIdx = FindLastFullHeart();

        heartIdx = (Life[heartIdx].NotIsFull()) ? heartIdx : heartIdx + 1;

        for(int i = 0; i < (Life.Length-1); i++) {
            if(Life[i] == null && i == heartIdx) return;
        }


        Heart hrt = Life[heartIdx];
        float prevHp = hrt.Amount;
        if(hrt.Amount + hp <= 1) {
            hrt.Amount += hp;
        } else {
            hrt.Amount = 1;
            if (hp - prevHp > 0)
                Heal(hp-prevHp);
        }

        UpdateLife();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Enemy")) {
            Damage(.5f);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Item")) {
            other.GetComponent<Item>().onPickup();
        }
    }

}
