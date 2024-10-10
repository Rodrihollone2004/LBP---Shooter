using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlashbang : MonoBehaviour
{
    Camera cam;
    [SerializeField] Transform flashbangPos; // Referencia a la flashbang
    Flashbang flash; // Referencia al script de la flashbang
    [SerializeField] float flashbangTriggerAngle = 45f; // �ngulo dentro del cual el jugador se flashea
    [SerializeField] LayerMask obstacleLayer; // Capa de obst�culos (paredes, suelos, etc.)
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
            // Usamos Raycast para verificar si hay un obst�culo en la l�nea de visi�n
            RaycastHit hit;
            Vector3 rayDirection = flashbangPos.position - cam.transform.position;

            // Realizamos el Raycast y verificamos si golpea algo antes de la flashbang
            if (Physics.Raycast(cam.transform.position, rayDirection, out hit, Mathf.Infinity))
            {
                // Si golpeamos la flashbang directamente
                if (hit.transform == flashbangPos)
                {
                    // No hay obst�culos entre la c�mara y la flashbang
                    Debug.Log("L�nea de visi�n despejada, jugador flasheado.");
                    isFlashed = true;
                }
                else if (((1 << hit.transform.gameObject.layer) & obstacleLayer) != 0)
                {
                    // Si golpeamos algo que pertenece a la capa de obst�culos, bloqueamos la flash
                    Debug.Log("L�nea de visi�n bloqueada por: " + hit.transform.name);
                    isFlashed = false;
                }
                else
                {
                    // Si golpeamos otro objeto que no pertenece a la capa de obst�culos, ignoramos el hit
                    Debug.Log("Objeto irrelevante golpeado: " + hit.transform.name);
                    isFlashed = true;
                }
            }
            else
            {
                // No hay ning�n obst�culo y no se golpe� la flashbang
                Debug.Log("No se detect� la flashbang en la l�nea de visi�n.");
                isFlashed = false;
            }
        }
        else
        {
            Debug.Log("C�mara no est� mirando hacia la flashbang.");
            isFlashed = false;
        }
    }
}