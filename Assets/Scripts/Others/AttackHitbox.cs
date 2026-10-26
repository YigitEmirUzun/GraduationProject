using System.Collections;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private float damage;
    private float knockback;
    private bool isCritical = false;

    public float lifeTime;

    public CameraFollow mainCamera;

    private void Awake()
    {
        StartCoroutine(DestroyRoutine());
        mainCamera = GameObject.FindAnyObjectByType<CameraFollow>();
    }

    public void Initialize(float damage, float knockback, bool isCritical)
    {
        this.damage = damage;
        this.knockback = knockback;
        this.isCritical = isCritical;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage, knockback,isCritical);
            StartCoroutine(SlowTimeRoutine());
            Debug.Log("Vurdun!");
        }
    }

    public IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(lifeTime);
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

    public IEnumerator SlowTimeRoutine()
    {
        yield return new WaitForSeconds(0.008f);
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.01f);
        Time.timeScale = 1f;
        mainCamera.ShakeCamera();
    }
}
