using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlayerSpawn : MonoBehaviour
{
    private GameObject playerSpawnPoint;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.Find("Player");
        if (player == null) { Debug.Log("oyuncu bulunamadý!"); }

        playerSpawnPoint = GameObject.Find(SceneTransition.spawnPoint);
        if (playerSpawnPoint == null) { Debug.Log("spawn noktasý bulunamadý!"); }

        Debug.Log(SceneTransition.spawnPoint);

        player.GetComponent<Rigidbody>().position = playerSpawnPoint.transform.position;
    }

}