using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyHealthUI healthUI;
    Rigidbody rb;
    [Header("Enemy Stats")]
    public string enemyName;
    public int level;
    public float baseHealth;
    public float speed;
    public float baseDamage;
    public bool canFollow = true;

    [Header("AI Settings")]
    public float detectionRange = 15f;
    protected bool hasDetectedPlayer = false;
    protected Transform player;
    protected NavMeshAgent agent;

    [Header("Dont Touch")]
    public float currentHealth;
    public float currentDamage;

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
        if (!hasDetectedPlayer)
        {
            float distance = Vector3.Distance(transform.position, player.position);

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

        Debug.Log(agent.enabled);
    }


    public virtual void TakeDamage(float damage, float knockback)
    {
        StartCoroutine(KnockbackRoutine(knockback, 0.2f));
        currentHealth -= damage;

        healthUI.UpgradeUI(baseHealth, currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Debug.Log($"{enemyName} adlý düþman öldürüldü.");
        Destroy(gameObject);
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
