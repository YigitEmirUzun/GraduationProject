using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float maxMana = 100;
    public float currentHealth;
    public float currentMana;

    private void Awake()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    public void UpgradeMaxHealth(float upgradeAmount)
    {
        // maxHealth ý arttýr.
        maxHealth += upgradeAmount;
    }

    public void UpgradeMaxMana(float upgradeAmount)
    {
        // maxManayý arttýr.
        maxMana += upgradeAmount;
    }

    public void Heal(float healAmount)
    {
        // Can maxHealth dan fazla olursa maxHealth'a eþitle.
        currentHealth = Mathf.Min(currentHealth + healAmount,maxHealth);
    }

    public void RestoreMana(float manaAmount)
    {
        // Mana maxMana dan fazla olursa maxMana'ya eþitle.
        currentMana = Mathf.Min(currentMana + manaAmount,maxMana);
    }

    public void TakeDamage(float damageAmount)
    {
        // Hasar al. Can 0 ýn altýna inerse 0 a eþitle
        currentHealth = Mathf.Max(0,currentHealth - damageAmount);
    }

    public void ConsumeMana(float manaAmount)
    {
        // Mana eksilt. Mana 0 ýn altýna inerse 0 a eþitle.
        currentMana = Mathf.Max(0, currentMana - manaAmount);
    }
}
