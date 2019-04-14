public class Keyboard extends Chubgraph {
    SndBuf wav[87];
    
    fun void load(string path) {
        ["C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B"] @=> string notes[];
        
        for (0 => int i; i < 87; ++i) {
            //start at A0
            (i+9) % 12 => int id;
            (i+9) / 12 => int octave;
            
            path + "/" + notes[id] + Std.itoa(octave) + ".wav" => wav[i].read;
            
            wav[i].samples() - 1 => wav[i].pos;
            wav[i] => outlet;
        }
    }
    
    fun void play(int midi) {
        midi - 21 => int id;
        
        if (id >= 0 && id < 87)
            0 => wav[id].pos;
    }
    
    
    fun void play(int midis[]) {
        for (0 => int i; i < midis.cap(); ++i)
            play(midis[i]);
    }
    
    fun int listen() {
        MidiIn min;
        min.open(0) => int result;
        if (!result)
            return result;
        
        while (true) {
            min => now;
            MidiMsg msg;
            while (min.recv(msg)) {
                //noteon
                if (msg.data1 >= 144 && msg.data1 <= 159 && msg.data3 != 0) {
                    play(msg.data2); //note number
                }
                //msg.data3 velocity
            }
        }
    }
}



Keyboard kb => Gain g => dac;
0.3 => g.gain;

kb.load(me.dir() + "piano");

0 => global int play;
[80] @=> global int midi[];
global float dt;


global Event start;
global Event notifier;


start => now;
while (true) {
    if (play != 0) {
        kb.play(midi);
        0 => play;
    }
    notifier.broadcast();
    dt::second => now; 
}



