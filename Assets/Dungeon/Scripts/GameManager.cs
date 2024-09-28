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
    public bool isInRoomTransition;
    private Image screen;
    private TilemapMerger merger;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance != null && Instance != this)
        {
            Destroy(this);
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
        StartCoroutine(WaitForTheDungeonToGenerate());
    }

    IEnumerator WaitForTheDungeonToGenerate()
    {
        Fade(true, false);
        SceneManager.LoadScene("Mazmorras testing");
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
    private void Fade(bool IN, bool OUT)
    {
        if (IN)
        {
            screen.DOFade(1, 1f);
        }
        else if (OUT)
        {
            screen.DOFade(0, 1f);
        }
    }
}
