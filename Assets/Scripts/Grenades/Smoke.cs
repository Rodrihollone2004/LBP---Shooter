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
    [SerializeField] GameObject objectToThrow; 
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
        StartCoroutine(GrenadeController());

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
        if (objectToThrow != null && ReadyToThrow)
        {
            GameObject thrownObject = Instantiate(objectToThrow, MainCamera.transform.position + MainCamera.transform.forward, Quaternion.identity);

            Rigidbody rb = thrownObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 throwDirection = MainCamera.transform.forward;
                rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
                rb.AddForce(MainCamera.transform.up * throwUpwardForce, ForceMode.Impulse);
            }

            totalThrows--; 

            Invoke(nameof(ResetThrow), throwCoolDown);
        }
    }

    public void ResetThrow()
    {
        readyToThrow = true; 
    }

    public void GrenadeInputs()
    {
        if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0)
        {
            Throw();
        }
    }
}