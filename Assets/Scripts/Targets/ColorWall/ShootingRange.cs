using UnityEngine;

public class ShootingRange : MonoBehaviour
{
    [SerializeField] private GameObject[] targets;
    [SerializeField] private Color activeColor = Color.red;
    [SerializeField] private Color inactiveColor = Color.white;
    [SerializeField] private ActivateTraining activate;

    private ITargetState currentState;
    private int activeTargetIndex;
    private bool isTargetActive = false;

    public GameObject[] Targets { get => targets; set => targets = value; }
    public Color ActiveColor { get => activeColor; set => activeColor = value; }
    public Color InactiveColor { get => inactiveColor; set => inactiveColor = value; }
    public bool IsTargetActive { get => isTargetActive; set => isTargetActive = value; }

    private void Start()
    {
        currentState = new InactiveState();
        DeactivateAllTargets();
        StartNewRound();
    }

    public void SetState(ITargetState state)
    {
        currentState = state;
    }

    public void HitTarget(GameObject hitTarget)
    {
        if (hitTarget == targets[activeTargetIndex] && isTargetActive)
        {
            currentState.Deactivate(this, activeTargetIndex);
            StartNewRound();

            activate.CurrentTargets += 1;
        }
    }

    private void StartNewRound()
    {
        activeTargetIndex = Random.Range(0, targets.Length);
        currentState.Activate(this, activeTargetIndex);
    }

    private void ActivateTarget(int index)
    {
        targets[index].GetComponent<Renderer>().material.color = activeColor;
        isTargetActive = true;
    }

    private void DeactivateTarget(int index)
    {
        targets[index].GetComponent<Renderer>().material.color = inactiveColor;
        isTargetActive = false;
    }

    private void DeactivateAllTargets()
    {
        foreach (var target in targets)
        {
            target.GetComponent<Renderer>().material.color = inactiveColor;
        }
        isTargetActive = false;
    }
}
