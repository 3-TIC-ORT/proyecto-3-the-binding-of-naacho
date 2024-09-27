using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool stop;
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
        if (merger==null) merger = GameObject.FindGameObjectWithTag("Rooms").GetComponent<TilemapMerger>();
        while (!merger.tilemapsMerged) yield return null;
        yield return new WaitForSecondsRealtime(2f);
        stop = false;
    }
}
