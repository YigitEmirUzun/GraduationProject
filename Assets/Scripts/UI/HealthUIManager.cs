using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIManager : MonoBehaviour
{
    public Image healthImage;
    public Image healthDelayImage;
    public GameObject healthBar;

    public Image manaImage;

    public PlayerHealth playerHealth;


    private void Awake()
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        UpdateHealthUI(playerHealth.currentHealth, playerHealth.maxHealth, PlayerHealth.ChangeType.Increase);
        UpdateManaUI(playerHealth.currentMana, playerHealth.maxMana, PlayerHealth.ChangeType.Increase);
    }


    private void OnEnable()
    {
        playerHealth.OnHealthChange += UpdateHealthUI;
        playerHealth.OnManaChange += UpdateManaUI;
    }
    private void OnDisable()
    {
        playerHealth.OnHealthChange -= UpdateHealthUI;
        playerHealth.OnManaChange -= UpdateManaUI;
    }

    private void UpdateHealthUI(float currentHealth, float maxHealth, PlayerHealth.ChangeType changeType)
    {
        if(changeType == PlayerHealth.ChangeType.Decrease)
        {
            healthImage.DOFillAmount(currentHealth / maxHealth, 0.2f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                healthDelayImage.DOFillAmount(currentHealth / maxHealth, 0.3f).SetDelay(0.3f);
            });

            healthBar.transform.DOPunchScale(new Vector2(0.1f, 0.2f), 0.2f);
        }
        else
        {
            healthImage.DOFillAmount(currentHealth / maxHealth, 0.4f).SetEase(Ease.OutCubic);
            healthDelayImage.DOFillAmount(currentHealth / maxHealth, 0.4f).SetDelay(0.1f);
        }

        
    }

    private void UpdateManaUI(float currentMana, float maxMana, PlayerHealth.ChangeType changeType)
    {
        healthImage.DOFillAmount(currentMana / maxMana, 0.1f).SetEase(Ease.OutCubic);
    }

}
