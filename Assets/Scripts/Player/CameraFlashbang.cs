using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlashbang : MonoBehaviour
{
    Camera cam;
    [SerializeField] Transform flashbangPos; // Referencia a la flashbang
    Flashbang flash; // Referencia al script de la flashbang
    [SerializeField] float flashbangTriggerAngle = 45f; // angulo dentro del cual el jugador se flashea
    [SerializeField] LayerMask obstacleLayer; // Capa de obstaculos (paredes, suelos, etc.)
    bool isFlashed;

    public Transform FlashbangPos { get => flashbangPos; set => flashbangPos = value; }
    public Flashbang Flash { get => flash; set => flash = value; }
    public bool IsFlashed { get => isFlashed; set => isFlashed = value; }

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    public void CheckView(GameObject flashbang)
    {
        Vector3 cameraForward = cam.transform.forward;
        Vector3 directionToFlash = (flashbangPos.position - cam.transform.position).normalized;
        float angle = Vector3.Angle(cameraForward, directionToFlash);

        if (angle <= flashbangTriggerAngle && flashbang != null)
        {
            RaycastHit hit;
            Vector3 rayDirection = flashbangPos.position - cam.transform.position;

            if (Physics.Raycast(cam.transform.position, rayDirection, out hit, Mathf.Infinity))
            {
                if (hit.transform == flashbangPos)
                {
                    isFlashed = true;
                }
                else if (((1 << hit.transform.gameObject.layer) & obstacleLayer) != 0)
                {
                    isFlashed = false;
                }
                else
                {
                    isFlashed = true;
                }
            }
            else
            {
                isFlashed = false;
            }
        }
        else
        {
            isFlashed = false;
        }
    }
}