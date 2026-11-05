using DG.Tweening;
using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class Enemy : MonoBehaviour, IDamageable
{
    public System.Action OnDeath;

    private SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock propertyBlock;

    [Header("Enemy Stats")]
    public string enemyName;
    public int level;
    public float baseHealth;
    public float speed;
    public float baseDamage;
    public bool canFollow = true;
    public bool shield = false;
    public bool isDead = false;
    public float exploitChance;

    [Header("Objects && Components")]
    public GameObject attackHitbox;
    public EnemyHealthUI healthUI;
    Rigidbody rb;
    public GameObject sprite;
    public GameObject damageText;
    public GameObject critText;
    public GameObject exploitButton;

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

        propertyBlock = new MaterialPropertyBlock();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        
        if (!hasDetectedPlayer)
        {
            distance = Vector3.Distance(transform.position, player.position);

            if(distance <= detectionRange)
            {
                hasDetectedPlayer = true;
            }
        }

        if (canFollow && hasDetectedPlayer && !isDead)
        {
            agent.SetDestination(player.position);
            DirectionControl();
        }
        else 
        {
            agent.SetDestination(transform.position);
        }

        direction = transform.position - player.position;
    }


    public virtual void TakeDamage(float damage, float knockback, bool isCritical)
    {
        if(!isDead)
        {
            StartCoroutine(KnockbackRoutine(knockback, 0.2f));
            StartCoroutine(FlashEffectRoutine());
            currentHealth -= damage;
            GameObject newDamageText = Instantiate(damageText, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
            newDamageText.GetComponent<DamageText>().text = damage.ToString();

            if (isCritical) { Instantiate(critText, transform.position + new Vector3(0.7f, 2f, 0), Quaternion.identity); }

            ExploitChance();

            healthUI.UpgradeUI(baseHealth, currentHealth);

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public virtual void Attack(float damage) 
    {
    
    }

    public void ExploitChance()
    {
        if(Random.value < exploitChance && !isDead)
        {
            GameObject newExploitButton = Instantiate(exploitButton, transform.position, Quaternion.identity);
            newExploitButton.GetComponent<ExploitManager>().enemyCs = this;
        }
    }


    public virtual void Die()
    {
        isDead = true;
        Debug.Log($"{enemyName} adlý düþman öldürüldü.");
        OnDeath?.Invoke();
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

    public void SetFlash(float flashAmount)
    {
        spriteRenderer.GetPropertyBlock(propertyBlock);

        propertyBlock.SetFloat("_HitFlashAmount", flashAmount);

        spriteRenderer.SetPropertyBlock(propertyBlock);
    }

    public IEnumerator FlashEffectRoutine()
    {
        SetFlash(1);
        yield return new WaitForSeconds(0.08f);
        SetFlash(0);
    }

    public IEnumerator KnockbackRoutine(float knockback, float time)
    {
        canFollow = false;
        rb.linearVelocity = Vector3.zero;
        Vector3 distance = (transform.position - player.position).normalized;
        rb.AddForce(new Vector3(distance.x * knockback, 0, distance.z * knockback), ForceMode.Impulse);
        yield return new WaitForSeconds(time);
        rb.linearVelocity = Vector3.zero;
        yield return new WaitForSeconds(0.2f);
        canFollow = true;
    }
}
