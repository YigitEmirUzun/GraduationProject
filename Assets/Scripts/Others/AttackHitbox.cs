using System.Collections;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private float damage;
    private float knockback;

    public float lifeTime;

    public CameraFollow mainCamera;

    private void Awake()
    {
        StartCoroutine(DestroyRoutine());
        mainCamera = GameObject.FindAnyObjectByType<CameraFollow>();
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
            StartCoroutine(SlowTimeRoutine());
            Debug.Log("Vurdun!");
        }
    }

    public IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(lifeTime);
        Time.timeScale = 1.0f;
        Destroy(gameObject);
    }

    public IEnumerator SlowTimeRoutine()
    {
        yield return new WaitForSeconds(0.005f);
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(0.003f);
        Time.timeScale = 1f;
        mainCamera.ShakeCamera();
    }
}
