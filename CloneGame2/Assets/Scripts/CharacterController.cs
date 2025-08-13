using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class CharacterControls : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private int moveSpeed;
    [SerializeField]
    private int JumpHeight;
    private Controls controls;
    private Vector2 lookInput;
    public Transform playerCamera;
    public Vector2 moveInput;
    private CharacterController characterController;
    private Vector3 velocity;
    private float gravity = -9.8f;
    [SerializeField] private float groundedGravity = -0.5f;
    [SerializeField] private float terminalVelocity = -50f;
    [SerializeField]
    private float lookSpeed;
    private float verticalLookRotation = 0f;


    [Header("Climbing Settings")]
    [SerializeField]
    private bool isClimbing;
    [SerializeField]
    private bool CanClimb;
    public int ClimbDistance;
    public Transform ClimbChecker2;
    public int ClimbSpeed;
    public Transform Crosshair;
    private StatManager statManagerScript;



    private void OnEnable()
    {
        controls = new Controls();
        controls.Player.Enable();
        controls.Player.LookAround.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.LookAround.canceled += ctx => lookInput = Vector2.zero;

        controls.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); // Update moveInput when movement input is performed
        controls.Player.Movement.canceled += ctx => moveInput = Vector2.zero; // Reset moveInput when movement input is canceled

        controls.Player.Jump.performed += ctx => Jump();
        controls.Player.Climb.performed += ctx => Climb();
        controls.Player.Climb.canceled += ctx => CancelClimb();


    }


    private void Start()
    {
        statManagerScript = GetComponent<StatManager>();
    }
    private void Update()
    {
        if (isClimbing && !CanClimb)
        {
            ApplyGravity();
            statManagerScript.IsClimbing = false;
        }
        else if (!isClimbing && CanClimb)
        {
            ApplyGravity();
            statManagerScript.IsClimbing = false;
        }
        else if (!isClimbing && !CanClimb)
        {
            ApplyGravity();
            statManagerScript.IsClimbing = false;
        }
        else if (CanClimb && isClimbing)
        {
            if (statManagerScript.IsClimbingAndMoving == false)
            {
                statManagerScript.IsClimbing = true;
            }
            else
            {
                statManagerScript.IsClimbing = true;
            }
        }

        Move();
        LookAround();
        ClimbCheck();

        //Vector3 mousePosition = Input.mousePosition;
        //Crosshair.position = new Vector3(mousePosition.x,  mousePosition.y, Crosshair.position.z);



        if (moveInput.x == 0 || moveInput.y == 0)
        {
            statManagerScript.IsClimbingAndMoving = false;
        }
        if (moveInput.x ==1  || moveInput.y == 1)
        {
            if(isClimbing && CanClimb)
            {
                statManagerScript.IsClimbingAndMoving = true;

            }
        }

    }


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }


    public void ClimbCheck()
    {

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, ClimbDistance))
        {
            if (hit.collider != null)
            {
                isClimbing = true;
            }

        }
        else if (hit.collider == null)
        {
            isClimbing = false;

        }

        Ray ray2 = new Ray(ClimbChecker2.position, ClimbChecker2.forward);
        RaycastHit hit2;

        if (Physics.Raycast(ray2, out hit2, ClimbDistance))
        {
            if (hit2.collider != null)
            {
                isClimbing = true;
            }

        }
        else if (hit2.collider == null)
        {
            isClimbing = false;

        }

    }
    void Climb()
    {
        CanClimb = true;

    }

    void CancelClimb()
    {
        CanClimb = false;
    }
    void Interact()
    {
       
    }

    void Jump()
    {
        if (!isClimbing)
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 2))
            {
                velocity.y = Mathf.Sqrt(JumpHeight * -2f * gravity);
            }
        }
        else if (isClimbing && CanClimb)
        {
            StartCoroutine(ClimbJump());
        }
        else if (isClimbing && !CanClimb)
        {
            StartCoroutine(ClimbJump());
        }
    }

    IEnumerator ClimbJump()
    {
        ClimbSpeed += 8;
        statManagerScript.LeapStaminaDepletion();
        yield return new WaitForSeconds(0.3f);
        ClimbSpeed -= 8;

    }

    void Move()
    {
        if (isClimbing && CanClimb)
        {
            Vector3 move = new Vector3(moveInput.x, moveInput.y, 0);

            move = transform.TransformDirection(move);

            characterController.Move(move * ClimbSpeed * Time.deltaTime);

        }
        else if (!isClimbing)
        {
            Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

            move = transform.TransformDirection(move);

            characterController.Move(move * moveSpeed * Time.deltaTime);

        }
    }

    public void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            float fallMultiplier = 2.5f;
            velocity.y += gravity * fallMultiplier * Time.deltaTime;
            velocity.y = Mathf.Max(velocity.y, terminalVelocity);
        }
        else if (velocity.y < 0)
        {
            velocity.y = groundedGravity;
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    public void LookAround()
    {
        /// Get horizontal and vertical look inputs and adjust based on sensitivity
        float LookX = lookInput.x * lookSpeed;
        float LookY = lookInput.y * lookSpeed;

        // Horizontal rotation: Rotate the player object around the y-axis
        transform.Rotate(0, LookX, 0);

        // Vertical rotation: Adjust the vertical look rotation and clamp it to prevent flipping
        verticalLookRotation -= LookY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90, 60);

        // Apply the clamped vertical rotation to the player camera
        playerCamera.localEulerAngles = new Vector3(verticalLookRotation, 0, 0);
    }

}
