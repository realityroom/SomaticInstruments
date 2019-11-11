using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarimbaSoundEngine : MonoBehaviour
{
    public static void SetNote( float note )
    {
        TheChuck.instance.SetFloat( "marimbaNote", note );
    }

    public static void PlayNote( float intensity )
    {
        TheChuck.instance.SetFloat( "marimbaNoteIntensity", intensity );
        TheChuck.instance.BroadcastEvent( "playMarimbaNote" );
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ChuckSubInstance>().RunCode( @"
            global float marimbaNote, marimbaNoteIntensity;
            global Event playMarimbaNote;

            SndBuf buf => JCRev rev => dac;
            0.05 => rev.mix;

            me.dir() + ""impact.wav"" => buf.read;
            buf.samples() - 1 => buf.pos;

            38 => int baseNote;

            Math.pow( 2, 1.0 / 12.0 ) => float twelfthOfTwo;
            1 / twelfthOfTwo => float oneOverTwelfthOfTwo;

            fun void SetRate()
            {
                if( marimbaNote >= baseNote )
                {
                    Math.pow( twelfthOfTwo, marimbaNote - baseNote ) => buf.rate;
                }
                else
                {
                    Math.pow( oneOverTwelfthOfTwo, baseNote - marimbaNote ) => buf.rate;
                }
            }

            fun void SetIntensity()
            {
                marimbaNoteIntensity => buf.gain;
            }

            while( true )
            {
                playMarimbaNote => now;
                SetRate();
                SetIntensity();
                0 => buf.pos;
            }
        " );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
