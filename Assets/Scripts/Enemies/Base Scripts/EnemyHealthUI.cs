using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{

    public Image healthImage;
    public Image healthDelayImage;
    public GameObject healthBG;

    private Vector3 baseScale;
    private void Awake()
    {
        
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpgradeUI(float maxHealth, float currentHealth)
    {
        healthBG.transform.DOPunchScale(Vector3.one * 0.6f, 0.2f).OnComplete(() => { healthBG.transform.DOScale(Vector3.one, 0.1f); });
        healthImage.DOFillAmount(currentHealth / maxHealth, 0.3f).SetEase(Ease.OutCubic);
        healthDelayImage.DOFillAmount(currentHealth / maxHealth, 0.1f).SetEase(Ease.OutCubic).SetDelay(0.3f);
        Debug.Log(healthImage.fillAmount);
    }
}
