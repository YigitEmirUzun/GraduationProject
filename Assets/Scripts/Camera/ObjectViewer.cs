using UnityEngine;

public class ObjectViewer : MonoBehaviour
{
    [Header("Ortalanacak obje")]
    public Transform targetObject;
    
    private CameraFollow cameraFollow;
    private Transform player;

    public float objTransformSmooth = 0.2f;
    public float playerTransformSmooth = 0.5f;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cameraFollow.targetObj = targetObject;
            cameraFollow.smoothTime = objTransformSmooth;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cameraFollow.targetObj = player;
            cameraFollow.smoothTime = playerTransformSmooth;
        }
    }
}
