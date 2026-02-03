using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public static PlayerLook instance;
    [Header("Input")]
    PlayerInput playerInput = null;
    [SerializeField] float sensX;
    [SerializeField] float sensY;

    [SerializeField]Transform cam;
    [SerializeField]Transform orientation;

    public float mouseX;
    public float mouseY;

    float multiplier = 0.02f;

    public float xRotation;
    public float yRotation;

    public bool inControl;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        playerInput = new PlayerInput();
    }

    void OnEnable()
    {
        playerInput.Enable();
        playerInput.Movement.Looking.performed += OnLookingPerformed;
        playerInput.Movement.Looking.canceled += OnLookingCanceled;
    }

    void OnLookingPerformed(InputAction.CallbackContext context)
    {
        if (!inControl) return;
        mouseX = context.ReadValue<Vector2>().x;
        mouseY = context.ReadValue<Vector2>().y;
    }

    void OnLookingCanceled(InputAction.CallbackContext context)
    {
        mouseX = 0;
        mouseY = 0;
    }

    void Update()
    {
        if (!inControl) return;
        yRotation += mouseX * sensX * multiplier;
        xRotation -= mouseY * sensY * multiplier;

        xRotation = Mathf.Clamp(xRotation, -85, 85);

        cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
