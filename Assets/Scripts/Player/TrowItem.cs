using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform cam; // Referencia a la cámara
    GameObject objectToThrow; // Objeto a lanzar
    private Camera mainCamera; // Cámara principal

    [Header("Settings")]
    [SerializeField] int totalThrows; // Total de lanzamientos permitidos
    [SerializeField] float throwCoolDown; // Tiempo de espera entre lanzamientos

    [Header("Throwing")]
    [SerializeField] KeyCode throwKey = KeyCode.Mouse0; // Tecla para lanzar
    [SerializeField] float throwForce; // Fuerza del lanzamiento
    [SerializeField] float throwUpwardForce; // Fuerza vertical adicional

    bool readyToThrow;

    public GameObject ObjectToThrow { get => objectToThrow; set => objectToThrow = value; }

    private void Start()
    {
        mainCamera = Camera.main; // Inicializa la cámara principal
        readyToThrow = true; // Permite lanzar al inicio
    }

    private void Update()
    {
        if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0)
        {
            Throw();
        }
    }

    public void Throw()
    {
        readyToThrow = false;

        // Lanza un rayo desde el centro de la pantalla
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        // Si el rayo impacta en un objeto
        Vector3 throwPosition;

        // Si no impacta, lanza desde la posición de la cámara
        throwPosition = ray.origin + ray.direction * 5f; // Un poco delante de la cámara

        // Instanciar el objeto a lanzar
        GameObject throwObject = Instantiate(objectToThrow, throwPosition, Quaternion.identity);

        // RigidBody
        Rigidbody rbProjectile = throwObject.GetComponent<Rigidbody>();

        // Dirección
        Vector3 forceDirection = ray.direction.normalized;

        // Fuerza
        Vector3 forceAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        rbProjectile.AddForce(forceAdd, ForceMode.Impulse);

        totalThrows--; // Reduce el total de lanzamientos disponibles

        // CoolDown Throws
        Invoke(nameof(ResetThrow), throwCoolDown);
    }

    public void ResetThrow()
    {
        readyToThrow = true; // Permite lanzar de nuevo
    }
}
