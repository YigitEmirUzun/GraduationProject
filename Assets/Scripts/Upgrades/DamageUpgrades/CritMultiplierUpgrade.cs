using UnityEngine;

[CreateAssetMenu(fileName = "DamageUpgrade", menuName = "Upgrades/Damage/CritMultiplier")]
public class CritMultiplierUpgrade : Upgrade
{
    public float critMultAdd;

    public override void Choose(PlayerComponents playerComponents)
    {
        playerComponents.playerAttack.currentCritMultiplier += critMultAdd;
    }
}
