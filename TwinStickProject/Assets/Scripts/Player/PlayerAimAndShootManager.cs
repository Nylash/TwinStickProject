using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerAimAndShootManager : MonoBehaviour
{
    public static PlayerAimAndShootManager instance;

    ControlsMap controlsMap;
    Animator animator;
    PlayerInput playerInput;

    private Vector2 stickDirection;
    private Vector2 mouseDirection;
    public Vector2 aimDirection;
    public float rotationSpeed = .075f;

    private void OnEnable() => controlsMap.Gameplay.Enable();
    private void OnDisable() => controlsMap.Gameplay.Disable();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        controlsMap = new ControlsMap();

        controlsMap.Gameplay.AimStick.performed += ctx => stickDirection = ctx.ReadValue<Vector2>();
        controlsMap.Gameplay.AimMouse.performed += ctx => mouseDirection = ctx.ReadValue<Vector2>();

        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (playerInput.currentControlScheme == "Keyboard")
        {
            Ray ray = Camera.main.ScreenPointToRay(mouseDirection);
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                Vector3 target = (ray.GetPoint(distance) - transform.position).normalized;
                aimDirection = new Vector2(target.x, target.z);
            }
        }
        else
        {
            aimDirection = stickDirection;
        }

        animator.SetFloat("InputX", Mathf.MoveTowards(animator.GetFloat("InputX"), aimDirection.x,rotationSpeed));
        animator.SetFloat("InputY", Mathf.MoveTowards(animator.GetFloat("InputY"), aimDirection.y, rotationSpeed));
    }
}
