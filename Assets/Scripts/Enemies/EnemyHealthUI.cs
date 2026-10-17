using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{

    public Image healthImage;
    public Image healthDelayImage;
    public GameObject healthBG;
    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpgradeUI(float maxHealth, float currentHealth)
    {
        healthBG.transform.DOPunchScale(Vector3.one * 0.4f, 0.3f);
        healthImage.DOFillAmount(currentHealth / maxHealth, 0.2f).SetEase(Ease.OutBounce);
        healthDelayImage.DOFillAmount(currentHealth / maxHealth, 0.1f).SetEase(Ease.OutBack).SetDelay(0.4f);
        Debug.Log(healthImage.fillAmount);
    }
}
