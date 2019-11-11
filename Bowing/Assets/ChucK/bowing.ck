//global float bowIntensity;
//global Event bowChangedDir;

SinOsc sqr => NRev r => dac;
.1 => r.mix;
440 => sqr.freq;
.1 => sqr.gain;

<<< "running chuck code..." >>>;

while( true )
{
    1::second => now;
}
