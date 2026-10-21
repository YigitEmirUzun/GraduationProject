using UnityEngine;

[CreateAssetMenu(fileName = "DamageUpgrade", menuName = "Upgrades/Damage/CritChance")]
public class CritChanceUpgrade : Upgrade
{
    public float critChance;

    public override void Choose(PlayerComponents playerComponents)
    {
        playerComponents.playerAttack.currentCritChance += critChance;
    }
}
