using UnityEngine;

[CreateAssetMenu(menuName = "RoomRewards/UpgradeReward")]
public class UpgradeReward : RoomRewards
{
    public override void Choose(RewardManager manager)
    {
        manager.ShowUpgradeMenu();
    }
}
