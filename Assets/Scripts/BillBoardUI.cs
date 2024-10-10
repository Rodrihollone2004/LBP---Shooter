using UnityEngine;

public class BillBoardUI : MonoBehaviour
{
    private Camera playerCamera;
    private void Start()
    {
        playerCamera = Camera.main;
    }

    private void LateUpdate()
    {
        LookAtPlayer();
    }

    private void LookAtPlayer()
    {
        Vector3 directionToFace = transform.position - playerCamera.transform.position;
        directionToFace.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(directionToFace);
        transform.rotation = targetRotation;
    }

}
