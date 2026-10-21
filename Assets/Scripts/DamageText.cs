using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public string text;
    public TextMeshProUGUI textMeshPro;
    private float randomX;
    public float duration;

    private void Awake()
    {
        randomX = Random.Range(-20, 20);
        transform.rotation = Quaternion.Euler(randomX, 0, 0);
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one,0.5f).SetEase(Ease.OutBounce);
        textMeshPro.DOFade(0, duration);
        Destroy(gameObject,duration);
        transform.DOMoveY(transform.position.y + 1, duration);
    }

    private void Start()
    {
        textMeshPro.text = text;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
