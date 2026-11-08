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
            mainCamera.ShakeCamera();
            mainCamera.SlowTime();
        }
    }

    public IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(lifeTime);
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

    
}
