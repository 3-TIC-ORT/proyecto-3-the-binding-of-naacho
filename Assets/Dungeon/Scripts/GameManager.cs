using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static int depth;
    public List<Color> dungeonColors = new List<Color>();
    public Material dungeonMaterial;
    public bool stop;
    public float fadeSpeed;
    private Image screen;
    private TilemapMerger merger;
    [Tooltip("Marca si la c�mara se est� moviendo al Naacho ser lastimado")]
    public bool nachoNullPrinted;
    private bool haveToDie;
    private void Awake()
    {
        Application.targetFrameRate = 55;
        DontDestroyOnLoad(gameObject);
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Update()
    {
        if (haveToDie && FadeManager.Instance.fadeInFinished)
        {
            SceneManager.LoadScene("GameOver");
            Destroy(gameObject);
        }
        if (Input.GetKeyDown(KeyCode.P)) Application.targetFrameRate = 55;
        else if (Input.GetKeyDown(KeyCode.O)) Application.targetFrameRate = 30;
        else if (Input.GetKeyDown(KeyCode.I)) Application.targetFrameRate = 10;
        else if (Input.GetKeyDown(KeyCode.U)) Application.targetFrameRate = 5;
    }
    void Start()
    {
        depth = -1;
        stop = true;
        screen = GameObject.FindGameObjectWithTag("TransitionScreen").GetComponent<Image>();
        merger = GameObject.FindGameObjectWithTag("Rooms").GetComponent<TilemapMerger>();
        StartCoroutine(WaitForTheDungeonToGenerate(false));
    }

    IEnumerator WaitForTheDungeonToGenerate(bool reloadTheScene)
    {
        UpdateDungeonMaterial();
        if (reloadTheScene)
        {
            if (depth<3) SceneManager.LoadScene("Mazmorras testing");
            else
            {
                SceneManager.LoadScene("WinCinematic");
                Destroy(gameObject);
            }
        }
        PlayerManager.Instance.GetComponent<Transform>().position = new Vector3(0, 0, 0);
        yield return new WaitForSecondsRealtime(0.5f);
        while (merger==null) merger = GameObject.FindGameObjectWithTag("Rooms").GetComponent<TilemapMerger>();
        while (!merger.tilemapsMerged)
        {
            merger = GameObject.FindGameObjectWithTag("Rooms").GetComponent<TilemapMerger>();
            yield return null;
        }
        yield return new WaitForSecondsRealtime(2f);
        FadeManager.Instance.FadeOut();
        stop = false;
    }
    public void Death()
    {
        DG.Tweening.DOTween.KillAll();
        FadeManager.Instance.FadeIn();
        haveToDie = true;
    }
    public IEnumerator ReloadScene()
    {
        FadeManager.Instance.FadeIn();
        yield return null;
        while (!FadeManager.Instance.fadeInFinished)
        {
            yield return null;
        }
        StartCoroutine(WaitForTheDungeonToGenerate(true));
    }
    public void UpdateDungeonMaterial()
    {
        depth++;
    }
}
