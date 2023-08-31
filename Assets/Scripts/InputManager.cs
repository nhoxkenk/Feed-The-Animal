using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls controller;
    AnimatorManager animatorManager;

    public Vector2 movementInput;
    private float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
    }

    private void OnEnable()
    {
        if(controller == null)
        {
            controller = new PlayerControls();

            controller.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        }

        controller.Enable();
    }

    private void OnDisable()
    {
        controller.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        //etc more input
    }

    public void onDeath()
    {
        verticalInput = 0;
        horizontalInput = 0;
        animatorManager.UpdateAnimatorValues(0, 0);
        animatorManager.setDeathAnimation();
    }

    void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount);
    }
}
