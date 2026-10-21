using UnityEngine;

public abstract class Upgrade : ScriptableObject
{
    public float maxStack;
    public string upgradeName;
    [TextArea] public string description;
    public string type;
    public float rarityWeigth;
    public string rarity;
    public Color rarityColor;

    public Sprite icon;

    public abstract void Choose(PlayerComponents playerComponents);

    public virtual void Activate(PlayerComponents playerComponents) { }
}
