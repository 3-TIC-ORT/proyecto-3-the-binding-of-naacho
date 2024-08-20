using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class RoomConector : MonoBehaviour
{
    public int pointDirection;
    private GameObject grid;
    public TileBase tileConector;
    private Tilemap targetTilemap;
    public bool doorsDestroyed=false;
    public bool spawnPointMoved=false;
    public bool colisioneperonosecumplio = false;
    void Start()
    {
        grid = GameObject.Find("Grid");
        targetTilemap=GameObject.Find("Entry Room").GetComponent<Tilemap>();
        // FEDE CAMBIALO DESPUÉS, NO TE OLVIDES ##############################################################
        // FEDE CAMBIALO DESPUÉS, NO TE OLVIDES ##############################################################
        Invoke("DestroyThis", 3000);
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        // Si colisiono con un spawnPoint que no sea una closedRoom, una bossRoom o una treasureRoom entonces me voy a mover a mi dirección respectiva.
        if (col.gameObject.CompareTag("SpawnPoint") && col.gameObject.GetComponent<RoomSpawner>().spawnedClosedRoom == false && !col.gameObject.GetComponent<RoomSpawner>().bossRoom && !col.gameObject.GetComponent<RoomSpawner>().treasureRoom && !spawnPointMoved)
        {
            if (pointDirection == 1) transform.position += (Vector3)(Vector2.down * 10);
            else if (pointDirection == 2) transform.position += (Vector3)(Vector2.up * 10);
            else if (pointDirection == 3) transform.position += (Vector3)(Vector2.left * 13);
            else if (pointDirection == 4) transform.position += (Vector3)(Vector2.right * 13);

            spawnPointMoved = true;
        }
        // Una vez que me moví, si colisiono con una pared de Room entonces voy a empezar a destruirla
        // ACLARACIÓN: Por la forma en la que lo hice, los roomConectors solo detectan los extremos o bordes de los tiles al colisionar#####
        else if (spawnPointMoved && !doorsDestroyed && col.name != "Closed(Closed)" && col.gameObject.CompareTag("Room"))
        {
            ConnectRooms(col);
        }
        // Por la razón del comentario de arriba, compruebo que yo no esté completamente adentro de wall tiles
        // Si lo estoy, destruyo las paredes.
        else
        {
            Vector3Int gridPoint = targetTilemap.WorldToCell(transform.position);
            TileBase tile = targetTilemap.GetTile(gridPoint);
            // Si el tile en el que estoy existe, no es piso, no es una pared de closedRoom, me moví y no llame a ConnectRooms, llama
            // a ConnectRooms.
            if (tile!=null && tile!=tileConector && col.name != "Closed(Closed)" && spawnPointMoved && !doorsDestroyed)
            {
                ConnectRooms(col);
            }
        }
    }
    private void ConnectRooms(Collider2D col)
    {
        StartCoroutine(esperar());

    }
    IEnumerator esperar()
    {
        doorsDestroyed = true;
        // Lista de movimientos de roomConector para destruir cada tile.
        List<Vector2> movimientosPeristalticos = new List<Vector2>();
        if (pointDirection == 1 || pointDirection == 2)
        {
            // Movimientos necesarios para destruir y conectar habitaciones alineadas verticalmente
            movimientosPeristalticos = new List<Vector2>(CreateMovimientosPeristalticos(11, 13));
            // Ponerse abajo a la izqueirda. La esquina. 
            movimientosPeristalticos.Insert(0, Vector2.down * 5.5f);
            movimientosPeristalticos.Insert(1, Vector2.left * 6.5f);
        }
        else if (pointDirection == 3 || pointDirection == 4)
        {
            // Movimientos necesarios para destruir y conectar habitaciones alineadas horizontalmente
            movimientosPeristalticos = new List<Vector2>(CreateMovimientosPeristalticos(7, 11));
            // Ponerse abajo a la izqueirda. La esquina. 
            movimientosPeristalticos.Insert(0, Vector2.down * 3.5f);
            movimientosPeristalticos.Insert(1, Vector2.left * 5.5f);
        }


        foreach (Vector2 mov in movimientosPeristalticos)
        {
            
            Vector3Int gridPoint = targetTilemap.WorldToCell(transform.position);
            TileBase tile = targetTilemap.GetTile(gridPoint);
            if (tile != null && tile != tileConector)
            {
                targetTilemap.SetTile(gridPoint, null);
                targetTilemap.SetTile(gridPoint, tileConector);
            }
            transform.position += (Vector3)mov;
            yield return new WaitForSecondsRealtime(0.0000001f);

        }
    }
    // repeticiones, pasos. Se pueden pensar como filas y columnas respectivamente.
    List<Vector2> CreateMovimientosPeristalticos(int repeticiones, int pasos)
    {
        List<Vector2> movimientos = new List<Vector2>();
        // Hacer lo siguiente por cada fila:
        for (int i = 0; i < repeticiones+1; i++)
        {
            // Ir columna por columna
            for (int j = 0; j < pasos; j++)
            {
                movimientos.Add(Vector2.right);
            }
            // Vuelvo a la primer casilla
            movimientos.Add(Vector2.left*pasos);
            // Subo una fila
            movimientos.Add(Vector2.up);
        }
        return movimientos;
    }
    List<Tilemap> GetChildren(GameObject parent)
    {
        List<Tilemap> children = new List<Tilemap>();
        foreach (Transform child in parent.transform)
        {
            children.Add(child.gameObject.GetComponent<Tilemap>());
        }
        return children;
    }
    void DestroyThis()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
