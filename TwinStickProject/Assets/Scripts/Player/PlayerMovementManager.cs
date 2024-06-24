using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementManager : Singleton<PlayerMovementManager>
{
    ControlsMap controlsMap;
    CharacterController controller;

    Vector2 movementDirection;
    [SerializeField] private float movementSpeed = 5;

    private void OnEnable() => controlsMap.Gameplay.Enable();
    private void OnDisable() => controlsMap.Gameplay.Disable();

    private void Awake()
    {
        controlsMap = new ControlsMap();

        controlsMap.Gameplay.Movement.performed += ctx => movementDirection = ctx.ReadValue<Vector2>();
        controlsMap.Gameplay.Movement.canceled += ctx => movementDirection = Vector2.zero;

        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        controller.Move(new Vector3(movementDirection.x, 0, movementDirection.y) * movementSpeed * Time.deltaTime);
    }
}
