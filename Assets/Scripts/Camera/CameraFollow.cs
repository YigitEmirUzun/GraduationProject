using UnityEngine;

public class CameraFollow : MonoBehaviour
{


    public Transform targetObj;
    public Vector3 targetPos;

    public bool canFollow = true ;
    public float smoothSpeed = 5f;

    private void Awake()
    {
        targetObj = GameObject.Find("Player").transform;
    }
    private void Start()
    {
        if (canFollow) { transform.position = new Vector3(targetObj.position.x - 31, 20, targetObj.position.z - 31); }
    }

    private void Update()
    {
        targetPos = new Vector3(targetObj.position.x - 31, 20, targetObj.position.z - 31);
    }

    private void LateUpdate()
    {
        if (canFollow)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
        }

    }

    
}
