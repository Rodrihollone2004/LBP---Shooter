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