using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool stop;
    public float fadeSpeed;
    private Image screen;
    private TilemapMerger merger;
    [Tooltip("Marca si la cámara se está moviendo al Naacho ser lastimado")]
    public bool nachoNullPrinted;
    
    private void Awake()
    {
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
    void Start()
    {
        stop = true;
        screen = transform.GetChild(0).transform.GetComponentInChildren<Image>();
        merger = GameObject.FindGameObjectWithTag("Rooms").GetComponent<TilemapMerger>();
        StartCoroutine(WaitForTheDungeonToGenerate(false));
    }

    IEnumerator WaitForTheDungeonToGenerate(bool reloadTheScene)
    {
        if (reloadTheScene) SceneManager.LoadScene("Mazmorras testing");
        PlayerManager.Instance.GetComponent<Transform>().position = new Vector3(0, 0, 0);
        yield return new WaitForSecondsRealtime(0.5f);
        while (merger==null) merger = GameObject.FindGameObjectWithTag("Rooms").GetComponent<TilemapMerger>();
        while (!merger.tilemapsMerged)
        {
            merger = GameObject.FindGameObjectWithTag("Rooms").GetComponent<TilemapMerger>();
            yield return null;
        }
        yield return new WaitForSecondsRealtime(2f);
        Fade(false, true);
        stop = false;
    }
    public void Fade(bool IN, bool OUT)
    {
        if (IN)
        {
            screen.DOFade(1, fadeSpeed).onComplete=()=> { StartCoroutine(WaitForTheDungeonToGenerate(true)); };
        }
        else if (OUT)
        {
            screen.DOFade(0, 1f);
        }
    }
}
