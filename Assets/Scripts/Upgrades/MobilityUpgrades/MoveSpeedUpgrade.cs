using UnityEngine;

[CreateAssetMenu(fileName = "MobilityUpgrade", menuName = "Upgrades/Mobility/MoveSpeed")]
public class MoveSpeedUpgrade : Upgrade
{
    public float moveSpeed;

    public override void Choose(PlayerComponents playerComponents)
    {
        playerComponents.playerController.currentSpeed += moveSpeed;
    }
}
