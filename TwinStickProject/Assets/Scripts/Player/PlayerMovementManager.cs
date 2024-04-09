using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementManager : MonoBehaviour
{
    public static PlayerMovementManager instance;

    ControlsMap controlsMap;
    CharacterController controller;

    Vector2 movementDirection;
    public float movementSpeed = 5;

    private void OnEnable() => controlsMap.Gameplay.Enable();
    private void OnDisable() => controlsMap.Gameplay.Disable();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        controlsMap = new ControlsMap();

        controlsMap.Gameplay.Movement.performed += ctx => ReadMovementInput(ctx);
        controlsMap.Gameplay.Movement.canceled += ctx => movementDirection = Vector2.zero;

        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        controller.Move(new Vector3(movementDirection.x, 0, movementDirection.y) * movementSpeed * Time.deltaTime);
    }

    void ReadMovementInput(InputAction.CallbackContext ctx)
    {
        movementDirection = ctx.ReadValue<Vector2>();
    }
}
