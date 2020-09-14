using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticles : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, GetComponent<ParticleSystem>().duration);
    }
}
