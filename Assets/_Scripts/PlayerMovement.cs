using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputReader inputReader;

    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = 9.8f;
    public float mouseSensitivity = 2f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float verticalRotation = 0f;

    [SerializeField] private Transform cameraTransform;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        inputReader.OnPlayerMoved += PlayerMoved;
        inputReader.OnPlayerViewChange += PlayerViewChanged;
        inputReader.OnPlayerJumped += PlayerJumped;

        cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDestroy()
    {
        inputReader.OnPlayerMoved -= PlayerMoved;
        inputReader.OnPlayerViewChange -= PlayerViewChanged;
        inputReader.OnPlayerJumped -= PlayerJumped;
    }

    void Update()
    {
        if (GameManager.Instance.currentGameState == GameState.Playing)
        {
            HandleMovement();
            HandleMouseLook();
        }
    }

    private void PlayerMoved(Vector2 value)
    {
        moveInput = value;
    }

    private void PlayerViewChanged(Vector2 value)
    {
        lookInput = value;
    }

    private void PlayerJumped()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
