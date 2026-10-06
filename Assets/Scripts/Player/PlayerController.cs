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
        currentSpeed = speed;
        rb = GetComponent<Rigidbody>();
        inputAction = new InputSystem_Actions();
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // Hareket Sistemi   z eksenindeki hýza + 3 ekliyorum kamera açýsýndan dolayý.(Kameranýn açýsý yüzünden karakterin x ekseni hýzý daha fazla gibi gözüküyor)
        moveInput = inputAction.Player.Move.ReadValue<Vector2>();
        Vector3 direction = transform.right * moveInput.x * currentSpeed + transform.forward * moveInput.y * (currentSpeed + 3);
        rb.linearVelocity = direction;
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
