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
        transform.DOMoveY(transform.position.y + 1, 0.5f);
        randomX = Random.Range(-20, 20);
        transform.rotation = Quaternion.Euler(randomX, 0, 0);
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one,0.5f).SetEase(Ease.OutCubic).OnComplete(() => { textMeshPro.DOFade(0, 1); });
        
        Destroy(gameObject,duration);
        
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
