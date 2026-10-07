using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(player.position.x + -30f, 18f , player.transform.position.z + -30f);
    }

    
}
