using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrigger : MonoBehaviour
{
  
    //public ParticleSystem explosion;
    public CameraShake cameraShake;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //explosion.Play();
            StartCoroutine(cameraShake.Shake(.15f,.4f));
        }
    }
}
