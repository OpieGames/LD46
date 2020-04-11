using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    public CPMPlayer Movement;
    public Animator Anim;
    public bool DoBob = true;
    [Range(0.0f, 1.0f)]
    public float BobMagnitude = 0.2f;
    [Range(0.1f, 50.0f)]
    public float BobFrequency = 2.0f;

    private const float MoveSpdScalingEnabledMin = 0.5f;
    private const float MoveSpdScalingDisabledMin = 0.25f;
    [Range(MoveSpdScalingEnabledMin, 1.0f)]
    public float MovementSpeedScaling = 1.0f;

    

    Vector3 StartingPoint;

    private float Accumulator = 0.0f;

    void Start()
    {
        if (!Movement)
        {
            Debug.LogError("WeaponBob script requires reference to CPMPlayer movement script for movement info!");
            DoBob = false;
        }
        if (!Anim)
        {
            Anim = gameObject.GetComponent<Animator>();
        }

        StartingPoint = transform.localPosition;
        // Anim.enabled = false;
    }

    void Update()
    {
        if (DoBob)
        {
            Bob();
        }
        
    }

    void Bob()
    {
        float MoveSpeed = Movement.GetSpeed();
        float MoveSpdFrequencyMult;
        float MoveSpdMagMult;
        bool IsGrounded = Movement.IsGrounded();
        if (MoveSpeed > 0.0f && IsGrounded)
        {
            MoveSpdFrequencyMult = Mathf.Max(MoveSpdScalingEnabledMin, ( (Mathf.Pow( MoveSpeed, 0.8f) / 0.25f ) * MovementSpeedScaling ) );
            MoveSpdMagMult = Mathf.Max( ( 1.0f - (Mathf.Min(MoveSpeed, 10.0f) / 10.0f) ) * MovementSpeedScaling, 0.0f);
        } else {
            MoveSpdFrequencyMult = MoveSpdScalingDisabledMin;
            MoveSpdMagMult = 1.0f;
        }
        
        Accumulator += MoveSpdFrequencyMult*BobFrequency*Time.deltaTime;

        transform.localPosition = new Vector3(StartingPoint.x, StartingPoint.y + (Mathf.Sin(Accumulator) * (MoveSpdMagMult*BobMagnitude*0.1f)), StartingPoint.z);

        if (Movement.jumpedThisFrame)
        {
            Anim.SetTrigger("Jump");
        } else if (Movement.landedThisFrame)
        {
            float MaxLandingSpeed = 16.0f;
            float LandingStr = Mathf.Max( Mathf.Pow( Mathf.Clamp( Mathf.Abs(Movement.landingVelocity), 0.0f, MaxLandingSpeed) / MaxLandingSpeed, 1.5f ), 0.1f);
            float LandingSpeed = 1.0f - LandingStr;
            Debug.Log("VEL: " + Movement.landingVelocity + " SCL: " + Mathf.Clamp( Mathf.Abs(Movement.landingVelocity), 0.0f, MaxLandingSpeed) / MaxLandingSpeed + " FNL: " + LandingStr);
            Anim.SetFloat("LandingStr",   LandingStr);
            Anim.SetFloat("LandingSpeed",   LandingSpeed);
            Anim.SetTrigger("Land");
        }

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + (Mathf.Sin(Accumulator) * (BobMagnitude*0.1f)), transform.localPosition.z);
        
    }
}
