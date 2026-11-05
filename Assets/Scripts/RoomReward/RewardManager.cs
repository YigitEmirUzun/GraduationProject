using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class RewardManager : MonoBehaviour
{
    public PlayerComponents playerComponents;
    public CameraFollow followCamera;
    
    public List<RoomRewards> allRewards = new List<RoomRewards>();

    public List<RoomRewards> currentRewardPool = new List<RoomRewards>();

    public Canvas rewardsCanvas;
    public Image fadeImage;

    [Header("First Reward UI")]
    public Button option1;
    public Image godImage1;
    public TextMeshProUGUI godName1;
    public TextMeshProUGUI description1;
    private Material godMat1;

    [Header("Second Reward UI")]
    public Button option2;
    public Image godImage2;
    public TextMeshProUGUI godName2;
    public TextMeshProUGUI description2;
    private Material godMat2;

    [Header("Upgrade UI")]
    public Canvas upgradeCanvas;
    public Button upgradeButton1;
    public Button upgradeButton2;
    public Button upgradeButton3;


    private void Awake()
    {
        fadeImage.color = new Color(0, 0, 0, 0);
        rewardsCanvas.enabled = false;

        godMat1 = Instantiate(godImage1.material);
        godImage1.material = godMat1;

        godMat2 = Instantiate(godImage2.material);
        godImage2.material = godMat2;
    }
    private void Start()
    {
        followCamera = FindAnyObjectByType<CameraFollow>();
        playerComponents = FindAnyObjectByType<PlayerComponents>();
        currentRewardPool = new List<RoomRewards>(allRewards);
    }

    public void SetRewards()
    {
        currentRewardPool = new List<RoomRewards>(allRewards);

        RoomRewards reward1 = GetRandomReward();
        RoomRewards reward2 = GetRandomReward();

        SetRewardsUI(reward1,reward2);

        option1.onClick.AddListener(() =>
        {
            reward1.Choose(this);
        });

        option2.onClick.AddListener(() =>
        {
            reward2.Choose(this);
        });
    }

    public void ShowRewards()
    {
        SetRewards();
        fadeImage.DOFade(0.95f, 0.2f).OnComplete(() => { Time.timeScale = 0.001f; });
        rewardsCanvas.enabled = true;
    }

    public void ShowUpgradeMenu()
    {
        rewardsCanvas.enabled = false;
        upgradeCanvas.enabled = true;
    }

    // Can Yenileme odulu
    public void HealPlayer()
    {
        Time.timeScale = 1f;
        fadeImage.DOFade(0f, 0.2f);
        playerComponents.playerHealth.Heal(playerComponents.playerHealth.maxHealth * 0.2f);
        rewardsCanvas.enabled = false;
        EnablePlayerComponents();
    }

    // Mana Yenileme odulu
    public void RestorePlayerMana()
    {
        Time.timeScale = 1f;
        fadeImage.DOFade(0f, 0.2f);
        playerComponents.playerHealth.RestoreMana(playerComponents.playerHealth.maxMana * 0.5f);
        rewardsCanvas.enabled = false;
        EnablePlayerComponents();
    }

    public void AddSoul(int soul)
    {
        Time.timeScale = 1f;
        fadeImage.DOFade(0f, 0.2f);
        playerComponents.playerResources.AddSoul(soul);
        rewardsCanvas.enabled = false;
        EnablePlayerComponents();
    }

    // Secilen odullerin UI ayarlamasi.
    public void SetRewardsUI(RoomRewards reward1, RoomRewards reward2)
    {
        godImage1.transform.localScale = Vector3.zero;
        godImage2.transform.localScale = Vector3.zero;
        godImage1.transform.localPosition = new Vector3(godImage1.transform.localPosition.x, 200, 0);
        godImage2.transform.localPosition = new Vector3(godImage2.transform.localPosition.x, -200, 0);

        godImage1.sprite = reward1.godImage;
        godName1.text = reward1.godName;
        description1.text = reward1.description;

        godImage2.sprite = reward2.godImage;
        godName2.text = reward2.godName;
        description2.text = reward2.description;

        // Tanri Image lerinin gorsel animasyonu.
        
        godMat1.SetFloat("_FlashAmount", 1f);

        godImage1.transform.DOLocalMoveY(0,0.1f).SetEase(Ease.InQuint).SetUpdate(true);
        godImage1.transform.DOScale(new Vector3(-8f,8f,8f),0.1f).SetEase(Ease.Flash).SetUpdate(true).OnComplete(() => 
        {
            followCamera.ShakeCamera(0.15f,0.05f,3f,4);
            godMat1.DOFloat(0f, "_FlashAmount", 0.5f).SetEase(Ease.OutQuad).SetUpdate(true);
            godImage1.transform.DOPunchScale(new Vector3(-8.2f,8.2f,8.2f), 0.08f).SetUpdate(true);
        });

        godMat2.SetFloat("_FlashAmount", 1f);
        
        godImage2.transform.DOLocalMoveY(0, 0.1f).SetEase(Ease.InQuint).SetUpdate(true);    
        godImage2.transform.DOScale(new Vector3(8, 8, 8), 0.1f).SetEase(Ease.Flash).SetUpdate(true).OnComplete(() => 
        {
            godMat2.DOFloat(0f, "_FlashAmount", 0.2f).SetEase(Ease.OutQuad).SetUpdate(true);
            godImage2.transform.DOPunchScale(new Vector3(8.2f, 8.2f, 8.2f), 0.08f).SetUpdate(true);
        });
    }

    // Random odul secimi
    public RoomRewards GetRandomReward()
    {
        float totalWeight = 0f;

        foreach (RoomRewards reward in currentRewardPool)
        {
            totalWeight += reward.rarityWeight;
        }

        float randomValue = Random.Range(0f, totalWeight);

        foreach (RoomRewards reward in currentRewardPool)
        {
            randomValue -= reward.rarityWeight;

            if (randomValue <= 0f)
            {
                currentRewardPool.Remove(reward);
                return reward;
            }
        }

        return null;
    }

    public void DisablePlayerComponents()
    {
        playerComponents.playerController.canMove = false;
        playerComponents.playerAttack.canAttack = false;

    }

    public void EnablePlayerComponents()
    {
        playerComponents.playerController.canMove = true;
        playerComponents.playerAttack.canAttack = true;
    }
}
