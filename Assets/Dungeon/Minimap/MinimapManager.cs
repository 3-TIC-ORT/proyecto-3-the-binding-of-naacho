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

                BoundsInt bounds = tilemapCom.cellBounds;

                Vector3Int cellCenter = new Vector3Int(
                    (bounds.xMin + bounds.xMax) / 2,
                    (bounds.yMin + bounds.yMax) / 2,
                    -1
                );

                Vector3 worldPosition = tilemapCom.CellToWorld(cellCenter);

                transform.position = worldPosition;

                Vector3Int dungeonSize = tilemapCom.size;
                float size = dungeonSize.y / 2f;
                float anchoVisible = size * 2 * 1.6f;
                if (anchoVisible < dungeonSize.x)
                {
                    size = dungeonSize.x / (2f * 1.6f);
                }
                myCamera.orthographicSize = size;

                cameraSizeSet = true;
            }
        }
    }
}
