using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using DG.Tweening;

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
    [SerializeField] private TextTest heartsRenderer;
    [SerializeField] private SpriteRenderer SpRenderer;
    [SerializeField] private BoxCollider2D NaachoHitbox;
    [SerializeField] private Color defaultColor;
    public int startingLife = 3;
    public Heart[] Life;
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
        NaachoHitbox = GetComponent<BoxCollider2D>();
        SpRenderer = GetComponent<SpriteRenderer>();
        heartsRenderer = GameObject.Find("Life").GetComponent<TextTest>();
        defaultColor = SpRenderer.color;
        Life = new Heart[MAX_LIFE];
        for(int i = 0; startingLife > i-1; i++) {
            Life[i] = new Heart();
        }
        heartsRenderer = GameObject.Find("Life").GetComponent<TextTest>();
        LifeAmount = GetLifeAmount();

        heartsRenderer.UIUpdate(LifeAmount);
    }

    public float GetLifeAmount() {
        float amount = 0; foreach(Heart h in Life) {
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
        if (!GameManager.Instance.cameraIsShaking) Feedback();
        StartCoroutine(VisualDamage());
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
        Destroy(gameObject); // Después no hay que destruirlo, si no dejarlo quieto con la animación de muerto :v
        GameManager.Instance.stop = true;
        GameManager.Instance.Fade(true, false);
        SceneManager.LoadScene(scene.name);
        heartsRenderer = GameObject.Find("Life").GetComponent<TextTest>();
    }
    void Feedback()
    {
        GameManager.Instance.cameraIsShaking = true;
        GameObject cameraAim = GameObject.FindGameObjectWithTag("CameraAim");
        CameraAim cameraAimScript = cameraAim.GetComponent<CameraAim>();
        Transform cameraAimTransform = cameraAim.GetComponent<Transform>();
        cameraAimTransform.DOMove(cameraAimTransform.position + new Vector3(1f, 1f, 0), cameraAimScript.cameraShakeSpeed).onComplete =
        () => 
        {
            cameraAimTransform.DOMove(cameraAimTransform.position + new Vector3(-1f, -2f, 0), cameraAimScript.cameraShakeSpeed * 2).onComplete =
           () => 
           {
               cameraAimTransform.DOMove(cameraAimTransform.position + new Vector3(-1f, 2f, 0), cameraAimScript.cameraShakeSpeed * 2).onComplete =
              () => 
              {
                  cameraAimTransform.DOMove(cameraAimTransform.position + new Vector3(1f, -1f, 0), cameraAimScript.cameraShakeSpeed).onComplete=
                  ()=>
                  {
                      GameManager.Instance.cameraIsShaking = false;
                  };
              };
            };
        };
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
            StartCoroutine(Iframes(collision.collider));
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Item")) {
            other.GetComponent<Item>().onPickup();
        }
    }

    IEnumerator VisualDamage()
    {
        SpRenderer.color = Color.red;

        while (SpRenderer.color.g + SpRenderer.color.b < 2f)
        {
            SpRenderer.color = new Color(SpRenderer.color.r, SpRenderer.color.g+0.05f, SpRenderer.color.b+0.05f);
            yield return null;
        }
        SpRenderer.color = defaultColor;
    }

    IEnumerator Iframes(Collider2D enemy) {
        gameObject.layer = 9;
        for(int i = 0; i < 60; i++) {
            yield return null;
        }
        gameObject.layer = 0;
    }
}
