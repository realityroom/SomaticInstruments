using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class BowingController : MonoBehaviour
{
    // the chuck subinstance
    public ChuckSubInstance m_chuck;
    // the previous position
    private Vector3 m_prevPos;
    // previous trigger state left
    private int m_prevTriggerLeft;
    // previous trigger state right
    private int m_prevTriggerRight;
    // integrator
    private float m_sum = 0f;
    // leak factor
    public float m_leak = .9999f;

    // get the controllers
    public SteamVR_Input_Sources m_leftHand = SteamVR_Input_Sources.LeftHand;
    public SteamVR_Input_Sources m_rightHand = SteamVR_Input_Sources.RightHand;

    // Start is called before the first frame update
    void Start()
    {
        // get the chuck sub instance
        m_chuck = GetComponent<ChuckSubInstance>();
        // run file
        m_chuck.RunFile( "bowing.ck", true );
    }

    // Update is called once per frame
    void Update()
    {
        // get current location
        Vector3 pos = transform.position;
        // delta since previous
        Vector3 diff = pos - m_prevPos;

        // put magnitude into leaky integrator
        m_sum += diff.magnitude;
        // leak!
        m_sum *= m_leak;

        Debug.Log("pos:" + pos + " prev:" + m_prevPos + " diff:" + diff + " sum:" + m_sum);

        // copy into prev
        m_prevPos = pos;

        m_prevTriggerLeft = (SteamVR_Actions._default.Squeeze.GetAxis(m_leftHand) > 0.0f ? 1 : 0);
        m_prevTriggerRight = (SteamVR_Actions._default.Squeeze.GetAxis(m_rightHand) > 0.0f ? 1 : 0);

        // send float to chuck
        m_chuck.SetFloat("bowIntensity", m_sum);
        m_chuck.SetFloat("thePitch", 24 + 36 * pos.y);
    }
}
