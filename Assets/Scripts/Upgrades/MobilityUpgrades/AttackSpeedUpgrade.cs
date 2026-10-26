using UnityEngine;

[CreateAssetMenu(fileName = "MobilityUpgrade", menuName = "Upgrades/Mobility/AttackSpeed")]
public class AttackSpeedUpgrade : Upgrade
{
    public float attackDelay;

    public override void Choose(PlayerComponents playerComponents)
    {
        playerComponents.playerAttack.currentAttackDelay -= attackDelay;
    }
}
