using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarimbaNote : MonoBehaviour
{
    public float myMidiNote;

    void OnTriggerEnter( Collider collider )
    {
        if( collider.gameObject.CompareTag( "ControllerTip" ) )
        {
            MarimbaSoundEngine.SetNote( myMidiNote );
            collider.GetComponent<MarimbaPlayNote>().PlayNote();
        }
    }
}
