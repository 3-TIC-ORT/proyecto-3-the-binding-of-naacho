using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [Header("Doors' sprites animation")]
    public Sprite[] leftUpDoorAnimation;
    public Sprite[] leftDownDoorAnimation;
    public Sprite[] rightUpDoorAnimation;
    public Sprite[] rightDownDoorAnimation;
    public Sprite[] topLeftDoorAnimation;
    public Sprite[] topRightDoorAnimation;
    public Sprite[] downLeftDoorAnimation;
    public Sprite[] downRightDoorAnimation;
    [Tooltip("El tiempo que tarda una puerta en cerrarse o abrirse")]
    public float doorAnimationTime;
    EnemyEnabler enemyEnabler;
    public bool isFighting;
    BoxCollider2D ActivationArea;
    public List<int> enemiesActivatedIDs = new List<int>();
    public List<int> enemiesIDsRecorded = new List<int>();
    // No es que los spawnPoints puedan estar cerrados, sino que sus puertas si lo están.
    public List<int> spawnPointsClosed = new List<int>();

    void Start() {
        targetTilemap = GameObject.Find("Entry Room").GetComponent<Tilemap>();
        roomTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        ActivationArea = GetComponent<BoxCollider2D>();
    }
    public class VectorWithUnit
    {
        public Vector2 originalVector;
        public Vector2 unitVector;

        public VectorWithUnit(Vector2 originalVectorP, Vector2 unitVectorP)
        {
            originalVector = originalVectorP;
            unitVector = unitVectorP;
        }
    }
    // Dale el nuevo Tile para la puerta. Dale la posición del spawnPoint detectado.
    private void detectDoors(bool closing, Vector2 spawnPointPos, bool enableDoorLights)
    {
        float hDistanciaBetweenDoorAndSpawnPoint = roomTemplates.centerBetweenHorizontalRooms * 2 - (roomTemplates.centerBetweenHorizontalRooms + roomTemplates.horizontalDoorToDoorRoomArea.x / 2);
        float vDistanciaBetweenDoorAndSpawnPoint = roomTemplates.centerBetweenVerticaltalRooms * 2 - (roomTemplates.centerBetweenVerticaltalRooms + roomTemplates.verticalDoorToDoorRoomArea.y / 2);
        // Array con las 4 direcciones con las magnitudes correspondientes donde podría haber una puerta
        List<VectorWithUnit> fourDirections = new List<VectorWithUnit>
        {
            new VectorWithUnit(Vector2.up*(vDistanciaBetweenDoorAndSpawnPoint+0.5f),Vector2.up),
            new VectorWithUnit(Vector2.down*(vDistanciaBetweenDoorAndSpawnPoint+0.5f),Vector2.down),
            new VectorWithUnit(Vector2.left*(hDistanciaBetweenDoorAndSpawnPoint+0.5f),Vector2.left),
            new VectorWithUnit(Vector2.right*(hDistanciaBetweenDoorAndSpawnPoint+0.5f),Vector2.right)
        };
        foreach (VectorWithUnit direction in fourDirections)
        {
            Vector2 doorPos = spawnPointPos + direction.originalVector;
            // Door es el collider de doorTrigger
            Collider2D[] colliders = Physics2D.OverlapBoxAll(doorPos, new Vector2(0.5f, 0.5f),0);
            foreach (Collider2D col in colliders)
            {
                if ( col!=null && col.gameObject.CompareTag("DoorTrigger"))
                {
                    Debug.Log(direction.unitVector);
                    ChangeDoor(closing, doorPos, direction.unitVector);
                }
                else if (col != null && col.gameObject.CompareTag("RoomLight"))
                {
                    col.GetComponent<Light2D>().enabled = enableDoorLights;
                }
            }
        }
    }
    // Si doorDirection es (1,0) entonces la puerta es la puerta de la derecha de la room.
    void ChangeDoor(bool closing, Vector2 doorPos, Vector2 doorDirection)
    {
        List<List<Sprite>> sprites = CorrespondingSprites(doorDirection);
        if (!closing)
        {
            foreach (Sprite sprite in sprites[0])
            {
                Debug.Log(sprite.name);
            }
            StartCoroutine(ChangeDoorSprite(sprites, doorPos, false));
        }
        else
        {
            sprites[0].Reverse();
            sprites[1].Reverse();
            foreach(Sprite sprite in sprites[0])
            {
                Debug.Log(sprite.name);
            }
            StartCoroutine(ChangeDoorSprite(sprites, doorPos,true));
        }
    }
    IEnumerator ChangeDoorSprite(List<List<Sprite>> sprites, Vector2 doorPos, bool closing)
    {
        // Uso la longitud de leftUpDoorAnimation porque da lo mismo, todas las animaciones deberían tener la misma
        // cantidad de frames
        for (int i = 0; i < leftUpDoorAnimation.Length; i++)
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            if (!closing) tile.colliderType=Tile.ColliderType.None;
            tile.sprite = sprites[1][i];
            targetTilemap.SetTile(targetTilemap.WorldToCell(doorPos), tile);
            tile.sprite = sprites[0][i];
            targetTilemap.SetTile(targetTilemap.WorldToCell(doorPos + new Vector2(-0.01f, -0.01f)), tile);
            yield return new WaitForSecondsRealtime(doorAnimationTime);
        }
    }
    List<List<Sprite>> CorrespondingSprites(Vector2 doorDirection)
    {
        List<List<Sprite>> list = new List<List<Sprite>>();
        if (doorDirection==Vector2.left)
        {
            list.Add(leftDownDoorAnimation.ToList<Sprite>());
            list.Add(leftUpDoorAnimation.ToList<Sprite>());
        }
        else if (doorDirection==Vector2.right) 
        {
            list.Add(rightDownDoorAnimation.ToList<Sprite>());
            list.Add(rightUpDoorAnimation.ToList<Sprite>());
        }
        else if (doorDirection == Vector2.up) 
        {
            list.Add(topLeftDoorAnimation.ToList<Sprite>());
            list.Add(topRightDoorAnimation.ToList<Sprite>());
        }
        else
        {
            list.Add(downLeftDoorAnimation.ToList<Sprite>());
            list.Add(downRightDoorAnimation.ToList<Sprite>());
        }
        return list;
    }
    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapAreaAll(transform.position - new Vector3(AreaWidth, AreaHeight, 0), transform.position + new Vector3(AreaWidth, AreaHeight, 0));

        //Debug.DrawLine(transform.position-new Vector3(AreaWidth, 0, 0), transform.position+new Vector3(AreaWidth, 0, 0), Color.red, 0.3f);
        //Debug.DrawLine(transform.position-new Vector3(0, AreaHeight, 0), transform.position+new Vector3(0, AreaHeight, 0), Color.red, 0.3f);

        List<GameObject> spawnPointsList = new List<GameObject>();
        foreach (Collider2D col in colliders)
        {
            if (col != null && col.CompareTag("Enemy"))
            {
                int enemyID = col.gameObject.GetComponent<Enemy>().ID;
                if (!enemiesIDsRecorded.Contains(enemyID))
                {
                    if (enemyID != 0)
                    {
                        enemiesIDsRecorded.Add(enemyID);
                        enemiesActivatedIDs.Add(enemyID);
                    }
                }
                col.GetComponent<Enemy>().enabled = true;
            }
            else if (col != null && col.CompareTag("SpawnPoint"))
            {
                spawnPointsList.Add(col.gameObject);
            }

            if (enemiesActivatedIDs.Count > 0)
            {
                if (targetTilemap != null)
                {
                    isFighting = true;
                    foreach (GameObject spawnPoint in spawnPointsList)
                    {
                        RoomSpawner roomSpawner = spawnPoint.GetComponent<RoomSpawner>();
                        if (!spawnPointsClosed.Contains(roomSpawner.ID))
                        {
                            spawnPointsClosed.Add(roomSpawner.ID);
                            Vector2 position = roomSpawner.gameObject.transform.position;
                            detectDoors(true, position, false);
                        }
                    }
                }
                else targetTilemap = GameObject.Find("Entry Room").GetComponent<Tilemap>();
            }
            else
            {
                if (targetTilemap != null)
                {
                    isFighting = false;
                    foreach (GameObject spawnPoint in spawnPointsList)
                    {
                        RoomSpawner roomSpawner = spawnPoint.GetComponent<RoomSpawner>();
                        if (spawnPointsClosed.Contains(roomSpawner.ID))
                        {
                            spawnPointsClosed.Remove(roomSpawner.ID);
                            Vector2 position = roomSpawner.gameObject.transform.position;
                            detectDoors(false, position, true);
                        }
                    }
                }
                else targetTilemap = GameObject.Find("Entry Room").GetComponent<Tilemap>();
            }
        }
    }
}
