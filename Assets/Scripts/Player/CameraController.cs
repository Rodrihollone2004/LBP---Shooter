using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public float sensitivity = 100f;
    private float xRot = 0f;
    private float tilt = 0f;
    [SerializeField] public Transform playerBody;
    [SerializeField] public PlayerController playerController;


    private void Start()
    {
        sensitivity = PlayerPrefs.GetFloat("CameraSensitivity", sensitivity);
        Cursor.lockState = CursorLockMode.Locked;
        playerController = FindObjectOfType<PlayerController>();
    }
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -45, 45);

        if (playerController.IsWallRunning)
        {
            float tiltDirection = Vector3.Dot(playerController.wallHit.normal, playerBody.right) > 0 ? -1 : 1;
            tilt = Mathf.Lerp(tilt, playerController.WallRunCameraTilt * tiltDirection, Time.deltaTime * 5f);
        }
        else
        {
            tilt = Mathf.Lerp(tilt, 0, Time.deltaTime * 5f);
        }

        transform.localRotation = Quaternion.Euler(xRot, 0f, tilt);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
