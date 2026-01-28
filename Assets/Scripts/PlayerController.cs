using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 5f;
    public Transform playerCamera;

    [Header("Flashlights")]
    public Light primaryFlashlight;    // Default flashlight
    public Light secondaryFlashlight;  // Flashlight while holding F
    public Key switchKey = Key.F;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SetupFlashlight(primaryFlashlight, true);
        SetupFlashlight(secondaryFlashlight, false);
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleFlashlights();
    }

    void SetupFlashlight(Light light, bool enabled)
    {
        if (light == null) return;

        light.transform.SetParent(playerCamera);
        light.transform.localPosition = Vector3.zero;
        light.transform.localRotation = Quaternion.identity;
        light.enabled = enabled;
    }

    void HandleFlashlights()
    {
        bool holdingRightClick = Mouse.current.rightButton.isPressed;

        if (primaryFlashlight != null)
            primaryFlashlight.enabled = !holdingRightClick;

        if (secondaryFlashlight != null)
            secondaryFlashlight.enabled = holdingRightClick;
    }

    void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
