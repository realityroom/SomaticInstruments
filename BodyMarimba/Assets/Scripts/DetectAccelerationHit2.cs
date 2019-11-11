using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class DetectAccelerationHit2 : MonoBehaviour
{
    // VR controller preamble
    private SteamVR_Behaviour_Pose controllerPose;

    private void Awake()
    {
        controllerPose = GetComponent<SteamVR_Behaviour_Pose>();
    }


    // Check for acceleration hits and play the bongo!
    private void Update()
    {
        if( CheckForHit() )
        {
            float intensity = controllerPose.GetVelocity().magnitude.MapClamp( 0, 1.1f, 0.5f, 1 );
            MarimbaSoundEngine.PlayNote( intensity );
            
            // TODO haptic feedback
            
        }
    }

    public float accelerationThreshold = 0.5f;
	public float debounceTime = 0.166666667f;
    public float hapticDelayTime = 0.050f;


    private Vector3 velocity = Vector3.zero;
    private Vector3 acceleration = Vector3.zero;
    private Vector3 jerk = Vector3.zero;
    private float magnitudeJerk = 0f;
    private Vector3 prevVelocity = Vector3.zero;
    private Vector3 prevAcceleration = Vector3.zero;
    private Vector3 prevJerk = Vector3.zero;
    private float prevMagnitudeJerk = 0f;
    private float prevHitTime = float.NegativeInfinity;
	// Update is called once per frame
	private bool CheckForHit()
    {
        bool hit = false;

	    velocity = controllerPose.GetVelocity();
        acceleration = velocity - prevVelocity;
        jerk = acceleration - prevAcceleration;
        magnitudeJerk = acceleration.magnitude - prevAcceleration.magnitude;

        // peak when:
        // (1) Acceleration magnitude above a threshold
        if( ( acceleration.magnitude >= accelerationThreshold || prevAcceleration.magnitude >= accelerationThreshold ) 
        // (2) Acceleration magnitude was increasing last time (the top of a peak)
        //    && prevMagnitudeJerk > 0
        // (3) Acceleration magnitude is decreasing this time (crested the peak)
            && magnitudeJerk < 0
        // (4) Velocity is horizontal (i.e. not mostly upward or mostly downward)
            //&& ! ( WithinCone( velocity, Vector3.down, 45 ) || WithinCone( velocity, Vector3.up, 45 ) )
        // (5) We've waited beyond our debounce time
            && (Time.time - prevHitTime) > debounceTime )
        {
            hit = true;
            prevHitTime = Time.time;
        }


        // bookkeeping
        prevVelocity = velocity;
        prevAcceleration = acceleration;
        prevJerk = jerk;
        prevMagnitudeJerk = magnitudeJerk;

        // was there a hit?
        return hit;
	}

    private bool WithinCone( Vector3 vectorToTest, Vector3 coneBasis, float coneAngle )
    {
        float coneThreshold = Mathf.Cos( coneAngle * Mathf.Deg2Rad );
        return ( Vector3.Dot( coneBasis, vectorToTest.normalized ) > coneThreshold );
    }
}
