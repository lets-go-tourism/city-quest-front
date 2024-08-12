using System.Collections.Generic;
using System.Xml;

class OsmWay : BaseOsm
{
    public ulong ID { get; private set; }

    public bool visible { get; private set; }

    public List<ulong> NodeIDs { get; private set; }

    public bool isBoundary { get; private set; }
    public bool isBuilding { get; private set; }
    public bool isHighway { get; private set; }

    public OsmWay(XmlNode node)
    {
        NodeIDs = new List<ulong>();

        ID = GetAttribute<ulong>("id", node.Attributes);
        visible = GetAttribute<bool>("visible", node.Attributes);

        XmlNodeList nds = node.SelectNodes("nd");
        foreach (XmlNode n in nds)
        {
            ulong refNo = GetAttribute<ulong>("ref", n.Attributes);
            NodeIDs.Add(refNo);
        }

        // TO DO : Determine what type of way this is; is it a road / boundary etc.

        if (NodeIDs.Count > 1)
        {
            isBoundary = NodeIDs[0] == NodeIDs[NodeIDs.Count - 1];
        }

        XmlNodeList tags = node.SelectNodes("tag");
        foreach(XmlNode t in tags)
        {
            string key = GetAttribute<string>("k", t.Attributes);
            if(key == "highway")    // 길
            {
                isHighway = true;
            }
            //else if (key == " ")    // 녹지
            //{

            //}
            //else if(key == "")      // 물
            //{

            //}
            else if (key == "building")
            {
                isBuilding = GetAttribute<string>("v", t.Attributes) == "yes";
            }
        }
    }
}