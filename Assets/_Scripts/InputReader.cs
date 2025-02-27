using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Input Reader", menuName = "MoDev/Helpers/New Input Reader")]
public class InputReader : ScriptableObject
{
    UserInput userInput;

    public Action<Vector2> OnPlayerMoved;
    public Action<Vector2> OnPlayerViewChange;
    public Action OnPlayerJumped;

    private void OnEnable()
    {
        userInput = new UserInput();

        userInput.Gameplay.Enable();
        userInput.Gameplay.Move.performed += MoveInput;
        userInput.Gameplay.Move.canceled += MoveInputCancelled;
        userInput.Gameplay.Look.performed += LookInput;
        userInput.Gameplay.Look.canceled += LookInputancelled;
        userInput.Gameplay.Jump.performed += JumpInput;
    }

    private void OnDisable()
    {
        userInput.Gameplay.Disable();
        userInput.Gameplay.Move.performed -= MoveInput;
        userInput.Gameplay.Move.canceled -= MoveInputCancelled;
        userInput.Gameplay.Look.performed -= LookInput;
        userInput.Gameplay.Look.canceled -= LookInputancelled;
        userInput.Gameplay.Jump.performed -= JumpInput;
    }

    private void MoveInput(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        OnPlayerMoved?.Invoke(value);
    }

    private void MoveInputCancelled(InputAction.CallbackContext context)
    {
        OnPlayerMoved?.Invoke(Vector2.zero);
    }

    private void LookInput(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        OnPlayerViewChange?.Invoke(value);
    }

    private void LookInputancelled(InputAction.CallbackContext context)
    {
        OnPlayerViewChange?.Invoke(Vector2.zero);
    }

    private void JumpInput(InputAction.CallbackContext context)
    {
        OnPlayerJumped?.Invoke();
    }
}
