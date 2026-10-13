using DG.Tweening;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public GameObject interactSprite;

    private void Awake()
    {
        interactSprite = transform.Find("Canvas/InteractButton").gameObject;
        interactSprite.SetActive(false);
        interactSprite.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactSprite.SetActive(true);
            interactSprite.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic, 0.2f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactSprite.transform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.InBack).OnComplete(() => {
                interactSprite.SetActive(false);
            });
        }
    }
}
