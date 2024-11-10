using System.Collections;
using UnityEngine;

public class GrenadeController : MonoBehaviour
{
    [SerializeField] Flashbang flashbang;
    [SerializeField] Smoke smoke;

    IGrenade grenade;

    private void Start()
    {
        grenade = flashbang;

        flashbang.ReadyToThrow = true;
        smoke.ReadyToThrow = true;
        flashbang.MainCamera = Camera.main;
        smoke.MainCamera = Camera.main;
    }

    private void Update()
    {
        SelectionGrenades();

        grenade.GrenadeInputs();
    }

    private void SelectionGrenades()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Flashbang");
            SetGrenade(flashbang);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Smoke");
            SetGrenade(smoke);
        }
    }

    public void SetGrenade(IGrenade grenade)
    {
        this.grenade = grenade;
    }

}
