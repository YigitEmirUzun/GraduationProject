using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour , IInteractable
{
    public static string spawnPoint;

    public Image fadeUI;


    [Header("Geçilecek Sahne Adý(String)")] 
    public string sceneToLoad;

    [Header("Geçilen Sahnedeki Spawn Point Adý(String)")]
    public string nextSceneSpawnPoint;
   
    void Awake()
    {
        fadeUI = GameObject.Find("FadeUI").GetComponent<Image>();
    }

    public void Interact()
    {
        spawnPoint = nextSceneSpawnPoint;
        LoadScene();
    }
    public void LoadScene()
    {
        

        fadeUI.DOFade(1f, 0.4f).OnComplete(() => {SceneManager.LoadScene(sceneToLoad);});
    }
}
