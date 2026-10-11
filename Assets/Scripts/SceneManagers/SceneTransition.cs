using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public static string spawnPoint;

    public string sceneToLoad;
    public string nextSceneSpawnPoint;

    public Image fadeUI;
    void Start()
    {
        fadeUI = GameObject.Find("FadeUI").GetComponent<Image>();
    }
    public void LoadScene()
    {
        spawnPoint = nextSceneSpawnPoint;

        fadeUI.DOFade(1f, 0.4f).OnComplete(() => {SceneManager.LoadScene(sceneToLoad);});

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LoadScene();
        }
    }


}
