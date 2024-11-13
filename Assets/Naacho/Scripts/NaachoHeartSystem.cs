using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using DG.Tweening;


public class Heart
{
    public enum HeartTypes
    {
        Normal,
    }

    public HeartTypes heartType = HeartTypes.Normal;
    public float Amount;
    public bool NotIsFull()
    {
        return Amount < 1f;
    }

    public Heart(float amount = 1f, HeartTypes ht = HeartTypes.Normal)
    {
        heartType = ht;
        Amount = amount;
    }

    public bool isEmpty()
    {
        return Amount == 0;
    }
}

public class NaachoHeartSystem : MonoBehaviour
{
    [SerializeField] private TextTest heartsRenderer;
    [SerializeField] private SpriteRenderer SpRenderer;
    [SerializeField] private BoxCollider2D NaachoHitbox;
    [SerializeField] private Color defaultColor;
    public int iframeTime;
    public int startingLife = 3;
    public Heart[] Life;
    public const int MAX_LIFE = 12;
    public float LifeAmount;
    public bool dead;

    public int GetMaxLife()
    {
        int heartIdx = 0;
        for (; Life[heartIdx] != null && heartIdx < Life.Length; heartIdx++)
        {
            if (Life[heartIdx] == null) return heartIdx;
        }
        return heartIdx;
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Mazmorras testing")
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        NaachoHitbox = GetComponent<BoxCollider2D>();
        SpRenderer = GetComponent<SpriteRenderer>();
        heartsRenderer = GameObject.Find("Life").GetComponent<TextTest>();
        defaultColor = SpRenderer.color;
        Life = new Heart[MAX_LIFE];
        for (int i = 0; startingLife > i - 1; i++)
        {
            Life[i] = new Heart();
        }
        LifeAmount = GetLifeAmount();

        heartsRenderer.UIUpdate(LifeAmount);
    }

    public float GetLifeAmount()
    {
        float amount = 0; foreach (Heart h in Life)
        {
            if (h == null) continue;
            amount += h.Amount;
        }
        return amount;
    }

    public float GetLifeAmontIdx(int idx)
    {
        return Life[idx].Amount;
    }

    // Update is called once per frame
    void UpdateLife()
    {
        LifeAmount = GetLifeAmount();
        if(heartsRenderer == null)
            heartsRenderer = GameObject.Find("Life").GetComponent<TextTest>();
        heartsRenderer.UIUpdate(LifeAmount);
    }

    public int FindLastFullHeart()
    {
        int heartIdx = Life.Length - 1;
        while (heartIdx >= 0)
        {
            if (Life[heartIdx] == null) --heartIdx;
            else if (Life[heartIdx].Amount <= 0) --heartIdx;
            else return heartIdx;
        }

        return -1;
    }

    public void Damage(float dp = .5f)
    {
        MinimapManager.Instance.minimapCanvas.SetActive(false);
        if (!PlayerManager.Instance.cameraIsShaking && !PlayerManager.Instance.correctingCamera) 
            Feedback();
        StartCoroutine(VisualDamage());
        int heartIdx = FindLastFullHeart();
        if (heartIdx < 0 || Life[heartIdx].Amount <= 0 && !dead) DeathSet();

        if(GameManager.Instance.stop) return;
        Heart hrt = Life[heartIdx];
        if (hrt.Amount - dp >= 0)
        {
            hrt.Amount -= dp;
        } else {
            hrt.Amount -= (-hrt.Amount);
            dp -= -hrt.Amount;
            Damage(dp);
        }
        UpdateLife();
    }
    private void DeathSet()
    {
        dead=true;
        GameManager.Instance.stop = true;
        GameManager.Instance.Death();   
        GetComponent<DoorDisabler>().enabled = false;
        heartsRenderer = GameObject.Find("Life").GetComponent<TextTest>();
        // Despu�s hay que dejarlo quieto con la animaci�n de muerto :v
    }
    void Feedback()
    {
        PlayerManager.Instance.cameraIsShaking = true;
        GameObject cameraAim = GameObject.FindGameObjectWithTag("CameraAim");
        CameraAim cameraAimScript = cameraAim.GetComponent<CameraAim>();
        Transform cameraAimTransform = cameraAim.GetComponent<Transform>();
        Vector3 initialPosition = cameraAimTransform.position;
        cameraAimTransform.DOMove(cameraAimTransform.position + new Vector3(1f, 1f, 0), cameraAimScript.cameraShakeSpeed).onComplete =
        () =>
        {
            cameraAimTransform.DOMove(cameraAimTransform.position + new Vector3(-1f, -2f, 0), cameraAimScript.cameraShakeSpeed * 2).onComplete =
           () =>
           {
               cameraAimTransform.DOMove(cameraAimTransform.position + new Vector3(-1f, 2f, 0), cameraAimScript.cameraShakeSpeed * 2).onComplete =
              () =>
              {
                  cameraAimTransform.DOMove(cameraAimTransform.position + new Vector3(1f, -1f, 0), cameraAimScript.cameraShakeSpeed).onComplete =
                  () =>
                  {
                      cameraAimTransform.DOMove(initialPosition, cameraAimScript.cameraShakeSpeed / 2).onComplete =
                      () =>
                      {
                          PlayerManager.Instance.cameraIsShaking = false;
                      };
                  };
              };
           };
        };
    }

    public void Heal(float hp = .5f)
    {
        int heartIdx = FindLastFullHeart();

        heartIdx = (Life[heartIdx].NotIsFull()) ? heartIdx : heartIdx + 1;

        for (int i = 0; i < (Life.Length - 1); i++)
        {
            if (Life[i] == null && i == heartIdx) return;
        }


        Heart hrt = Life[heartIdx];
        float prevHp = hrt.Amount;
        if (hrt.Amount + hp <= 1)
        {
            hrt.Amount += hp;
        }
        else
        {
            hrt.Amount = 1;
            if (hp - prevHp > 0)
                Heal(hp - prevHp);
        }

        UpdateLife();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Damage(.5f);
            StartCoroutine(Iframes(collision.collider));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            other.GetComponent<Item>().onPickup();
        }
        else if (other.CompareTag("Projectile") && other.GetComponent<ProjectileScript>().isEnemy)
        {
            Damage(other.GetComponent<ProjectileScript>().Damage);
            StartCoroutine(Iframes(other));
        }
    }

    IEnumerator VisualDamage()
    {
        SpRenderer.color = Color.red;

        while (SpRenderer.color.g + SpRenderer.color.b < 2f)
        {
            SpRenderer.color = new Color(SpRenderer.color.r, SpRenderer.color.g + 0.05f, SpRenderer.color.b + 0.05f);
            yield return null;
        }
        SpRenderer.color = defaultColor;
    }

    IEnumerator Iframes(Collider2D enemy)
    {
        gameObject.layer = 9;
        for (int i = 0; i < iframeTime; i++)
        {
            if (i % 4 == 0) SpRenderer.enabled = false;
            else SpRenderer.enabled = true;
            yield return null;
        }
        SpRenderer.enabled = true;
        gameObject.layer = 0;
    }
}
