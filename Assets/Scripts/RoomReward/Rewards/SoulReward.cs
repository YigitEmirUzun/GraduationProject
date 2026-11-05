using UnityEngine;

[CreateAssetMenu(menuName = "RoomRewards/SoulReward")]
public class SoulReward : RoomRewards
{
    [Header("Special Variables")]
    public int soulCount;
    public override void Choose(RewardManager manager)
    {
        manager.AddSoul(soulCount);
    }
}
