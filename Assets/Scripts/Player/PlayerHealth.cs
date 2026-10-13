using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 100;
    public float maxMana = 100;
    public float currentHealth = 100;
    public float currentMana = 100;

    public Action<float, float, ChangeType> OnHealthChange;
    public Action<float, float, ChangeType> OnManaChange;

    public enum ChangeType
    {
        Increase,
        Decrease
    }

    private void Awake()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
    }


    // maxHealth ý arttýr.
    public void UpgradeMaxHealth(float upgradeAmount)
    {
        maxHealth += upgradeAmount;
        OnHealthChange?.Invoke(currentHealth, maxHealth, ChangeType.Increase);
    }

    // maxManayý arttýr.
    public void UpgradeMaxMana(float upgradeAmount)
    {  
        maxMana += upgradeAmount;
        OnManaChange?.Invoke(currentMana, maxMana, ChangeType.Increase);
    }

    // Can maxHealth dan fazla olursa maxHealth'a eþitle.
    public void Heal(float healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount,maxHealth);
        OnHealthChange?.Invoke(currentHealth, maxHealth, ChangeType.Increase);
    }

    // Mana maxMana dan fazla olursa maxMana'ya eþitle.
    public void RestoreMana(float manaAmount)
    {
        currentMana = Mathf.Min(currentMana + manaAmount,maxMana);
        OnManaChange?.Invoke(currentMana, maxMana, ChangeType.Increase);
    }

    // Hasar al. Can 0 ýn altýna inerse 0 a eþitle
    public void TakeDamage(float damageAmount, float knockback)
    {
        currentHealth = Mathf.Max(0,currentHealth - damageAmount);
        OnHealthChange?.Invoke(currentHealth, maxHealth, ChangeType.Decrease);
    }

    // Mana eksilt. Mana 0 ýn altýna inerse 0 a eþitle.
    public void ConsumeMana(float manaAmount)
    {
        currentMana = Mathf.Max(0, currentMana - manaAmount);
        OnManaChange?.Invoke(currentMana, maxMana, ChangeType.Decrease);
    }
}
