using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool stop;
    public bool isInRoomTransition;
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
        merger = GameObject.FindGameObjectWithTag("Rooms").GetComponent<TilemapMerger>();
        StartCoroutine(WaitForTheDungeonToGenerate());
    }

    IEnumerator WaitForTheDungeonToGenerate()
    {
        SceneManager.LoadScene("Mazmorras testing");
        yield return new WaitForSecondsRealtime(0.5f);
        while (merger==null) merger = GameObject.FindGameObjectWithTag("Rooms").GetComponent<TilemapMerger>();
        while (!merger.tilemapsMerged)
        {
            merger = GameObject.FindGameObjectWithTag("Rooms").GetComponent<TilemapMerger>();
            yield return null;
        }
        yield return new WaitForSecondsRealtime(2f);
        stop = false;
    }
}
