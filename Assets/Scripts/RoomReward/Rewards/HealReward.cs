using UnityEngine;

[CreateAssetMenu(menuName = "RoomRewards/HealReward")]
public class HealReward : RoomRewards
{
    public override void Choose(RewardManager manager)
    {
        manager.HealPlayer();
    }
}
