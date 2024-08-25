using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml;
using UnityEngine;

public class MapReader : MonoBehaviour
{
    [HideInInspector]
    public Dictionary<ulong, OsmNode> nodes;

    [HideInInspector]
    public List<OsmWay> ways;

    [HideInInspector]
    public OsmBounds bounds;

    public GameObject groundPlane;

    [Tooltip("The resource file that contains the OSM map data")]
    public string resourceFile;

    public bool IsReady { get; private set; }

    public void ReadMap()
    {
        nodes = new Dictionary<ulong, OsmNode>();
        ways = new List<OsmWay>();

        var txtAsset = Resources.Load<TextAsset>("MapData/" + resourceFile);

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(txtAsset.text);

        SetBounds(doc.SelectSingleNode("/osm/bounds"));
        GetNodes(doc.SelectNodes("/osm/node"));
        GetWays(doc.SelectNodes("/osm/way"));

        float minx = (float)MercatorProjection.lonToX(bounds.MinLon);
        float maxx = (float)MercatorProjection.lonToX(bounds.MaxLon);
        float miny = (float)MercatorProjection.latToY(bounds.MinLat);
        float maxy = (float)MercatorProjection.latToY(bounds.MaxLat);

        groundPlane.transform.localScale = new Vector3((maxx - minx) / 2, 1, (maxy - miny) / 2);

        IsReady = true;
    }

    void GetWays(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode node in xmlNodeList)
        {
            OsmWay way = new OsmWay(node);
            if (way.IsRoad == false)
                continue;

            ways.Add(way);
        }

        foreach (OsmWay way in ways)
        {
            foreach (OsmWay anotherWay in ways)
            {
                if (way == anotherWay)
                    continue;

                for (int i = 0; i < anotherWay.NodeIDs.Count; i++)
                {
                    // 시작점이 연결되어 있을때
                    if (way.NodeIDs[0] == anotherWay.NodeIDs[i])
                    {
                        if(i > 0)
                            way.StartRelationNode.Add(anotherWay.NodeIDs[i - 1]);
                        if(i < anotherWay.NodeIDs.Count - 1)
                            way.StartRelationNode.Add(anotherWay.NodeIDs[i + 1]);
                    }
                    // 끝나는점이 연결되어 있을때
                    else if (way.NodeIDs[way.NodeIDs.Count - 1] == anotherWay.NodeIDs[i])
                    {
                        if (i > 0)
                            way.EndRelationNode.Add(anotherWay.NodeIDs[i - 1]);
                        if (i < anotherWay.NodeIDs.Count - 1)
                            way.EndRelationNode.Add(anotherWay.NodeIDs[i + 1]);
                    }
                }
            }
        }
    }

    void GetNodes(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode n in xmlNodeList)
        {
            OsmNode node = new OsmNode(n);
            nodes[node.ID] = node;
        }
    }

    void SetBounds(XmlNode xmlNode)
    {
        bounds = new OsmBounds(xmlNode);
    }
}
