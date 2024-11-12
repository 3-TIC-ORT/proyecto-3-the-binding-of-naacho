using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MinimapManager : MonoBehaviour
{
    private Camera myCamera;
    private bool cameraSizeSet;
    private TilemapMerger merger;
    private GameObject minimapCanvas;
    private bool minimapCanvasActivated;
    void Start()
    {
        myCamera = GetComponent<Camera>();
        merger = GameObject.FindGameObjectWithTag("Rooms").GetComponent<TilemapMerger>();
        minimapCanvas = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        SetCameraSize();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            minimapCanvas.SetActive(!minimapCanvas.activeSelf);
        }
    }
    private void SetCameraSize()
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
                    -1 // Esto igual se pone en 0 autom�ticamente
                );

                Vector3 worldPosition = tilemapCom.CellToWorld(cellCenter);

                transform.position = worldPosition;
                transform.Translate(0, 0, -1);

                Vector3Int dungeonSize = tilemapCom.size;
                float size = dungeonSize.y / 2f;
                float anchoVisible = size * 2;
                if (anchoVisible < dungeonSize.x)
                {
                    size = dungeonSize.x/2;
                }
                myCamera.orthographicSize = size;

                cameraSizeSet = true;
            }
        }
    }
}
