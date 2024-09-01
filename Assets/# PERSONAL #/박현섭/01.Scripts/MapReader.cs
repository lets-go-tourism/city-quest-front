using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml;
using UnityEngine;

public class MapReader : MonoBehaviour
{
    public static MapReader Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    [HideInInspector]
    public Dictionary<ulong, OsmNode> nodes;

    [HideInInspector]
    public List<OsmWay> ways;

    public OsmBounds bounds;
    public Vector3 boundsCenter = Vector3.zero;

    public GameObject groundPlane;

    [Tooltip("The resource file that contains the OSM map data")]
    public string resourceFile;

    public bool IsReady { get; private set; }

    [Header("�� ������ ����")]
    public double mapSize = 1;

    private void Start()
    {
        Application.targetFrameRate = 120;
    }

    public void ReadMap()
    {
        MercatorProjection.Size = mapSize;

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

        boundsCenter = bounds.Center;
    }

    void GetWays(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode node in xmlNodeList)
        {
            OsmWay way = new OsmWay(node);
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
                    // �������� ����Ǿ� ������
                    if (way.NodeIDs[0] == anotherWay.NodeIDs[i])
                    {
                        if(i > 0)
                            way.StartRelationNode.Add(anotherWay.NodeIDs[i - 1]);
                        if(i < anotherWay.NodeIDs.Count - 1)
                            way.StartRelationNode.Add(anotherWay.NodeIDs[i + 1]);
                    }
                    // ���������� ����Ǿ� ������
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
