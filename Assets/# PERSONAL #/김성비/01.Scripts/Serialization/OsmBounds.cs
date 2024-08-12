using System.Xml;
using UnityEngine;

class OsmBounds : BaseOsm
{
    public float minLat { get; private set; }
    
    public float maxLat { get; private set; }
   
    public float minLon { get; private set; }
    
    public float maxLon { get; private set; }

    public Vector3 Centre { get; private set; }

    public OsmBounds(XmlNode node) 
    {
        minLat = GetAttribute<float>("minlat", node.Attributes);
        maxLat = GetAttribute<float>("maxlat", node.Attributes);
        minLon = GetAttribute<float>("minlon", node.Attributes);
        maxLon = GetAttribute<float>("maxlon", node.Attributes);

        float x = (float)((MercatorProjection.lonToX(maxLon) + MercatorProjection.lonToX(minLon)) / 2);
        float y = (float)((MercatorProjection.latToY(maxLat) + MercatorProjection.latToY(minLat)) / 2);

        Centre = new Vector3(x, 0, y);
    }
}