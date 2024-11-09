using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MinimapManager : MonoBehaviour
{
    private Camera myCamera;
    private bool cameraSizeSet;
    private TilemapMerger merger;
    void Start()
    {
        myCamera = GetComponent<Camera>();
        merger = GameObject.FindGameObjectWithTag("Rooms").GetComponent<TilemapMerger>();
    }

    void Update()
    {
        if (!cameraSizeSet)
        {
            if (merger.tilemapsMerged)
            {
                Tilemap tilemapCom = GameObject.Find("Entry Room").GetComponent<Tilemap>();
                Vector3Int dungeonSize = tilemapCom.size;
                float size = dungeonSize.y / 2;
                float anchoVisible = size * 2 * 1.6f;
                if (anchoVisible < dungeonSize.x) size = dungeonSize.x / (2 * 1.6f);
                myCamera.orthographicSize= size;
                cameraSizeSet = true;
                transform.position = new Vector3(tilemapCom.transform.position.x,tilemapCom.transform.position.y,transform.position.z);
            }
        }
    }
}
