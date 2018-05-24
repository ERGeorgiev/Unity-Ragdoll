using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemAutodestroy : MonoBehaviour
{
    private new ParticleSystem particleSystem;
    
	void Start ()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }
	
	void FixedUpdate ()
    {
        if (particleSystem)
        {
            if (particleSystem.IsAlive() == false)
            {
                Destroy(gameObject);
            }
        }
    }
}
