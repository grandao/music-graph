using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ITuple = System.Tuple<float, int>;
using NoteList = System.Collections.Generic.List<System.Tuple<float, int>>;

public class DecorationStyleScheduler
{
    public delegate void PushNote(int time, int note);
    public struct Pair { int note; int time; }

    int[] midis = new int[] { 69, 71, 72, 74, 76, 77, 79 };


    Dictionary<int, NoteList> styles = new Dictionary<int, NoteList>();

    //initial manual setup
    //later should load if from a file
    // (time, note)
    // time is relative to whole note
    public void Init()
    {
        // single note
        var list = new NoteList();
        list.Add(new ITuple(0, 0));
        styles[0] = list;

        //major chord
        list = new NoteList();
        list.Add(new ITuple(0, 0));
        list.Add(new ITuple(0, 4));
        list.Add(new ITuple(0, 7));
        styles[1] = list;

        //minor chord
        list = new NoteList();
        list.Add(new ITuple(0, 0));
        list.Add(new ITuple(0, 3));
        list.Add(new ITuple(0, 7));
        styles[2] = list;

        //major chord arp
        list = new NoteList();
        list.Add(new ITuple(0.00f, 0));
        list.Add(new ITuple(0.25f, 4));
        list.Add(new ITuple(0.50f, 7));
        list.Add(new ITuple(0.75f, 4));

        list.Add(new ITuple(1.00f, 0));
        list.Add(new ITuple(1.25f, 4));
        list.Add(new ITuple(1.50f, 7));
        list.Add(new ITuple(1.75f, 4));
        styles[3] = list;

        //minor chord
        list = new NoteList();
        list.Add(new ITuple(0.00f, 0));
        list.Add(new ITuple(0.25f, 3));
        list.Add(new ITuple(0.50f, 7));
        list.Add(new ITuple(0.75f, 3));

        list.Add(new ITuple(1.00f, 0));
        list.Add(new ITuple(1.25f, 3));
        list.Add(new ITuple(1.50f, 7));
        list.Add(new ITuple(1.75f, 3));
        styles[4] = list;

        //major chord arp 2
        list = new NoteList();
        list.Add(new ITuple(0.00f, 0));
        list.Add(new ITuple(0.33f, 4));
        list.Add(new ITuple(0.66f, 7));

        list.Add(new ITuple(1.00f, 0));
        list.Add(new ITuple(1.33f, 4));
        list.Add(new ITuple(1.66f, 7));
        styles[5] = list;

        //minor chord 2
        list = new NoteList();
        list.Add(new ITuple(0.00f, 0));
        list.Add(new ITuple(0.33f, 3));
        list.Add(new ITuple(0.66f, 7));

        list.Add(new ITuple(1.00f, 0));
        list.Add(new ITuple(1.33f, 3));
        list.Add(new ITuple(1.66f, 7));
        styles[6] = list;

        //double stop
        list = new NoteList();
        list.Add(new ITuple(0, 0));
        list.Add(new ITuple(0, -12));
        styles[7] = list;

        //double stop 2
        list = new NoteList();
        list.Add(new ITuple(0.00f, 0));
        list.Add(new ITuple(0.00f, -12));
        list.Add(new ITuple(0.75f, 7 - 12));
        styles[8] = list;

        //double stop 3
        list = new NoteList();
        list.Add(new ITuple(0.00f, 0));
        list.Add(new ITuple(0.00f, -12));

        list.Add(new ITuple(0.33f, 0));
        list.Add(new ITuple(0.33f, -12));

        list.Add(new ITuple(0.66f, 0));
        list.Add(new ITuple(0.66f, -12));
        styles[9] = list;

        //double stop 4
        list = new NoteList();
        list.Add(new ITuple(0.00f, 0));
        list.Add(new ITuple(0.00f, -12));

        list.Add(new ITuple(0.25f, 0));
        list.Add(new ITuple(0.25f, -12));

        list.Add(new ITuple(0.50f, 0));
        list.Add(new ITuple(0.50f, -12));

        list.Add(new ITuple(0.75f, 0));
        list.Add(new ITuple(0.75f, -12));
        styles[10] = list;
    }

    public void Schedule(Node node, int subdiv, PushNote push)
    {
        int next = (int)(node.duration * subdiv);
        int root = midis[node.note];

        int id = node.socket.GetId();
        if (id < 0 || id > 10) id = 0;

        foreach (var note in styles[id])
        {
            int dt = Mathf.RoundToInt(note.Item1 * subdiv * node.duration);
            if (dt < next)
                push(dt, root + note.Item2);
        }

    }
}
