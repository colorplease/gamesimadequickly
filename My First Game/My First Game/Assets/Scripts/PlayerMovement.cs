using System;
using UnityEngine;
using UnityEngine.InputSystem;
[System.Serializable]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]AudioSource audioSource;
    [Header("Input")]
    PlayerInput playerInput = null;
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float movementMultiplier = 10f;
    [SerializeField] float airMultiplier = 0.4f;
    [SerializeField] Transform orientation;
    [SerializeField] Transform cam;
    [SerializeField] float Frequency;
    [SerializeField] float Smooth;
    [SerializeField] float Amount;
    Vector3 startPos;
    float adjustedVerticalMovement;
    public bool commandeerInput;

    [Header("Footsteps")]
    //check ground type to play different sound for footsteps
    public bool footstepSoundEnabled = false;
    string currentGround;
    float Sin;
    [SerializeField] AudioClip[] woodFootsteps;
    [SerializeField] AudioClip[] grassFootsteps;
    [SerializeField] AudioClip[] tileFootsteps;
    bool isTriggered;

    [Header("Jumping")]
    public float playerHeight = 2;
    public float jumpForce = 15f;
    float groundDistance = 0.4f;
    [SerializeField] LayerMask groundMask;

    [Header("Drag")]
    [SerializeField]float groundDrag = 6f;
    [SerializeField]float airDrag = 2f;

    public float horizontalMovement;
    public float verticalMovement;

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    public Rigidbody rb;

    bool isGrounded;

    RaycastHit slopeHit;

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.5f))
        {
            currentGround = slopeHit.transform.tag;
            if(slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Application.targetFrameRate = 240;
    }

    void Awake()
    {
        playerInput = new PlayerInput();
    }

    void OnEnable()
    {
        playerInput.Enable();
        playerInput.Movement.Walking.performed += OnWalkingPerformed;
        playerInput.Movement.Walking.canceled += OnWalkingCanceled;
        playerInput.Movement.Jumping.performed += OnJumpingPerformed;
    }

    void OnWalkingPerformed(InputAction.CallbackContext context)
    {
        adjustedVerticalMovement = context.ReadValue<Vector2>().y;
    }

    void OnWalkingCanceled(InputAction.CallbackContext context)
    {
        adjustedVerticalMovement = 0;
        moveDirection = Vector3.zero;
    }

    void OnJumpingPerformed(InputAction.CallbackContext context)
    {
        if(isGrounded)
        {
            Jump();
        }
    }
    private void Update()
    {
        // print((orientation.rotation.y));
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, 1, 0), groundDistance, groundMask);
        ControlDrag();
        CheckForHeadbobTrigger();
        StopHeadbob();

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

         //prevents the player from moving backwards by pressing W and S
        if(Mathf.Abs(orientation.rotation.y) > 0.45f)
        {
            if(adjustedVerticalMovement > 0)
            {
                adjustedVerticalMovement = 0;
            }
        }
        else if(Mathf.Abs(orientation.rotation.y) < 0.45f)
        {
            if(adjustedVerticalMovement < 0)
            {
                adjustedVerticalMovement = 0;
            }
        }
        verticalMovement = adjustedVerticalMovement;
    }

    void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = airDrag;
        }
    }

    private void FixedUpdate()
    {
        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed *movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }

    void CheckForHeadbobTrigger()
    {
        float inputMagnitude = new Vector3(horizontalMovement, 0, verticalMovement).magnitude;

        if(inputMagnitude > 0 && isGrounded)
        {
            StartHeadbob();
        }
    }

    Vector3 StartHeadbob()
    {
        //matches frequency and plays a footstep every step
        Sin = Mathf.Sin(Time.time * Frequency);
        if(Sin > 0.97f && isTriggered == false)
        {
            isTriggered = true;
            PlayFootStep();
        }
        else if (isTriggered == true && Sin < -0.97f)
        {
            isTriggered = false;
        }

            //headbob
            Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * Frequency) * Amount * 1.4f, Smooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * Frequency / 2f) * Amount * 1.6f, Smooth * Time.deltaTime);
        cam.localPosition += pos;
        return pos;
    }

    void PlayFootStep()
    {
        if(footstepSoundEnabled)
        {
            var randomPitch = UnityEngine.Random.Range(0.85f, 1.25f);
            AudioClip step = null;
            switch(currentGround)
            {
                case "wood":
                    step = woodFootsteps[UnityEngine.Random.Range(0, woodFootsteps.Length - 1)];
                    break;
                case "grass":
                    step = grassFootsteps[UnityEngine.Random.Range(0, grassFootsteps.Length - 1)];
                    break;
                case "tile":
                    step = tileFootsteps[UnityEngine.Random.Range(0, grassFootsteps.Length - 1)];
                    break;
            }
            audioSource.pitch = randomPitch;
            audioSource.PlayOneShot(step);
        }
    }

    void StopHeadbob()
    {
        if (cam.localPosition == startPos) return;
        cam.localPosition = Vector3.Lerp(cam.localPosition, startPos, 1 * Time.deltaTime);
    }
}
