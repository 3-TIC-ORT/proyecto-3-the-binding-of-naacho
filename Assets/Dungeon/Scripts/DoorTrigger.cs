using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public bool isVertical = false;
    public float lerpPositionDuration;
    private BoxCollider2D collider;
    private RoomTemplates templates;
    private GameObject player;
    public float OcclusionCullingDistance;
    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        player = GameObject.FindGameObjectWithTag("Player");
        // Si es una puerta vertical entonces rote la puerta 90�
        if (isVertical)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }
    private void Update()
    {
        // Occlusion Culling. Si el jugador est� lo suficientemente lejos desactive el collider para ahorrar recursos.
        if ((transform.position - player.transform.position).magnitude > OcclusionCullingDistance) collider.enabled = false;
        else collider.enabled = true;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            MoveToTheNextRoom(col.gameObject);
        }
        else if (col.gameObject.CompareTag("RoomConector")) StartCoroutine(CheckIfImNecesarry(col.gameObject));
    }
    private void ActivateEnemies(Vector2 direction)
    {
        int halfRoomDistance = 0;
        // A que distancia hay que moverse para acceder al spawnPoint
        if (direction == Vector2.left || direction == Vector2.right) halfRoomDistance = 13;
        else halfRoomDistance = 10; 
        // Acceder al spawnPoint
        Collider2D[] spawnPointArray = Physics2D.OverlapPointAll((Vector2)transform.position + (direction * halfRoomDistance));
        // Como pueden haber varios spawnPoint en un lugar, solo activa a los enemigos de los spawnPoints que tengan hijos.
        foreach (Collider2D spawnPoint in  spawnPointArray)
        {
            // enemiesPrefab es un �nico prefab que contiene varios enemigos. Siempre habr� uno solo, y no m�s, en un spawnPoint.
            List<GameObject> enemiesPrefab = GetChildren(spawnPoint.gameObject, false, "");
            if (enemiesPrefab.Count > 0)
            {
                // Enemies es cada enemigo individual dentro de enemiesPrefab.
                List<GameObject> enemies = GetChildren(enemiesPrefab[0], false, "");
                foreach (GameObject enemy in enemies)
                {
                    enemy.GetComponent<EnemTest>().enabled = true;
                }
                
            }
        }
    }
    // Mueve al jugador a la siguiente habitaci�n. �Viste? que locura.
    private void MoveToTheNextRoom(GameObject player)
    {
        Vector3 playerPos = player.GetComponent<Transform>().position;
        if (isVertical) 
        {
            // Identificar si el jugador est� a la abajo de este objeto (puerta) o no
            // y as� mover el jugador para arriba o a la abajo (en este caso que es una puerta vertical).
            if (playerPos.y - transform.position.y<0)
            {
                StartCoroutine(LerpPosition(playerPos.y, playerPos.y + 12, lerpPositionDuration, player, false));
                ActivateEnemies(Vector2.up);
            }
            else
            {
                StartCoroutine(LerpPosition(playerPos.y, playerPos.y - 12, lerpPositionDuration, player, false));
                ActivateEnemies(Vector2.down);
            }
        }
        else
        {
            if (playerPos.x - transform.position.x < 0)
            {
                StartCoroutine(LerpPosition(playerPos.x, playerPos.x + 12, lerpPositionDuration, player, true));
                ActivateEnemies(Vector2.right);
            }
            else
            {
                StartCoroutine(LerpPosition(playerPos.x, playerPos.x - 12, lerpPositionDuration, player, true));
                ActivateEnemies(Vector2.left);
            }
        }
    }
    // Mueve al jugador de una habitaci�n a otra.
    // initialAxis: valor del eje (X si es horizontal, Y si es vertical) del jugador
    // finalAxis: valor del eje del otro lado de la puerta
    // duration: que tan r�pido ser� la transici�n (segundos)
    // player: no lo se
    // isHorizontal: para saber si el eje es el eje X o Y.
    IEnumerator LerpPosition(float initialAxis, float finalAxis, float duration, GameObject player, bool isHorizontal)
    {
        float time=0;
        float currentAxis;
        Vector3 playerPos = player.GetComponent<Transform>().position;
        while (time<duration)
        {
            currentAxis = Mathf.Lerp(initialAxis, finalAxis, time / duration);
            if (isHorizontal) playerPos.x = currentAxis;
            else playerPos.y = currentAxis;
            player.GetComponent<Transform>().position = playerPos;

            time += Time.deltaTime;
            yield return null;
        }
    }
    // Se llama cuando se colisiona con un roomConector. Si las dos habitaciones est�n conectadas entonces soy necesario y no me destruyo
    // En el caso contrario, me destruyo porque como no est�n conectadas y por consiguiente se van a conectar formando una habitaci�n
    // grande, no debo existir :,(
    IEnumerator CheckIfImNecesarry(GameObject roomConector)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        RoomConector roomConectorSCR = roomConector.GetComponent<RoomConector>();
        if (!roomConectorSCR.bothRoomsAreConected) Destroy(gameObject);
    }
    List<GameObject> GetChildren(GameObject parent, bool filter, string tag)
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in parent.transform)
        {
            if (!filter)
            {
                children.Add(child.gameObject);
            }
            else if (filter && child.CompareTag(tag))
            {
                children.Add(child.gameObject);
            }
        }

        return children;
    }
}
