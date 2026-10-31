using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    InputSystem_Actions inputAction;

    public static PlayerController instance { get; private set; }

    private PlayerAttack playerAttack;
    public GameObject sprite;

    public bool canMove = true;

    public float speed;
    public float currentSpeed;
    public Rigidbody rb;

    Vector2 moveInput;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        currentSpeed = speed;
        
        inputAction = new InputSystem_Actions();

        rb = GetComponent<Rigidbody>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        moveInput = inputAction.Player.Move.ReadValue<Vector2>();
        XDirection();
    }

    private void FixedUpdate()
    {
        // Hareket Sistemi   z eksenindeki hýza + 3 ekliyorum kamera açýsýndan dolayý.(Kameranýn açýsý yüzünden karakterin x ekseni hýzý daha fazla gibi gözüküyor)
        
        Vector3 direction = transform.right * moveInput.x * currentSpeed + transform.forward * moveInput.y * (currentSpeed + 3);
        if (canMove)
        {
            rb.linearVelocity = new Vector3(direction.x, rb.linearVelocity.y, direction.z);
        }
        else { rb.linearVelocity = Vector3.zero; }
    }

    private void OnEnable()
    {
        inputAction.Enable();

        inputAction.Player.Attack.performed += OnAttack;
        inputAction.Player.Exploit.performed += OnExploit;
    }

    private void OnDisable()
    {
        inputAction.Player.Attack.performed -= OnAttack;
        inputAction.Player.Exploit.performed -= OnExploit;

        inputAction.Disable();      
    }

    private void XDirection()
    {
        if(moveInput.x > 0.5)
        {
            sprite.transform.localScale = new Vector3(1, 1, 1);
        }
        else if(moveInput.x < -0.5) { sprite.transform.localScale = new Vector3(-1, 1, 1); }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        playerAttack.Attack();
    }

    private void OnExploit(InputAction.CallbackContext context)
    {
        playerAttack.Exploit();
    }
}
