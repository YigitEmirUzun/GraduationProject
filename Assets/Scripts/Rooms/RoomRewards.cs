using UnityEngine;
using UnityEngine.UI;

public class RoomRewards : ScriptableObject
{
    public string rewardName;
    [TextArea]public string description;
    public string godName;

    public float rarityWeight;
    public Sprite godImage;
    

    public virtual void Choose(RewardManager manager)
    {

    }
}
