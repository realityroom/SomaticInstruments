using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class MarimbaPlayNote : MonoBehaviour
{
    // VR controller preamble
    private SteamVR_Behaviour_Pose controllerPose;

    public float debounceTime = 0.166666667f;
    private float prevHitTime = float.NegativeInfinity;

    private void Awake()
    {
        controllerPose = GetComponent<SteamVR_Behaviour_Pose>();
    }

    public void PlayNote()
    {
        if( ( Time.time - prevHitTime ) > debounceTime )
        {
            prevHitTime = Time.time;

            float intensity = controllerPose.GetVelocity().magnitude.MapClamp( 0, 1.1f, 0.3f, 1 );
            MarimbaSoundEngine.PlayNote( intensity );
        }

    }
}
