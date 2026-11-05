using UnityEngine;

public class Ults : ScriptableObject
{
    public string ultiName;
    [TextArea] public string description;
    public float rarityWeigth;
    public string rarity;
    public Color rarityColor;
    public Sprite icon;
    
    public float coolDown;
    public float damage;
    public float knockback;

    
    public virtual void Activate()
    {

    }

}
