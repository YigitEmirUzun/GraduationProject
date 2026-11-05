
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    public int soul;

    public List<Ults> unlockedUlts = new List<Ults>();

    public List<SpecialAttack> unlockedSpecialAttacks = new List<SpecialAttack>();

    public Ults currentUlti;
    public SpecialAttack currentSpecialAttack;

    public HealthUIManager healthUIManager;

    private void Awake()
    {
        healthUIManager = FindAnyObjectByType<HealthUIManager>();
        healthUIManager.UpdateSoulsUI(soul);
    }

    public void AddSoul(int count)
    {
        soul += count;
        healthUIManager.UpdateSoulsUI(soul);
    }

    public void ConsumeSoul(int count)
    {
        soul -= count;
        healthUIManager.UpdateSoulsUI(soul);
    }

    public void UnlockUlti(Ults ulti)
    {
        unlockedUlts.Add(ulti);
    }

    public void UnlockSpecial(SpecialAttack special)
    {
        unlockedSpecialAttacks.Add(special);
    }

    public void UseUlti()
    {
        Debug.Log("Ulti Kullanýldý.");
    }

    public void UseSpecial()
    {
        Debug.Log("Özel Saldýrý Kullanýldý.");
    }




}
