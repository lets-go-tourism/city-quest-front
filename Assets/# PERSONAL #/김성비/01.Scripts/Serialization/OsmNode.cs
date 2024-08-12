using System.Xml;
using UnityEngine;

class OsmNode : BaseOsm
{
    public ulong ID { get; private set; }

    public float Latitude { get; private set; }
    public float Longitude { get; private set; }

    public float x { get; private set; }
    public float y { get; private set; }

    public static implicit operator Vector3(OsmNode node)
    {
        return new Vector3(node.x, 0, node.y);
    }

    public OsmNode(XmlNode node)
    {
        ID = GetAttribute<ulong>("id", node.Attributes);
        Latitude = GetAttribute<float>("lat", node.Attributes);
        Longitude = GetAttribute<float>("lon", node.Attributes);

        x = (float)MercatorProjection.lonToX(Longitude);
        y = (float)MercatorProjection.latToY(Latitude);
    }
}