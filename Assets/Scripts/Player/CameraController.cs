using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public float sensitivity = 400f;
    private float xRot = 0f;
    [SerializeField] public Transform playerBody;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -45, 45);

        transform.localRotation = Quaternion.Euler(xRot, transform.localRotation.eulerAngles.y + mouseX, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
