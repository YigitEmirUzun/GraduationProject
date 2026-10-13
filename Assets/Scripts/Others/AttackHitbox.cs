using System.Collections;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private float damage;
    private float knockback;

    public float lifeTime;

    private void Awake()
    {
        StartCoroutine(DestroyRoutine());
    }

    public void Initialize(float damage, float knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage, knockback);
            Debug.Log("Vurdun!");
        }
    }

    public IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
