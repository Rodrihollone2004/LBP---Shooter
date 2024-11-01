using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IState currentState;
    public PlayerInput playerInput;
    private Camera mainCamera;
    private Rigidbody rb;

    private float walkSpeed = 5f;
    private float runSpeed = 10f;
    private float jumpHeight = 1.25f;
    private float gravity = -9.81f;
    private Vector3 velocity;
    [SerializeField] private LayerMask groundLayer;
    public Vector3 Velocity { get => velocity; set => velocity = value; }
    public float WalkSpeed { get => walkSpeed; set => walkSpeed = value; }
    public float RunSpeed { get => runSpeed; set => runSpeed = value; }
    public float JumpHeight { get => jumpHeight; set => jumpHeight = value; }
    public float Gravity { get => gravity; set => gravity = value; }

    private float verticalVelocity;
    public float VerticalVelocity { get => verticalVelocity; set => verticalVelocity = value; }

    [Header("WallRunning")]
    [SerializeField] private LayerMask wallLayer;
    private float wallCheckDistance = 2f;
    private bool isWallRunning;
    [SerializeField] private float wallRunCameraTilt = 15f;

    [SerializeField] private float impulseAngle = 1.2f; //ajustar el impulso más hacia arriba
    [SerializeField] private float impulseSideWall = 10f; // ajustar la fuerza de impulso para el lado contrario de la pared

    public float WallRunCameraTilt { get => wallRunCameraTilt; set => wallRunCameraTilt = value; }
    public bool IsWallRunning { get => isWallRunning; set => isWallRunning = value; }

    public RaycastHit wallHit;

    [Header("Sounds")]
    [SerializeField] private AudioClip runSound;
    private AudioSource audioSource;

    [Header("Graffiti Settings")]
    [SerializeField] private Texture graffitiTexture;
    [SerializeField] private float graffitiSize = 1f;
    [SerializeField] private LayerMask placementLayer; 
    [SerializeField] private Material graffitiMaterial; 

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        audioSource = GetComponent<AudioSource>();
        mainCamera = GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        currentState = new WalkingState();
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
        HandleGraffitiPlacement();
        if (playerInput.IsRunning && IsGrounded() && IsMoving())
        {
            PlayRunSound();
        }
        else
        {
            StopRunSound();
        }
        Move();
    }

    private bool IsMoving()
    {
        Vector3 horizontalMove = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        return horizontalMove.magnitude > 0.1f;
    }

    private void PlayRunSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = runSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void StopRunSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void HandleGraffitiPlacement()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlaceGraffiti();
        }
    }

    public void TransitionToState(IState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public void Move()
    {
        float speed = playerInput.IsRunning ? runSpeed : walkSpeed;
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 move = mainCamera.transform.forward * direction.z + mainCamera.transform.right * direction.x;
        move.y = 0;

        rb.MovePosition(rb.position + move.normalized * speed * Time.deltaTime);

        if (IsGrounded())
        {
            if (velocity.y < 0)
            {
                velocity.y = -0f;
            }

            if (playerInput.IsJumping)
            {
                velocity.y = Mathf.Sqrt(JumpHeight * -2f * gravity);
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        rb.MovePosition(rb.position + new Vector3(0, velocity.y, 0) * Time.deltaTime);
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }

    public void AdjustCrouchHeight(float targetHeight)
    {
        float targetLocalScaleY = playerInput.IsCrouch ? 0.65f : 1f;
        transform.localScale = new Vector3(1, targetHeight, 1);
    }

    public bool IsTouchingWall()
    {
        Vector3 position = transform.position + Vector3.up * 0.5f;

        bool touchingWall = Physics.Raycast(position, mainCamera.transform.right, out wallHit, wallCheckDistance, wallLayer) ||
                            Physics.Raycast(position, -mainCamera.transform.right, out wallHit, wallCheckDistance, wallLayer);

        Debug.DrawRay(position, mainCamera.transform.right * wallCheckDistance, Color.red);
        Debug.DrawRay(position, -mainCamera.transform.right * wallCheckDistance, Color.green);

        return touchingWall;
    }

    public void JumpOffWall()
    {
        Vector3 jumpDirection = (wallHit.normal + Vector3.up * impulseAngle).normalized;
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        rb.MovePosition(rb.position + jumpDirection * runSpeed * impulseSideWall * Time.deltaTime);
        TransitionToState(new JumpingState());
    }

    private void PlaceGraffiti()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 10f, placementLayer))
        {
            GameObject graffitiQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);

            Renderer quadRenderer = graffitiQuad.GetComponent<Renderer>();
            quadRenderer.material = graffitiMaterial;

            quadRenderer.material.mainTexture = graffitiTexture;

            graffitiQuad.transform.position = hitInfo.point + hitInfo.normal * 0.01f; 
            graffitiQuad.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
            graffitiQuad.transform.localScale = new Vector3(graffitiSize, graffitiSize, 1f);

            Destroy(graffitiQuad.GetComponent<Collider>());
        }
    }

}
