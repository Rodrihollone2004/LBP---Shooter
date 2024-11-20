using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private KeyCode forward = KeyCode.W;
    [SerializeField] private KeyCode backward = KeyCode.S;
    [SerializeField] private KeyCode left = KeyCode.A;
    [SerializeField] private KeyCode right = KeyCode.D;
    [SerializeField] private KeyCode jump = KeyCode.Space;
    [SerializeField] private KeyCode run = KeyCode.LeftShift;
    [SerializeField] private KeyCode crouch = KeyCode.LeftControl;
    [SerializeField] private KeyCode restart = KeyCode.Z;
    [SerializeField] private KeyCode quit = KeyCode.Escape;

    private Vector3 inputVector;
    public Vector3 InputVector => inputVector;

    private bool isJumping;
    public bool IsJumping { get => isJumping; set => isJumping = value; }

    private bool isRunning;
    public bool IsRunning { get => isRunning; set => isRunning = value; }

    private bool isCrouch;
    public bool IsCrouch { get => isCrouch; set => isCrouch = value; }

    private float xInput;
    private float yInput;
    private float zInput;

    public void HandleInput()
    {
        xInput = 0;
        yInput = 0;
        zInput = 0;

        if (Input.GetKey(forward))
        {
            zInput++;
        }
        if (Input.GetKey(backward))
        {
            zInput--;
        }
        if (Input.GetKey(left))
        {
            xInput--;
        }
        if (Input.GetKey(right))
        {
            xInput++;
        }

        if (Input.GetKeyDown(restart))
            SceneManager.LoadScene("Game");

        if (Input.GetKeyDown(quit))
            Application.Quit();

        inputVector = new Vector3(xInput, yInput, zInput).normalized;

        isJumping = Input.GetKeyDown(jump);
        isRunning = Input.GetKey(run);
        isCrouch = Input.GetKey(crouch);
    }
    void Update()
    {
        HandleInput();
    }
}
