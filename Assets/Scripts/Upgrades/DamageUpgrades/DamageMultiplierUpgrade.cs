using UnityEngine;

[CreateAssetMenu(fileName = "DamageUpgrade", menuName = "Upgrades/Damage/Multiplier")]
public class DamageMultiplierUpgrade : Upgrade
{
    public float damageMult = 1.2f;

    public override void Choose(PlayerComponents playerComponents)
    {
        playerComponents.playerAttack.currentDamage *= damageMult;
    }
}
