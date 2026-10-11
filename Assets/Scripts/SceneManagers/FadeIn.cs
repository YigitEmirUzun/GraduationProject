using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour
{
    private Image fadeUI;
    void Start()
    {
        fadeUI = GetComponent<Image>();
        StartCoroutine(fadeRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator fadeRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        fadeUI.DOFade(0f, 3f);
    }
}
