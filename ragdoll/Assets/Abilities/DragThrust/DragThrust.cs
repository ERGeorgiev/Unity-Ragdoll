using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class DragThrust : MonoBehaviourExt
{
    private const float timeLimit = 0.5f;
    private bool followTrail = false;
    [Range(0,100)]
    public float InitialThrust = 10;

    private List<Vector2> Lmb_Trail;
    private float trailRepetition = 0;
    private int trailCount = 0;

    [SerializeField]
    protected UnityEvent OnExceedingTrailCapacity;

    private void Start()
    {
        Lmb_Trail = new List<Vector2>((int)(timeLimit / Time.fixedDeltaTime));
        enabled = false;
    }

    public void StartTrail()
    {
        Lmb_Trail.Clear();
        Lmb_Trail.Add(MouseController.Lmb.Position);
        followTrail = false;
        enabled = true;
    }

    public void StopTrail()
    {
        trailCount = Lmb_Trail.Count;
        followTrail = true;
    }

    private void FixedUpdate()
    {
        if (followTrail == false)
        {
            Lmb_Trail.Add(MouseController.Lmb.Position);
            if (Lmb_Trail.Count >= Lmb_Trail.Capacity)
            {
                enabled = false;
                Lmb_Trail.Clear();
                OnExceedingTrailCapacity.Invoke();
            }
        }
        else
        {
            if (Lmb_Trail.Count != 0)
            {
                if (trailRepetition == 0)
                {
                    Lmb_Trail[0] = (Lmb_Trail[0] - transform.GetComponent<Rigidbody2D>().position).normalized;
                    Lmb_Trail[0] *= 1 + Mathf.Pow((trailCount - Lmb_Trail.Count) / 100, 2);
                }
                RagdollController.Head.AddForce(Lmb_Trail[0], ForceMode2D.Impulse);
                if (trailRepetition >= 3)
                {
                    trailRepetition = 0;
                    Lmb_Trail.RemoveAt(0);
                }
                else trailRepetition++;
            }
            else
            {
                enabled = false;
            }
        }
    }

    public void ReflexThrust()
    {
        if (MouseController.Lmb.TimeHeld > timeLimit
            || MouseController.Lmb.IsClick())
        {
            return;
        }
        float timeMultiplier = Mathf.Pow(Mathf.Clamp(timeLimit / MouseController.Lmb.TimeDown, 1, Mathf.Infinity), 1f / 1f);

        ObjectController.NullifyVelocities();
        ApplyThrust(MouseController.Lmb.Path.magnitude * timeMultiplier);
    }

    public void ApplyThrust(float mupltiplier)
    {
        Vector2 forceVector = MouseController.Lmb.Path * InitialThrust * mupltiplier;
        forceVector /= 2;
        //RagdollController.Head.AddForce(forceVector * Mathf.Clamp(Mathf.Cos(angle * Mathf.PI / 180), 0, 1), ForceMode2D.Impulse);
        //RagdollController.Torso.AddForce(forceVector * Mathf.Clamp(Mathf.Sin(angle * Mathf.PI / 180), 0, 1), ForceMode2D.Impulse);
        //RagdollController.LegLeft.AddForce(forceVector / 2 * -Mathf.Clamp(Mathf.Cos(angle * Mathf.PI / 180), -1, 0), ForceMode2D.Impulse);
        //RagdollController.LegRight.AddForce(forceVector / 2 * -Mathf.Clamp(Mathf.Cos(angle * Mathf.PI / 180), -1, 0), ForceMode2D.Impulse);
        //RagdollController.Torso.AddForceAtPosition(MouseController.Lmb.Path * InitialThrust, MouseController.Lmb.PositionDown, ForceMode2D.Impulse);
        RagdollController.Torso.AddForce(new Vector2(forceVector.x, 0), ForceMode2D.Impulse);
        ObjectController.NullifyAngularVelocity();
    }

    public void ApplyThrust(float mupltiplier, Vector2 target)
    {
        Vector2 forceVector = (target - RagdollController.Torso.position).normalized * InitialThrust * mupltiplier;
        RagdollController.Torso.AddForce(forceVector, ForceMode2D.Impulse);
    }
}
