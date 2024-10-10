using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke: MonoBehaviour
{
    private Rigidbody RB;
    private ParticleSystem SmokeParticle;
    private MeshRenderer Renderer;

    private void Start()
    {
        Renderer = GetComponent<MeshRenderer>();
        SmokeParticle = GetComponent<ParticleSystem>();
        RB = GetComponent<Rigidbody>();
        RB.velocity = new Vector3(10, 0, 0);
        StartCoroutine(WhiteFade());
    }

    private IEnumerator WhiteFade()
    {
        yield return new WaitForSeconds(4f);

        Smoke smoke = this;

        if (smoke != null)
        {
            SmokeParticle.Play();
            Renderer.enabled = false;
        }
    }
}