using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class SwordFollow : MonoBehaviour
{
    public static SwordFollow instance { get; private set; }

    public Transform swordPoint;
    public Transform player;

    public GameObject swordObject;
    public SpriteRenderer swordSr;

    public Material swordMat;

    public float smoothTime = 0.15f;
    private Vector3 velocity;

    private float respawnTimer = 1;
    private bool swordActive = true;

    private bool canFollow = true;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        swordPoint = GameObject.Find("SwordPoint").GetComponent<Transform>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        swordSr = swordObject.GetComponent<SpriteRenderer>();

        swordMat = Instantiate(swordSr.material);
        swordSr.material = swordMat;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(respawnTimer > 0 && !swordActive)
        {
            respawnTimer -= Time.deltaTime;
        }

        if(respawnTimer <= 0 && !swordActive)
        {
            transform.position = swordPoint.position;
            swordActive = true;
            swordObject.SetActive(true);
            swordMat.DOFloat(0f, "_HitFlashAmount", 0.2f).SetEase(Ease.OutQuad).SetUpdate(true).OnComplete(() => { canFollow = true; });
        }
    }

    private void FixedUpdate()
    {
        if(canFollow)
        {
            transform.position = Vector3.SmoothDamp(
            transform.position,
            swordPoint.position,
            ref velocity,
            smoothTime
            );
        }
    }

    public void Attack()
    {
        respawnTimer = 1f;
        canFollow = false;
        transform.DOMove(player.position, 0.04f).SetEase(Ease.InQuint).SetUpdate(true).OnComplete(() =>
        {
            swordObject.SetActive(false);
            swordActive = false;
            swordMat.SetFloat("_HitFlashAmount", 1f);
        });
    }
}
