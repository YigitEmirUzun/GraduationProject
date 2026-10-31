using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Stats")]
    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private float baseKnockback = 1f;
    [SerializeField] private float baseCritChance = 0.1f;
    [SerializeField] private float baseCritMultiplier = 2f;
    [SerializeField] private float baseAttackDelay = 0.4f;

    public float currentDamage;
    public float currentKnockback;
    public float currentCritChance;
    public float currentCritMultiplier;
    public float currentAttackDelay;

    public GameObject attackHitbox;
    public Transform spawnPos;

    public PlayerController playerController;

    
    public bool canAttack;
    public bool exploit = false;
    public float exploitTime = 0.5f;
    public bool isCritical;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        
        currentDamage = baseDamage;
        currentKnockback = baseKnockback;
        currentCritChance = baseCritChance;
        currentCritMultiplier = baseCritMultiplier;
        currentAttackDelay = baseAttackDelay;
    }

    public float CalculatedDamage()
    {
        isCritical = Random.value < currentCritChance;

        float damage = currentDamage;

        if (isCritical)
            damage *= currentCritMultiplier;

        return damage;
    }

    public void Attack()
    {
        if (canAttack)
        {
            StartCoroutine(AttackSpeedRoutine());
            StartCoroutine(StopMovementRoutine());
            float damageGiven = CalculatedDamage();
            GameObject hitbox = Instantiate(attackHitbox, spawnPos.position, Quaternion.Euler(0f, 45f, 0f));
            hitbox.GetComponent<AttackHitbox>().Initialize(damageGiven, currentKnockback,isCritical);
            isCritical = false;
        }
    }

    public void Exploit()
    {
        if (!exploit)
        {
            StartCoroutine(ExploitRoutine());
        }
    }

    IEnumerator AttackSpeedRoutine()
    {
        canAttack = false;
        yield return new WaitForSeconds(currentAttackDelay);
        canAttack = true;
    }

    IEnumerator StopMovementRoutine()
    {
        playerController.canMove = false;
        yield return new WaitForSeconds(0.1f);
        playerController.canMove = true;

    }

    IEnumerator ExploitRoutine()
    {
        exploit = true;
        yield return new WaitForSeconds(exploitTime);
        exploit = false;
    }




}
