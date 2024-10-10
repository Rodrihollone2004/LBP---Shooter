using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Flashbang : MonoBehaviour, IGrenade
{
    private Image WhiteImage;
    private Rigidbody RB;
    private ParticleSystem FlashParticle;
    private MeshRenderer Renderer;
    private AudioSource WhiteNoise;
    //private AudioSource Bang;
    private CameraFlashbang player;

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

        Renderer = gameObject.GetComponent<MeshRenderer>();
        WhiteImage = GameObject.FindGameObjectWithTag("WhiteImage").GetComponent<Image>();
        WhiteNoise = GameObject.FindGameObjectWithTag("WhiteNoise").GetComponent<AudioSource>();
        //Bang = GameObject.FindGameObjectWithTag("Bang").GetComponent<AudioSource>();
        FlashParticle = gameObject.GetComponent<ParticleSystem>();
        RB = gameObject.GetComponent<Rigidbody>();
        RB.velocity = new Vector3(10, 0, 0);
        player = FindObjectOfType<CameraFlashbang>();
        StartCoroutine(GrenadeController());

        //Para lanzar el objeto
    }

    public IEnumerator GrenadeController()
    {
        yield return new WaitForSeconds(4f);

        Flashbang flashbang = this;

        if (flashbang != null)
        {
            player.FlashbangPos = gameObject.transform;
            player.Flash = flashbang;

            player.CheckView(gameObject);

            if (player.IsFlashed)
            {
                WhiteImage.color = new Vector4(1, 1, 1, 1);
                FlashParticle.Play();
                //Bang.Play();
                WhiteNoise.Play();
                Renderer.enabled = false;

                float FadeSpeed = 1f;
                float Modifier = 0.01f;
                float WaitTime = 0;

                for (int i = 0; WhiteImage.color.a > 0; i++)
                {
                    WhiteImage.color = new Vector4(1, 1, 1, FadeSpeed);
                    FadeSpeed = FadeSpeed - 0.025f;
                    Modifier = Modifier * 1.5f;
                    WaitTime = 0.5f - Modifier;
                    if (WaitTime < 0.1f) WaitTime = 0.1f;
                    WhiteNoise.volume -= 0.05f;
                    yield return new WaitForSeconds(WaitTime);
                }

                WhiteNoise.Stop();
                WhiteNoise.volume = 1;
                Destroy(gameObject);
            }
            else
            {
                FlashParticle.Play();
                //Bang.Play();
                Renderer.enabled = false;
                Destroy(gameObject);
            }
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