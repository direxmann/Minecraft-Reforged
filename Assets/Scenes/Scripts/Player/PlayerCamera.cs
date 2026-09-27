using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float sensitivity = 0.25f;

    private InputSystem_Actions inputActions;

    private float verticalRotation;
    private bool cursorLocked;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        LockCursor();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
        UnlockCursor();
    }

    private void Update()
    {
        HandleCursor();
        Look();
    }

    private void HandleCursor()
    {
        if (inputActions.Player.Pause.WasPressedThisFrame())
        {
            if (cursorLocked)
            {
                UnlockCursor();
            }
            else
            {
                LockCursor();
            }
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cursorLocked = true;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cursorLocked = false;
    }

    private void Look()
    {
        if (!cursorLocked)
            return;

        Vector2 mouseInput = inputActions.Player.Look.ReadValue<Vector2>();

        float mouseX = mouseInput.x * sensitivity;
        float mouseY = mouseInput.y * sensitivity;

        transform.parent.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(
            verticalRotation,
            0f,
            0f
        );
    }
}