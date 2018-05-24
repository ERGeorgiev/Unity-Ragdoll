using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charging : MonoBehaviourExt
{
    public float Thrust = 5000;
    public float fullChargeAngularDrag = 50;
    public float fullChargeSeconds = 1f;
    private float chargeIncrements;
    private float chargeValue = 0;

    public Transform chargeParticles;
    public Transform dischargeParticles;
    private GameObject chargeParticlesObj = null;

    public AudioClip chargeAudio;
    public AudioClip fullChargeAudio;
    private AudioSource audioSource;

    private void Start()
    {
        enabled = false;
        chargeIncrements = fullChargeAngularDrag * (Time.fixedDeltaTime / fullChargeSeconds);
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void SpawnChargeParticles()
    {
        chargeParticlesObj = Instantiate(chargeParticles, ObjectController.transform.position, chargeParticles.rotation, transform).gameObject;
    }

    private void SpawnFullChargeParticles()
    {
        Instantiate(dischargeParticles, ObjectController.transform.position, dischargeParticles.rotation);
    }

    private bool IsFullyCharged
    {
        get { return chargeValue >= fullChargeAngularDrag; }
    }

    private bool IsFullyDischarged
    {
        get { return chargeValue == 0; }
    }

    void FixedUpdate ()
    {
        Charge();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (chargeValue > 0 || ObjectController.FreezeRotation == true)
        {
            ObjectController.FreezeRotation = false;
            Discharge();
            enabled = false;
        }
    }

    public void Interrupt()
    {
        if (!IsFullyCharged)
        {
            audioSource.Stop();
            Discharge();
            enabled = false;
        }
    }

    private void Charge()
    {
        if (IsFullyCharged)
        {
            audioSource.PlayOneShot(fullChargeAudio);
            ObjectController.FreezeRotation = true;
            ApplyForce(MouseController.Lmb.Position);
            SpawnFullChargeParticles();
            Discharge();
            enabled = false;
        }
        else
        {
            if (chargeParticlesObj == null)
            {
                audioSource.PlayOneShot(chargeAudio);
                SpawnChargeParticles();
            }
            chargeValue += chargeIncrements;
            for (int i = 0; i < ObjectController.Child.Count; i++)
            {
                ObjectController.Child[i].drag += chargeIncrements;
                ObjectController.Child[i].angularDrag += chargeIncrements;
            }
        }
    }

    private void Discharge()
    {
        if (chargeParticlesObj != null)
        {
            Destroy(chargeParticlesObj);
        }
        for (int i = 0; i < ObjectController.Child.Count; i++)
        {
            ObjectController.Child[i].drag -= chargeValue;
            ObjectController.Child[i].angularDrag -= chargeValue;
        }
        chargeValue = 0;
    }

    private void ApplyForce(Vector2 target)
    {
        Vector2 forceVector = (target - RagdollController.Torso.position).normalized * Thrust;
        RagdollController.Torso.AddForce(forceVector, ForceMode2D.Impulse);
    }
}
