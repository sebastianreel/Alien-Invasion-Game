using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootParticle : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<ParticleSystem>().Play();
    }
}
