using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class GameSerializer
{
    public static bool Save(string file, Dictionary<int, Node> nodes, Dictionary<int, SortedSet<int>> edges)
    {
        XmlWriter writer = XmlWriter.Create(file);
        writer.WriteStartDocument();
        writer.WriteStartElement("graph");

        //writer.WriteStartElement("nodes");
        foreach (KeyValuePair<int, Node> e in nodes)
        {
            writer.WriteStartElement("node");
            writer.WriteAttributeString("id", e.Value.id.ToString());
            writer.WriteAttributeString("note", e.Value.note.ToString());
            writer.WriteAttributeString("duration", e.Value.duration.ToString());

            var pos = e.Value.gameObject.transform.position;
            writer.WriteAttributeString("position", string.Format("{0}, {1}, {2}", pos.x, pos.y, pos.z));

            writer.WriteStartElement("decoration");
            writer.WriteAttributeString("type", e.Value.socket.type.ToString());
            writer.WriteAttributeString("id", e.Value.socket.id.ToString());
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
        //writer.WriteEndElement();


        //writer.WriteStartElement("edges");
        foreach (KeyValuePair<int, SortedSet<int>> e in edges)
        {
            foreach (int to in e.Value)
            {
                writer.WriteStartElement("edge");
                writer.WriteAttributeString("from", e.Key.ToString());
                writer.WriteAttributeString("to", to.ToString());
                writer.WriteEndElement();
            }
        }
        //writer.WriteEndElement();

        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Close();
        return true;
    }

    public delegate GameObject CreateNodeFn(Vector3 pos);
    public delegate GameObject CreateEdgeFn(GameObject from, GameObject to);

    public static bool Load(string file, CreateNodeFn AddNode, CreateEdgeFn AddEdge)
    {
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.DtdProcessing = DtdProcessing.Parse;
        XmlReader reader = XmlReader.Create(file, settings);

        Dictionary<int, GameObject> nodes = new Dictionary<int, GameObject>();
        GameObject current = null;

        reader.MoveToContent();
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    switch (reader.Name)
                    {
                        case "node":
                            {
                                int id = System.Int32.Parse(reader.GetAttribute("id"));
                                int note = System.Int32.Parse(reader.GetAttribute("note"));
                                float duration = float.Parse(reader.GetAttribute("duration"));
                                string[] p = reader.GetAttribute("position").Split(',');
                                Vector3 position = new Vector3(float.Parse(p[0]), float.Parse(p[1]), float.Parse(p[2]));

                                nodes[id] = AddNode(position);
                                current = nodes[id];
                                var node = current.GetComponent<Node>();
                                node.note = note;
                                node.duration = duration;
                            }
                            break;
                        case "edge":
                            {
                                int from = System.Int32.Parse(reader.GetAttribute("from"));
                                int to = System.Int32.Parse(reader.GetAttribute("to"));

                                AddEdge(nodes[from], nodes[to]);
                            }
                            break;
                        case "decoration":
                            if (current)
                            {
                                int id = System.Int32.Parse(reader.GetAttribute("id"));
                                string type = reader.GetAttribute("type");
                                Node n = current.GetComponent<Node>();
                                n.socket.type = (Decoration.DecorationType)System.Enum.Parse(typeof(Decoration.DecorationType), type);
                                n.socket.id = id;
                            }
                            break;
                    }
                    break;
                case XmlNodeType.EndElement:
                    if (reader.Name == "node") current = null;
                    break;
            }
        }
        return false;
    }
}
