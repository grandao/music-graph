using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(ChuckSubInstance))]
public class GameController : MonoBehaviour
{
    string[] scale = new string[] { "A3", "B4", "C4", "D4", "E4", "F4", "G4" };
    int[] midis = new int[] { 69, 71, 72, 74, 76, 77, 79 };


    private void Awake()
    {
        //GetComponent<ChuckSubInstance>().chuckMainInstance = GameObject.Find("ChuckInstance").GetComponent<ChuckMainInstance>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ChuckSubInstance inst = GetComponent<ChuckSubInstance>();
        inst.RunCode(@"
public class Keyboard extends Chubgraph {
    SndBuf wav[87];
    
    fun void load(string path) {
        [""C"", ""Db"", ""D"", ""Eb"", ""E"", ""F"", ""Gb"", ""G"", ""Ab"", ""A"", ""Bb"", ""B""] @=> string notes[];


        for (0 => int i; i < 87; ++i)
        {
            //start at A0
            (i + 9) % 12 => int id;
            (i + 9) / 12 => int octave;

            path + ""/"" + notes[id] + Std.itoa(octave) + "".wav"" => wav[i].read;

            wav[i].samples() - 1 => wav[i].pos;
            wav[i] => outlet;
        }
    }

    fun void play(int midi)
    {
        midi - 21 => int id;

        if (id >= 0 && id < 87)
            0 => wav[id].pos;
    }


    fun void play(int midis[])
    {
        for (0 => int i; i < midis.cap(); ++i)
            play(midis[i]);
    }

    fun int listen()
    {
        MidiIn min;
        min.open(0) => int result;
        if (!result)
            return result;

        while (true)
        {
            min => now;
            MidiMsg msg;
            while (min.recv(msg))
            {
                //noteon
                if (msg.data1 >= 144 && msg.data1 <= 159 && msg.data3 != 0)
                {
                    play(msg.data2); //note number
                }
                //msg.data3 velocity
            }
        }
    }
}

Keyboard kb => dac;

<<<""Loading piano"">>>;
kb.load(me.dir() + ""piano"");

fun void play(int midi, float dt)
{
    kb.play(midi);
    dt::second => now;
}

global int note;
global float dt;
global Event ev;

<<<""Looping"">>>;
while (true) {
    ev => now;
    <<<""Sporking event"">>>;
    spork ~ play(note, dt);
}
");


        Play(69, 15);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Play(int midi, float dt)
    {
        ChuckSubInstance inst = GetComponent<ChuckSubInstance>();
        inst.SetFloat("dt", dt);
        inst.SetInt("note", midi);
        inst.BroadcastEvent("ev");
    }
}


public class Graph
{
    Node root;
    Dictionary<int, Node> nodes;
    Dictionary<int, SortedSet<int>> edges;
    Dictionary<int, SortedSet<int>> reverse_edges;

    public Graph()
    {
        Clear();
    }

    public void Clear()
    {
        root = null;
        nodes = new Dictionary<int, Node>();
        edges = new Dictionary<int, SortedSet<int>>();
        reverse_edges = new Dictionary<int, SortedSet<int>>();
    }

    public Node GetRoot() { return root; }
    public SortedSet<int> GetEdges(int id) { return edges[id]; }
    public Node GetNode(int id) { return nodes[id]; }

    public void AddNode(Node n)
    {
        nodes[n.id] = n;
        edges[n.id] = new SortedSet<int>();
        reverse_edges[n.id] = new SortedSet<int>();

        if (nodes.Count == 1)
            root = n;
    }

    public void RemoveNode(Node n)
    {
        if (n == root) return;

        nodes.Remove(n.id);
        foreach (var id in edges[n.id])
            reverse_edges[id].Remove(n.id);

        edges.Remove(n.id);
    }

    public void AddEdge(Edge e)
    {
        Node origin = e.origin;
        Node dest = e.dest;

        if (!nodes.ContainsKey(origin.id) || !nodes.ContainsKey(dest.id))
        {
            Debug.Log("Bad graph link!");
            return;
        }

        edges[origin.id].Add(dest.id);
        reverse_edges[dest.id].Add(origin.id); 
    }

    public void RemoveEdge(Edge e)
    {
        Node origin = e.origin;
        Node dest = e.dest;

        if (!nodes.ContainsKey(origin.id) || !nodes.ContainsKey(dest.id))
        {
            Debug.Log("Edge nodes does not exists!");
            return;
        }

        edges[origin.id].Remove(dest.id);
        reverse_edges[dest.id].Remove(origin.id);
    }
}




public class Scheduler
{
    int bpm = 60;
    int measure = 4;
    int subdiv = 2;

    int ticks = 0;

    ChuckSubInstance instance;
    Chuck.VoidCallback callback;

    public float dt
    {
        get { return 1.0f / (bpm * measure * subdiv); }
    }


    public Scheduler(ChuckSubInstance inst)
    {
        instance = inst;
        if (!instance.RunFile("runner.ck", false))
            Debug.Log("Could not load runner.ck!");

        callback = instance.CreateVoidCallback(Tick);
        instance.StartListeningForChuckEvent("notifier", callback);

        instance.SetFloat("dt", dt);
    }

    ~Scheduler()
    {
        instance.StopListeningForChuckEvent("notifier", callback);
    }

    void Play(int[] notes)
    {
        var data = Array.ConvertAll(notes, val => (long)val);
        instance.SetIntArray("midi", data);
        instance.SetInt("play", 1);
    }

    void Tick()
    {

    }
}