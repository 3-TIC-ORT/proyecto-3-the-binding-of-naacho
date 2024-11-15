using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(RoomSpawner))]
public class DecorationSpawner : MonoBehaviour
{
    private RoomTemplates templates;
    private RoomSpawner roomSpawner;
    private bool spawnObstacles;
    private bool decorationSpawned;
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        roomSpawner = GetComponent<RoomSpawner>();
        spawnObstacles = Random.value>0.5f;
    }
    void Update()
    {
        if (roomSpawner.spawned && !roomSpawner.spawnedClosedRoom)
        {
            if (spawnObstacles && !roomSpawner.specialRoom)
            {
                SpawnObstacles();
            }
            else if (!spawnObstacles)
            {
                // Spawnear algo normal, como piedritas. Cosas que no afecten al jugador
            }
        }
    }
    // Por ahora solo son tiles.
    private void SpawnObstacles()
    {

    }
}
[System.Serializable]
public class Matrix
{
    public int rows;
    public int columns;
    public List<List<bool>> data;

    public Matrix(int rows, int columns)
    {
        this.rows=rows;
        this.columns=columns;
        data = new List<List<bool>>();

        for (int x=0; x<rows; x++)
        {
            List<bool> row = new List<bool>();
            for (int y=0; y<columns; y++)
            {
                row.Add(false);
            }
            data.Add(row);
        }
    }
}
public class ArrayOfMatrices : MonoBehaviour
    {
        public int matricesAmount;
        private RoomTemplates templates;
        private void Start() 
        {
            templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        }
        public List<Matrix> matrices;
        [ContextMenu(" Crear Array de Matrices")]
        private void CreateMatricesDecoration()
        {
            matrices = new List<Matrix>
            {
                new Matrix((int)templates.insideRoomArea.x,(int)templates.insideRoomArea.y)
            };
        }

    }