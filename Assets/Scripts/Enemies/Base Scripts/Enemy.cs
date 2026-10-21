using DG.Tweening;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class Enemy : MonoBehaviour, IDamageable
{
    
    [Header("Enemy Stats")]
    public string enemyName;
    public int level;
    public float baseHealth;
    public float speed;
    public float baseDamage;
    public bool canFollow = true;
    public bool shield = false;

    [Header("Objects && Components")]
    public GameObject attackHitbox;
    public EnemyHealthUI healthUI;
    Rigidbody rb;
    public GameObject sprite;
    public GameObject damageText;
    public GameObject critText;

    [Header("AI Settings")]
    public float detectionRange = 15f;
    protected bool hasDetectedPlayer = false;
    protected Transform player;
    protected NavMeshAgent agent;

    [Header("Dont Touch")]
    public float currentHealth;
    public float currentDamage;
    protected float distance;
    protected Vector3 direction;

    protected virtual void Awake()
    {
        healthUI = GetComponent<EnemyHealthUI>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        currentHealth = baseHealth * level * 2f;
        currentDamage = baseDamage * level * 2f;
        baseHealth = baseHealth * level * 2f;
        baseDamage = baseDamage * level * 2f;
        agent.updateRotation = false;
    }

    protected virtual void Update()
    {
        DirectionControl();
        if (!hasDetectedPlayer)
        {
            distance = Vector3.Distance(transform.position, player.position);

            if(distance <= detectionRange)
            {
                hasDetectedPlayer = true;
            }
        }

        if (canFollow && hasDetectedPlayer)
        {
            agent.SetDestination(player.position);
        }
        else 
        {
            agent.SetDestination(transform.position);
        }

        direction = transform.position - player.position;
    }


    public virtual void TakeDamage(float damage, float knockback, bool isCritical)
    {
        StartCoroutine(KnockbackRoutine(knockback, 0.2f));
        currentHealth -= damage;
        GameObject newDamageText = Instantiate(damageText, transform.position + new Vector3(0,1,0), Quaternion.identity);
        newDamageText.GetComponent<DamageText>().text = damage.ToString();

        if (isCritical) { Instantiate(critText, transform.position + new Vector3(0.7f, 1.5f, 0), Quaternion.identity); }

        healthUI.UpgradeUI(baseHealth, currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Attack(float damage) 
    {
    
    }


    public virtual void Die()
    {
        Debug.Log($"{enemyName} adlý düþman öldürüldü.");
        Destroy(gameObject, 0.5f);
    }

    public virtual void DirectionControl()
    {
        if(direction.x - direction.z < 0)
        {
            sprite.transform.localScale = new Vector3(1,1,1);
        }
        else
        {
            sprite.transform.localScale = new Vector3(-1,1,1);
        }
    }

    public IEnumerator KnockbackRoutine(float knockback, float time)
    {
        canFollow = false;
        Vector3 distance = (transform.position - player.position).normalized;
        rb.AddForce(new Vector3(distance.x * knockback, 0, distance.z * knockback), ForceMode.Impulse);
        yield return new WaitForSeconds(time);
        rb.linearVelocity = Vector3.zero;
        canFollow = true;
    }
}
