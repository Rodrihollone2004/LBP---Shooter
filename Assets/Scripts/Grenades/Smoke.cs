using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke: MonoBehaviour, IGrenade
{
    private Rigidbody RB;
    private ParticleSystem SmokeParticle;
    private MeshRenderer Renderer;

    [Header("Setting Throw Item")]
    [SerializeField] KeyCode throwKey = KeyCode.M;

    private Camera mainCamera; 
    GameObject objectToThrow; 
    [SerializeField] float throwForce; 
    [SerializeField] float throwUpwardForce; 

    [SerializeField] int totalThrows; 
    [SerializeField] float throwCoolDown; 

    bool readyToThrow;

    public bool ReadyToThrow { get => readyToThrow; set => readyToThrow = value; }
    public Camera MainCamera { get => mainCamera; set => mainCamera = value; }

    private void Start()
    {
        objectToThrow = this.gameObject;

        Renderer = GetComponent<MeshRenderer>();
        SmokeParticle = GetComponent<ParticleSystem>();
        RB = GetComponent<Rigidbody>();
        RB.velocity = new Vector3(10, 0, 0);
        StartCoroutine(GrenadeController());

        //Para lanzar el objeto
        mainCamera = Camera.main; 
        readyToThrow = true; 
    }

    public IEnumerator GrenadeController()
    {
        yield return new WaitForSeconds(4f);

        Smoke smoke = this;

        if (smoke != null)
        {
            SmokeParticle.Play();
            Renderer.enabled = false;
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

    public void GrenadeInputs()
    {
        if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0)
        {
            Throw();
        }
    }
}