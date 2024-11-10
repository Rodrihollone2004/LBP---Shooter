using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IState currentState;
    public PlayerInput playerInput;
    private Camera mainCamera;
    private Rigidbody rb;

    private float walkSpeed = 3f;
    private float runSpeed = 5f;
    private float jumpHeight = 1.5f;
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

    [Header("Sounds")]
    [SerializeField] private AudioClip runSound;
    private AudioSource audioSource;

    [Header("Dash Settings")]
    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashUpwardForce = 2f;
    [SerializeField] private float dashDuration = 0.2f;
    public float DashDuration { get => dashDuration; set => dashDuration = value; }
    [SerializeField] private float dashCooldown = 1f;
    private float dashCooldownTimer;
    public float DashCooldownTimer { get => dashCooldownTimer; }
    private bool isDashing;
    private Vector3 dashDirection;

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
        HandleDashInput();

        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        HandleGraffitiPlacement();

        if (!isDashing)
        {
            Move();
            if (playerInput.IsRunning && IsGrounded() && IsMoving())
            {
                PlayRunSound();
            }
            else
            {
                StopRunSound();
            }
        }
    }

    private void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.C) && dashCooldownTimer <= 0)
        {
            if (!isDashing)
            {
                if (currentState is JumpingState)
                {
                    TransitionToState(new DashingState());
                }
                else if (currentState is WalkingState || currentState is RunningState)
                {
                    StartDash();
                }
            }
        }
    }


    public void StartDash()
    {
        isDashing = true;
        dashCooldownTimer = dashCooldown;

        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        dashDirection = mainCamera.transform.TransformDirection(inputDirection);

        rb.useGravity = false;
        rb.velocity = Vector3.zero; 

        Vector3 dashVelocity = dashDirection * dashForce + Vector3.up * dashUpwardForce;
        rb.AddForce(dashVelocity, ForceMode.Impulse);

        rb.drag = 0f; 
        rb.angularDrag = 0f;
    }


    public void EndDash()
    {
        isDashing = false;
        rb.useGravity = true;

        rb.drag = 5f;
        rb.angularDrag = 5f;

        dashDuration = 0.2f;
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
    private void HandleGraffitiPlacement()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlaceGraffiti();
        }
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

            StartCoroutine(DestroyGraffitiAfterTime(graffitiQuad, 5f));
        }
    }

    private IEnumerator DestroyGraffitiAfterTime(GameObject graffiti, float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(graffiti);
    }
}
