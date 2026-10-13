using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetObj;
    public Vector3 targetPos;

    public bool canFollow = true ;
    public float smoothTime = 0.5f;

    private Vector3 velocity;

    public Vector3 offset = new Vector3(-31, 17, -31);

    private void Awake()
    {
        targetObj = GameObject.Find("Player").transform;
    }
    private void Start()
    {
        if (canFollow) { transform.position = targetObj.position + offset; }
    }

    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        targetPos = targetObj.position + offset;

        if (canFollow)
        {
            transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            smoothTime
            );
        }

    }

    
}
