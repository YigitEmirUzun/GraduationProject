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

    
    public bool canAttack;

    private void Awake()
    {
        currentDamage = baseDamage;
        currentKnockback = baseKnockback;
        currentCritChance = baseCritChance;
        currentCritMultiplier = baseCritMultiplier;
        currentAttackDelay = baseAttackDelay;
    }

    public float CalculatedDamage()
    {
        bool isCritical = Random.value < currentCritChance;

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
            float damageGiven = CalculatedDamage();
            GameObject hitbox = Instantiate(attackHitbox, spawnPos.position, Quaternion.Euler(0f, 45f, 0f));
            hitbox.GetComponent<AttackHitbox>().Initialize(damageGiven, currentKnockback);
        }
    }

    IEnumerator AttackSpeedRoutine()
    {
        canAttack = false;
        yield return new WaitForSeconds(currentAttackDelay);
        canAttack = true;
    }




}
