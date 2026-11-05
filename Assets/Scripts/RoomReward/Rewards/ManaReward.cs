using UnityEngine;

[CreateAssetMenu(menuName = "RoomRewards/ManaReward")]
public class ManaReward : RoomRewards
{
    public override void Choose(RewardManager manager)
    {
        manager.RestorePlayerMana();
    }
}
