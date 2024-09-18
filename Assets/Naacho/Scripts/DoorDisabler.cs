using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class DoorDisabler : MonoBehaviour
{
    private GameObject grid;
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
                        Light2D _light = col.GetComponent<Light2D>();
                        _light.enabled = enableDoorLights;
                    }
            }
        }
    }

    void Update() {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, ActivationArea.size, 0);
        int enemyCounter = 0;
        foreach(Collider2D col in colliders) {
            if(col.CompareTag("Enemy")) {
                isFighting = true;
                break;
            }
            isFighting = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("SpawnPoint") && isFighting) {
            changeDoorsSprite(tileClosed, (Vector2)other.gameObject.transform.position, other.GetComponent<RoomSpawner>().treasureRoom);
        }
        if(other.gameObject.CompareTag("Enemy")) {
            isFighting = true;
            other.GetComponent<Enemy>().enabled = true;
            other.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("SpawnPoint") && !isFighting) {
            changeDoorsSprite(tileOpened, (Vector2)other.gameObject.transform.position, other.GetComponent<RoomSpawner>().treasureRoom);
        }
        if(other.gameObject.CompareTag("Enemy")) {
            isFighting = false;
            other.GetComponent<Enemy>().enabled = false;
            other.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
