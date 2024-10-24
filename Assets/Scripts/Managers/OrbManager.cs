using System.Collections;
using UnityEngine;

public class OrbManager: MonoBehaviour
{
    public IEnumerator RespawnOrb(GameObject orb, float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);

        orb.SetActive(true);
    }
}
