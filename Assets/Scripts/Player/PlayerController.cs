using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputSystem_Actions inputAction;

    public float speed;
    public float currentSpeed;
    Rigidbody rb;

    Vector2 moveInput;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        currentSpeed = speed;
        rb = GetComponent<Rigidbody>();
        inputAction = new InputSystem_Actions();
    }

    void Update()
    {
        moveInput = inputAction.Player.Move.ReadValue<Vector2>();
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
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }
}
