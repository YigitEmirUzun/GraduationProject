using UnityEngine;

public class SpecialAttack : ScriptableObject
{
    public string specialName;
    [TextArea] public string description;
    public float rarityWeigth;
    public string rarity;
    public Color rarityColor;
    public Sprite icon;

    public float damage;
    public float knockback;
    public virtual void Activate()
    {

    }
}
