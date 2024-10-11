using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class DoorDisabler : MonoBehaviour
{
    private GameObject grid;
    private RoomTemplates roomTemplates;
    public float AreaWidth;
    public float AreaHeight;
    public Tilemap targetTilemap;
    public TileBase tileOpened;
    public TileBase tileClosed;
    EnemyEnabler enemyEnabler;
    public bool isFighting;
    BoxCollider2D ActivationArea;
    public List<int> enemiesActivatedIDs = new List<int>();
    public List<int> enemiesIDsRecorded = new List<int>();

    void Start() {
        targetTilemap = GameObject.Find("Entry Room").GetComponent<Tilemap>();
        roomTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        ActivationArea = GetComponent<BoxCollider2D>();
    }

    // Dale el nuevo Tile para la puerta. Dale la posición del spawnPoint detectado.
    private void changeDoorsSprite(TileBase tile, Vector2 spawnPointPos, bool enableDoorLights)
    {
        float hDistanciaBetweenDoorAndSpawnPoint = roomTemplates.centerBetweenHorizontalRooms * 2 - (roomTemplates.centerBetweenHorizontalRooms + roomTemplates.horizontalDoorToDoorRoomArea.x / 2);
        float vDistanciaBetweenDoorAndSpawnPoint = roomTemplates.centerBetweenVerticaltalRooms * 2 - (roomTemplates.centerBetweenVerticaltalRooms + roomTemplates.verticalDoorToDoorRoomArea.y / 2);
        // Array con las 4 direcciones con las magnitudes correspondientes donde podría haber una puerta
        Vector2[] fourDirections= 
        {
            Vector2.up*(vDistanciaBetweenDoorAndSpawnPoint+0.5f),
            Vector2.down * (vDistanciaBetweenDoorAndSpawnPoint+0.5f), 
            Vector2.left*(hDistanciaBetweenDoorAndSpawnPoint+0.5f),
            Vector2.right*(hDistanciaBetweenDoorAndSpawnPoint+0.5f)
        };
        foreach (Vector2 direction in fourDirections)
        {
            Vector2 doorPos = spawnPointPos + direction;
            // Door es el collider de doorTrigger
            Collider2D[] colliders = Physics2D.OverlapBoxAll(doorPos, new Vector2(0.5f, 0.5f),0);
            foreach (Collider2D col in colliders)
            {
                if ( col!=null && col.gameObject.CompareTag("DoorTrigger"))
                {
                    // Cambia el tile de la puerta
                    targetTilemap.SetTile(targetTilemap.WorldToCell(doorPos), tile);
                    // Cambia el tile de la otra puerta al restarle un poco de posición (mala prática, lo sé, pero me ganó la tarea para el cole)
                    targetTilemap.SetTile(targetTilemap.WorldToCell(doorPos+new Vector2(-0.01f,-0.01f)), tile);
                }
                else if (col != null && col.gameObject.CompareTag("RoomLight"))
                {
                    col.GetComponent<Light2D>().enabled = enableDoorLights;
                }
            }
        }
    }

    void Update() {
        Collider2D[] colliders = Physics2D.OverlapAreaAll(transform.position - new Vector3(AreaWidth, AreaHeight, 0), transform.position + new Vector3(AreaWidth, AreaHeight, 0));

        //Debug.DrawLine(transform.position-new Vector3(AreaWidth, 0, 0), transform.position+new Vector3(AreaWidth, 0, 0), Color.red, 0.3f);
        //Debug.DrawLine(transform.position-new Vector3(0, AreaHeight, 0), transform.position+new Vector3(0, AreaHeight, 0), Color.red, 0.3f);

        List<Vector2> spawnPointsPositions=new List<Vector2>();
        foreach(Collider2D col in colliders) {
            if (col!=null && col.CompareTag("Enemy"))
            {
                int enemyID= col.gameObject.GetComponent<Enemy>().ID;
                if (!enemiesIDsRecorded.Contains(enemyID)) 
                {
                    if (enemyID!=0)
                    {
                        enemiesIDsRecorded.Add(enemyID);
                        enemiesActivatedIDs.Add(enemyID); 
                    }
                }
                col.GetComponent<Enemy>().enabled = true;
            }
            else if (col != null && col.CompareTag("SpawnPoint"))
            {
                spawnPointsPositions.Add((Vector2)col.gameObject.transform.position);
            }

            if (enemiesActivatedIDs.Count>0)
            {
                if (targetTilemap!=null)
                {
                    isFighting = true;
                    foreach (Vector2 position in spawnPointsPositions) 
                    {
                        changeDoorsSprite(tileClosed, position, false);
                    }
                }
                else targetTilemap = GameObject.Find("Entry Room").GetComponent<Tilemap>();
            }
            else
            {
                if (targetTilemap != null)
                {
                    isFighting = false;
                    foreach (Vector2 position in spawnPointsPositions)
                    {
                        changeDoorsSprite(tileOpened, position, true);
                    }
                }
                else targetTilemap = GameObject.Find("Entry Room").GetComponent<Tilemap>();
            }
        }
    }
}
