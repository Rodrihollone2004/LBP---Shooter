using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IState currentState;
    public PlayerInput playerInput;
    private CharacterController characterController;
    private Camera mainCamera;

    private float walkSpeed = 5f;
    private float runSpeed = 10f;
    private float crouchSpeed = 2.5f;
    private float jumpHeight = 1.25f;
    private float gravity = -9.81f;
    public float WalkSpeed { get => walkSpeed; set => walkSpeed = value; }
    public float RunSpeed { get => runSpeed; set => runSpeed = value; }
    public float CrouchSpeed { get => crouchSpeed; set => crouchSpeed = value; }
    public float JumpHeight { get => jumpHeight; set => jumpHeight = value; }
    public float Gravity { get => gravity; set => gravity = value; }

    private float verticalVelocity;
    public float VerticalVelocity { get => verticalVelocity; set => verticalVelocity = value; }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        mainCamera = GetComponentInChildren<Camera>();

        currentState = new WalkingState();
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void TransitionToState(IState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public void Move(Vector3 direction, float speed)
    {
        Vector3 move = mainCamera.transform.forward * direction.z + mainCamera.transform.right * direction.x;
        move.y = 0;
        characterController.Move(move * speed * Time.deltaTime);
        characterController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    public void CalculateVertical()
    {
        if (IsGrounded)
        {
            if (playerInput.IsJumping)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }


    public bool IsGrounded
    {
        get
        {
            return characterController.isGrounded;
        }
    }


    public void AdjustCrouchHeight(float targetHeight)
    {
        float targetLocalScaleY = playerInput.IsCrouch ? 0.65f : 1f;
        transform.localScale = new Vector3(1, targetHeight, 1);
    }
}
