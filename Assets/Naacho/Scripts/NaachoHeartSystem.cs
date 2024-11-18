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
    public int startingLife;
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
            ProjectileCreator.modifiers.Clear();
            ProjectileCreator.proyectilColor = new Color(1, 1, 1, 1);
            Destroy(gameObject);
            return;
        }
        if(heartsRenderer == null) getRenderer();
    }

    void Start()
    {
        NaachoHitbox = GetComponent<BoxCollider2D>();
        SpRenderer = GetComponent<SpriteRenderer>();
        getRenderer();
        defaultColor = SpRenderer.color;
        Life = new Heart[MAX_LIFE];
        for (int i = 0; startingLife > i; i++)
        {
            Life[i] = new Heart();
        }
        LifeAmount = GetLifeAmount();

        heartsRenderer.UIUpdate();
    }

    void getRenderer() {
        Transform localCanvas = GameObject.Find("LocalCanvas").transform;
        foreach(Transform child in localCanvas) {
            if(child.name == "Life") {
                heartsRenderer = child.gameObject.GetComponent<TextTest>();
                return;
            }
        }
        heartsRenderer.naachoHeartSystem = this;
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
        if(heartsRenderer == null || heartsRenderer.naachoHeartSystem == null)
            getRenderer();
        heartsRenderer.UIUpdate();
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
        if(dead) return;
        StartCoroutine(VisualDamage());
        int heartIdx = FindLastFullHeart();
        if ((heartIdx < 0) && !dead) {
            DeathSet();
            return;
        }

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

        if ((heartIdx < 0 || (heartIdx == 0 && Life[heartIdx].Amount <= 0)) && !dead) DeathSet();

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
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Damage(.5f);
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

    public IEnumerator Iframes(Collider2D other = null)
    {
        gameObject.layer = 9;
        if(other != null) {
            Physics2D.IgnoreCollision(NaachoHitbox, other, true);
        }
        for (int i = 0; i < iframeTime; i++)
        {
            if (i % 4 == 0) SpRenderer.enabled = false;
            else SpRenderer.enabled = true;
            yield return null;
        }
        SpRenderer.enabled = true;
        gameObject.layer = 0;
        if(other != null) {
            Physics2D.IgnoreCollision(NaachoHitbox, other, false);
        }
    }
}
