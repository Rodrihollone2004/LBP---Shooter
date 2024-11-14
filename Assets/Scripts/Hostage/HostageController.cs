using UnityEngine;

public class HostageController : MonoBehaviour
{
    Transform playerBehind;
    [SerializeField] float speed;
    [SerializeField] float stoppingDistance;
    [SerializeField] float holdKeyTime = 4f;
    float currentHoldKey;

    bool isCaught;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        playerBehind = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        Inputs();

        if (isCaught)
        {
            FollowPlayer();
        }
        else
        {
            StopFollow();
        }
    }

    private void FollowPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerBehind.position);

        if (distanceToPlayer > stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerBehind.position, speed * Time.deltaTime);
        }
    }

    private void StopFollow()
    {
        //Acá se pone la lógica de lo que va a hacer el rehen cuando esté sin pickear
    }

    private void Inputs()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        int layerMask = LayerMask.GetMask("Hostage");

        if (Physics.Raycast(ray, out hit, 2f, layerMask))
        {
            if (Input.GetKey(KeyCode.Alpha4) && !isCaught)
            {
                currentHoldKey += Time.deltaTime;

                if (currentHoldKey >= holdKeyTime)
                {
                    Debug.Log("Rehén en manos");
                    isCaught = true;
                    currentHoldKey = 0f;
                }
            }
            else
            {
                currentHoldKey = 0f;
            }
        }

        else if (Input.GetKeyDown(KeyCode.Alpha4) && isCaught)
        {
            isCaught = false;
        }
    }
}
