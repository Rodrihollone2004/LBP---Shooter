using TMPro;
using UnityEngine;

public class HostageController : MonoBehaviour
{
    Transform playerBehind;
    [SerializeField] float speed;
    [SerializeField] float stoppingDistance;
    [SerializeField] float holdKeyTime = 4f;
    [SerializeField] TMP_Text textHostagePick;
    [SerializeField] TMP_Text textHostageNoPick;
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
        //Ac� se pone la l�gica de lo que va a hacer el rehen cuando est� sin pickear // cheto
    }

    private void Inputs()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        int layerMask = LayerMask.GetMask("Hostage");

        if (Physics.Raycast(ray, out hit, 2f, layerMask))
        {
            textHostageNoPick.text = "Hold 4 to pick hostage";
            textHostageNoPick.gameObject.SetActive(true);

            if (Input.GetKey(KeyCode.Alpha4) && !isCaught)
            {
                currentHoldKey += Time.deltaTime;

                if (currentHoldKey >= holdKeyTime)
                {
                    textHostageNoPick.gameObject.SetActive(false);
                    textHostagePick.gameObject.SetActive(true);
                    textHostagePick.text = "Press 4 to leave hostage";
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
            textHostagePick.gameObject.SetActive(false);
            isCaught = false;
        }      
        else
        {
            textHostageNoPick.gameObject.SetActive(false);
        }
        if (isCaught)
        {
            textHostageNoPick.gameObject.SetActive(false);
        }
    }
}
