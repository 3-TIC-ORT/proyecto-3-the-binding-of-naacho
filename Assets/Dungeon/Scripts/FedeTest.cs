using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        changeDoorsSprite(tilex, new Vector2(0, 0));
    }
    // Dale el nuevo Tile para la puerta. Dale la posici�n del spawnPoint detectado.
    private void changeDoorsSprite(TileBase tile, Vector2 spawnPointPos)
    {
        // Array con las 4 direcciones con las magnitudes correspondientes donde podr�a haber una puerta
        Vector2[] fourDirections= {Vector2.up*4.5f,Vector2.down * 4.5f, Vector2.left*7.5f,Vector2.right*7.5f};
        foreach (Vector2 direction in fourDirections)
        {
            Vector2 doorPos = spawnPointPos + direction;
            // Door es el collider de doorTrigger
            Collider2D door = Physics2D.OverlapBox(doorPos, new Vector2(0.5f, 0.5f),0);
            if (door != null) 
            {
                if (door.gameObject.CompareTag("DoorTrigger"))
                {
                    // Cambia el tile de la puerta
                    targetTilemap.SetTile(targetTilemap.WorldToCell(doorPos), tile);
                    // Cambia el tile de la otra puerta al restarle un poco de posici�n (mala pr�tica, lo s�, pero me gan� la tarea para el cole)
                    targetTilemap.SetTile(targetTilemap.WorldToCell(doorPos+new Vector2(-0.01f,-0.01f)), tile);
                }
            }
        }
    }
    
}
