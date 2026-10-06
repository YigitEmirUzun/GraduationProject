using UnityEngine;

public abstract class Potion : Item
{
    public float duration;

    public abstract void Use(PlayerComponents player);
}
