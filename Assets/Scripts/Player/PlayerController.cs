using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputSystem_Actions inputAction;

    public static PlayerController instance { get; private set; }

    private PlayerAttack playerAttack;
    public GameObject sprite;

    public float speed;
    public float currentSpeed;
    Rigidbody rb;

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

        //Componentslarý atama.
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
        rb.linearVelocity = new Vector3(direction.x,rb.linearVelocity.y, direction.z);
    }

    private void OnEnable()
    {
        inputAction.Enable();

        inputAction.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        inputAction.Player.Attack.performed -= OnAttack;

        inputAction.Disable();      
    }

    private void XDirection()
    {
        if(moveInput.x > 0)
        {
            sprite.transform.localScale = new Vector3(5, 12, 5);
        }
        else if(moveInput.x < 0) { sprite.transform.localScale = new Vector3(-5, 12, 5); }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        playerAttack.Attack();
    }
}
