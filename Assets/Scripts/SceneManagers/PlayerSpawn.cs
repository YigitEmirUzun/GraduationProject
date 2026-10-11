using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerSpawn : MonoBehaviour
{
    
    void Start()
    {
        GameObject player = GameObject.Find("Player");

        GameObject spawnPoint = GameObject.Find(SceneTransition.spawnPoint);

        player.transform.position = spawnPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
