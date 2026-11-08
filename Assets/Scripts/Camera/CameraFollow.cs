using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public bool hasCameraShaken = false;
    public bool hasSlowTime = false;
    public Transform targetObj;
    public Vector3 targetPos;
    public Camera mainCamera;

    public bool canFollow = true ;
    public float smoothTime = 0.15f;

    private Vector3 velocity;

    public Vector3 offset = new Vector3(-31, 17, -31);

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        targetObj = GameObject.Find("Player").transform;    
    }
    private void Start()
    {
        if (canFollow) { transform.position = targetObj.GetComponent<Rigidbody>().position + offset; }
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

    public void ShakeCamera(float orthoSize = 0.15f, float punchDuration = 0.05f, float shakeStrength = 0.5f, int vibration = 2)
    {
        if(!hasCameraShaken)
        {
            hasCameraShaken=true;
            mainCamera.DOOrthoSize(mainCamera.orthographicSize - orthoSize, punchDuration).SetEase(Ease.OutBounce).SetLoops(2, LoopType.Yoyo).SetUpdate(true);
            mainCamera.DOShakePosition(0.08f, shakeStrength, vibration, 10).SetUpdate(true).OnComplete(() => { hasCameraShaken = false; });
        }
    }

    public void SlowTime()
    {
        if(!hasSlowTime)
        {
            StartCoroutine(SlowTimeRoutine());
        }
    }

    public IEnumerator SlowTimeRoutine()
    {
        hasSlowTime = true;
        yield return new WaitForSeconds(0.008f);
        Time.timeScale = 0.001f;
        yield return new WaitForSeconds(0.0001f);
        Time.timeScale = 1f;
        hasSlowTime = false;
    }


}
