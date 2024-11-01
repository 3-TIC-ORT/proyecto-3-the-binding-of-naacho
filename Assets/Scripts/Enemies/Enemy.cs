using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class Enemy : MonoBehaviour
{
    public string Name;

    public float HealthPoints;
    public float DamagePoints;

    public uint Speed;
    public bool canRecieveKnockback = true;
    public bool isBoss;
    public bool hasKnockback = false;

    protected SpriteRenderer SpRenderer;
    protected Color defaultColor;
    protected BoxCollider2D Col2D;
    protected Rigidbody2D rb2D;
    protected GameObject Player;
    protected float maxHealth;
    protected DoorDisabler doorDisabler;
    protected bool isDead = false;
    // Es para leerlo desde otros scripts. No se usa.
    public float readableMaxHealth;
    public int ID;
    // Start is called before the first frame update
    public virtual void Start()
    {
        ID = GetInstanceID();
        Player = GameObject.Find("Naacho");
        rb2D = GetComponent<Rigidbody2D>();
        Col2D = GetComponent<BoxCollider2D>();
        SpRenderer = GetComponent<SpriteRenderer>();
        doorDisabler=PlayerManager.Instance.GetComponent<DoorDisabler>();
        defaultColor = SpRenderer.color;
        isBoss = false;
        maxHealth = HealthPoints;
        readableMaxHealth = maxHealth;
        CheckFourDirections();
        AddSelfToPlayerList();
    }
    public void AddSelfToPlayerList()
    {
        DoorDisabler doorDisabler = Player.GetComponent<DoorDisabler>();
        doorDisabler.enemiesActivatedIDs.Add(ID);
        doorDisabler.enemiesIDsRecorded.Add(ID);
    }
    // Update is called once per frame
    public virtual void Update() {
        if (Player != null) Player.GetComponent<DoorDisabler>().isFighting = true;
        else if (!GameManager.Instance.nachoNullPrinted)
        {
            Debug.LogWarning("Che macho, Naacho es null");
            GameManager.Instance.nachoNullPrinted = true;
        }
        if(hasKnockback) return;
    }
    public virtual void CheckFourDirections()
    {
        Vector2[] directions = {Vector2.up, Vector2.down,Vector2.right,Vector2.left};
        foreach (Vector2 direction in directions) ActivateEnemiesChain(direction);
    }
    public virtual void ActivateEnemiesChain(Vector2 direction)
    {
        Ray ray = new Ray(transform.position, direction);
        float magnitude=1;
        while (true)
        {
            RaycastHit2D[] colliders = Physics2D.RaycastAll(ray.origin, ray.direction, magnitude);
            foreach (RaycastHit2D collider in colliders)
            {
                if (collider.collider.gameObject.CompareTag("Enemy"))
                {
                    EnemyEnabler enemyEnablerScript = collider.collider.gameObject.GetComponent<EnemyEnabler>();
                    if (!enemyEnablerScript.enemyEnabled)
                    {
                        enemyEnablerScript.SetComponents(true);
                    }
                }
                else if (collider.collider.gameObject.CompareTag("Room"))
                {
                    return;
                }
            }
            magnitude++;
        }
    }
    public virtual void OnTriggerEnter2D(Collider2D collision) {}

    public virtual void Damage(float dp)
    {
        HealthPoints -= dp;
        StartCoroutine(VisualDamage());
        if (HealthPoints <= 0 && !isDead) {
            isDead = true;
            StartCoroutine(OnDeath());
        }
    }

    public virtual IEnumerator OnDeath() {
        yield return null;
        if (doorDisabler.enemiesIDsRecorded.Contains(ID)) doorDisabler.enemiesActivatedIDs.Remove(ID);
        ParticlesManager.Instance.InstanceDeathParticle(transform.position);
        Destroy(gameObject);
    }

    public virtual IEnumerator VisualDamage()
    {
        SpRenderer.color = Color.red;

        while (SpRenderer.color.g + SpRenderer.color.b < 2f)
        {
            SpRenderer.color = new Color(SpRenderer.color.r, SpRenderer.color.g+0.05f, SpRenderer.color.b+0.05f);
            yield return null;
        }
        SpRenderer.color = defaultColor;
    }
}
