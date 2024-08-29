using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Xml;
using Unity.VisualScripting.FullSerializer;
using UnityEngine.UIElements;

/*
    Copyright (c) 2017 Sloan Kelly

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/

/// <summary>
/// An OSM object that describes an arrangement of OsmNodes into a shape or road.
/// </summary>
public class OsmWay : BaseOsm
{
    /// <summary>
    /// Way ID.
    /// </summary>
    public ulong ID { get; private set; }

    /// <summary>
    /// True if visible.
    /// </summary>
    public bool Visible { get; private set; }

    /// <summary>
    /// List of node IDs.
    /// </summary>
    public List<ulong> NodeIDs { get; private set; }

    /// <summary>
    /// True if the way is a boundary.
    /// </summary>
    public bool IsBoundary { get; private set; }

    /// <summary>
    /// True if the way is a road.
    /// </summary>
    public bool IsRoad { get; private set; }

    public bool IsWater { get; private set; }

    public bool IsForest { get; private set; }

    /// <summary>
    /// The name of the object.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// The number of lanes on the road. Default is 1 for contra-flow
    /// </summary>
    public int Lanes { get; private set; }

    public enum WaySizeEnum
    {
        Primary,
        Secondary,
        Tertiary,
        Residential
    }
    // 도로 크기의 3가지 단계
    public WaySizeEnum WaySize { get; private set; }

    public List<ulong> StartRelationNode { get; private set; } = new List<ulong>();
    public List<ulong> EndRelationNode { get; private set; } = new List<ulong>();

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="node"></param>
    public OsmWay(XmlNode node)
    {
        NodeIDs = new List<ulong>();
        Lanes = 1;      // Number of lanes either side of the divide 
        Name = "";

        // Get the data from the attributes
        ID = GetAttribute<ulong>("id", node.Attributes);
        Visible = GetAttribute<bool>("visible", node.Attributes);

        // Get the nodes
        XmlNodeList nds = node.SelectNodes("nd");
        foreach(XmlNode n in nds)
        {
            ulong refNo = GetAttribute<ulong>("ref", n.Attributes);
            NodeIDs.Add(refNo);
        }

        if (NodeIDs.Count > 1)
        {
            IsBoundary = NodeIDs[0] == NodeIDs[NodeIDs.Count - 1];
        }

        // Read the tags
        XmlNodeList tags = node.SelectNodes("tag");
        foreach (XmlNode t in tags)
        {
            string key = GetAttribute<string>("k", t.Attributes);
            string value = GetAttribute<string>("v", t.Attributes);
            if (key == "highway")
            {
                if (value != "footway" && value != "path" && value != "steps" && value != "service" && value != "pedestrian" && value != "corridor" && value != "living_street")
                {
                    switch (value)
                    {
                        case "primary": WaySize = WaySizeEnum.Primary; break;
                        case "secondary": WaySize = WaySizeEnum.Secondary; break;
                        case "tertiary": WaySize = WaySizeEnum.Tertiary; break;
                        case "residential": WaySize = WaySizeEnum.Residential; break;
                    }
                    IsRoad = true;
                }
            }
            else if (key=="lanes")
            {
                Lanes = GetAttribute<int>("v", t.Attributes);
            }
            else if (key=="name")
            {
                Name = GetAttribute<string>("v", t.Attributes);
            }
            else if(key == "natural")
            {
                if (value == "water")
                    IsWater = true;
                else if (value == "wood")
                    IsForest = true;
            }
            else if(key == "landuse")
            {
                if (value == "grass")
                    IsForest = true;
            }
            else if(key == "leisure")
            {
                if (value == "park")
                    IsForest = true;
            }
        }
    }
}

