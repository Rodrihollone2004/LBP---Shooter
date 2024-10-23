using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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

    [Header("WallRunning")]
    [SerializeField] private LayerMask wallLayer;
    private float wallCheckDistance = 2f;
    private bool isWallRunning;

    public bool IsWallRunning { get => isWallRunning; set => isWallRunning = value; }

    public RaycastHit wallHit;

    [Header("Sounds")]
    [SerializeField] private AudioClip runSound;
    private AudioSource audioSource;

    public bool hasLanded;

    [Header("Graffiti Settings")]
    [SerializeField] private Texture graffitiTexture;
    [SerializeField] private float graffitiSize = 1f;
    [SerializeField] private LayerMask placementLayer; 
    [SerializeField] private Material graffitiMaterial; 

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        audioSource = GetComponent<AudioSource>();
        mainCamera = GetComponentInChildren<Camera>();

        currentState = new WalkingState();
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);

        HandleGraffitiPlacement();

        Debug.Log($"IsGrounded: {characterController.isGrounded}");

        if (playerInput.IsRunning)
        {
            PlayRunSound();
        }
        else
        {
            StopRunSound();
        }
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
            else
            {
                verticalVelocity = 0;
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

    public bool IsTouchingWall()
    {
        Vector3 position = transform.position + Vector3.up * 0.5f;

        bool touchingWall = Physics.Raycast(position, mainCamera.transform.right, out wallHit, wallCheckDistance, wallLayer) || // Derecha
                            Physics.Raycast(position, -mainCamera.transform.right, out wallHit, wallCheckDistance, wallLayer) || // Izquierda
                            Physics.Raycast(position, mainCamera.transform.forward, out wallHit, wallCheckDistance, wallLayer) || // Frontal
                            Physics.Raycast(position, -mainCamera.transform.forward, out wallHit, wallCheckDistance, wallLayer); // Detrás


        Debug.DrawRay(position, mainCamera.transform.right * wallCheckDistance, Color.red); 
        Debug.DrawRay(position, -mainCamera.transform.right * wallCheckDistance, Color.green);
        Debug.DrawRay(position, mainCamera.transform.forward * wallCheckDistance, Color.blue); 
        Debug.DrawRay(position, -mainCamera.transform.forward * wallCheckDistance, Color.yellow);

        return touchingWall;
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
