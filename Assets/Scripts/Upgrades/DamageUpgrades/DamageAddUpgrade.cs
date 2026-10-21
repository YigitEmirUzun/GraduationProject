using UnityEngine;

[CreateAssetMenu(fileName = "DamageUpgrade", menuName = "Upgrades/Damage/Add")]
public class DamageAddUpgrade : Upgrade
{
    public float addDamage;

    public override void Choose(PlayerComponents playerComponents)
    {
        playerComponents.playerAttack.currentDamage += addDamage;
    }
}
