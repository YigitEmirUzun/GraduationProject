using UnityEngine;

public enum PotionType
{
    HealthPotion,
    ManaPotion
}
[CreateAssetMenu(fileName = "New Basic Potion", menuName = "Items/Potions/Basic Potion")]
public class BasicPotions : Potion
{
    public float amount;
    public PotionType type;

    public override void Use(PlayerComponents player)
    {
        switch(type)
        {
            case PotionType.HealthPotion:
                player.playerHealth.Heal(amount); 
                break;

            case PotionType.ManaPotion:
                player.playerHealth.RestoreMana(amount); 
                break;
        }
    }
}
