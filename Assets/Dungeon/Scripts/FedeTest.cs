using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
public class FedeTest : MonoBehaviour
{
    private GameObject grid;
    public Tilemap targetTilemap;
    public TileBase tilex;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("ASD");
    }
    IEnumerator ASD()
    {
        yield return new WaitForSecondsRealtime(0);
        changeDoorsSprite(tilex, new Vector2(0, 0), false);
    }
    // Dale el nuevo Tile para la puerta. Dale la posición del spawnPoint detectado.
    private void changeDoorsSprite(TileBase tile, Vector2 spawnPointPos, bool enableDoorLights)
    {
        // Array con las 4 direcciones con las magnitudes correspondientes donde podría haber una puerta
        Vector2[] fourDirections= {Vector2.up*4.5f,Vector2.down * 4.5f, Vector2.left*7.5f,Vector2.right*7.5f};
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
    
}
