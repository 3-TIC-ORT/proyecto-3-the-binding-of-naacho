using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class DoorDisabler : MonoBehaviour
{
    private GameObject grid;
    public float AreaWidth;
    public float AreaHeight;
    public Tilemap targetTilemap;
    public TileBase tileOpened;
    public TileBase tileClosed;
    EnemyEnabler enemyEnabler;
    public bool isFighting;
    BoxCollider2D ActivationArea;

    void Start() {
        targetTilemap = GameObject.Find("Entry Room").GetComponent<Tilemap>();
        ActivationArea = GetComponent<BoxCollider2D>();
    }

    // Dale el nuevo Tile para la puerta. Dale la posición del spawnPoint detectado.
    private void changeDoorsSprite(TileBase tile, Vector2 spawnPointPos, bool enableDoorLights)
    {
        // Array con las 4 direcciones con las magnitudes correspondientes donde podría haber una puerta
        Vector2[] fourDirections= {Vector2.up*5.5f,Vector2.down * 5.5f, Vector2.left*8.5f,Vector2.right*8.5f};
        foreach (Vector2 direction in fourDirections)
        {
            Vector2 doorPos = spawnPointPos + direction;
            // Door es el collider de doorTrigger
            Collider2D[] colliders = Physics2D.OverlapBoxAll(doorPos, new Vector2(0.5f, 0.5f),0);
            foreach (Collider2D col in colliders)
            {
                if (col.gameObject.CompareTag("DoorTrigger"))
                {
                    // Cambia el tile de la puerta
                    targetTilemap.SetTile(targetTilemap.WorldToCell(doorPos), tile);
                    // Cambia el tile de la otra puerta al restarle un poco de posición (mala prática, lo sé, pero me ganó la tarea para el cole)
                    targetTilemap.SetTile(targetTilemap.WorldToCell(doorPos+new Vector2(-0.01f,-0.01f)), tile);
                }
                else if (col.gameObject.CompareTag("RoomLight"))
                {
                    col.GetComponent<Light2D>().enabled = enableDoorLights;
                }
            }
        }
    }

    void Update() {
        Collider2D[] colliders = Physics2D.OverlapAreaAll(transform.position - new Vector3(AreaWidth, AreaHeight, 0), transform.position + new Vector3(AreaWidth, AreaHeight, 0));

        Debug.DrawLine(transform.position-new Vector3(AreaWidth, 0, 0), transform.position+new Vector3(AreaWidth, 0, 0), Color.red, 0.3f);
        Debug.DrawLine(transform.position-new Vector3(0, AreaHeight, 0), transform.position+new Vector3(0, AreaHeight, 0), Color.red, 0.3f);

        int enemiesAmount = 0;
        List<Vector2> spawnPointsPositions=new List<Vector2>();
        foreach(Collider2D col in colliders) {
            if (col.CompareTag("Enemy"))
            {
                enemiesAmount++;
                col.GetComponent<Enemy>().enabled = true;
            }
            else if (col.CompareTag("SpawnPoint"))
            {
                spawnPointsPositions.Add((Vector2)col.gameObject.transform.position);

            }

            if (enemiesAmount >0 || isFighting)
            {
                isFighting = true;
                foreach (Vector2 position in spawnPointsPositions) 
                {
                    changeDoorsSprite(tileClosed, position, false);
                }
            } else if (enemiesAmount==0)
            {
                isFighting = false;
                foreach (Vector2 position in spawnPointsPositions)
                {
                    changeDoorsSprite(tileOpened, position, true);

                }
            }
        }
    }
}
