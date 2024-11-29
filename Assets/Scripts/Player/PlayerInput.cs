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
    [SerializeField] private KeyCode previous = KeyCode.O;
    [SerializeField] private KeyCode quit = KeyCode.Escape;
    [SerializeField] private KeyCode next = KeyCode.P;

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

        if (Input.GetKeyDown(previous))
            LoadPreviousScene();

        if (Input.GetKeyDown(quit))
            SceneManager.LoadScene("Menu");

        if (Input.GetKeyDown(next))
            LoadNextScene();

        inputVector = new Vector3(xInput, yInput, zInput).normalized;

        isJumping = Input.GetKeyDown(jump);
        isRunning = Input.GetKey(run);
        isCrouch = Input.GetKey(crouch);
    }
    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void LoadPreviousScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int previousSceneIndex = (currentSceneIndex - 1 + SceneManager.sceneCountInBuildSettings) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(previousSceneIndex);
    }
    void Update()
    {
        HandleInput();
    }
}
