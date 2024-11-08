using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivateTraining : MonoBehaviour
{
    [Header("Textos")]
    [SerializeField] private TMP_Text startTraining;
    [SerializeField] private TMP_Text timeOfTraining;
    [SerializeField] private TMP_Text targetsOfTraining;

    [Header("Imagenes según condición")]
    [SerializeField] private Image victoryScreen;
    [SerializeField] private Image defeatScreen;
                     
    [Header("Variables de cada Actividad")]
    [SerializeField] private float timeTraining;
    private float startTimer;
    [SerializeField] private int totalTargetsTraining;
    private int currentTargets = 0;

    bool isTraining;

    public int CurrentTargets { get => currentTargets; set => currentTargets = value; }

    private void Start()
    {
        startTimer = timeTraining;
    }

    private void Update()
    {
        if (isTraining)
        {
            StartTraining();
        }
        else
        {
            StopTraining();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            startTraining.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.H) && isTraining == false)
            {
                StartTraining();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isTraining)
                CheckCondition();

            isTraining = false;
            startTraining.gameObject.SetActive(false);
            StopTraining();
        }
    }

    private void StartTraining()
    {
        startTraining.gameObject.SetActive(false);
        isTraining = true;

        timeOfTraining.text = timeTraining.ToString("F2");
        targetsOfTraining.text = $"{currentTargets} / {totalTargetsTraining}";

        timeOfTraining.gameObject.SetActive(true);
        targetsOfTraining.gameObject.SetActive(true);

        TimerTrainings();
    }

    private void StopTraining()
    {
        currentTargets = 0;
        timeTraining = startTimer;

        timeOfTraining.gameObject.SetActive(false);
        targetsOfTraining.gameObject.SetActive(false);
    }

    private void TimerTrainings()
    {
        timeTraining -= Time.deltaTime;

        if (timeTraining <= 0 && isTraining)
        {
            isTraining = false;
            timeTraining = startTimer;

            CheckCondition();
            StopTraining();
        }
    }

    private void CheckCondition()
    {
        if (currentTargets >= totalTargetsTraining)
        {
            StartCoroutine(SetScreenCondition(victoryScreen));
        }
        else
        {
            StartCoroutine(SetScreenCondition(defeatScreen));
        }
    }

    private IEnumerator SetScreenCondition(Image image)
    {
        image.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        image.gameObject.SetActive(false);
    }
}
