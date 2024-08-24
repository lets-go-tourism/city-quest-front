using System.Collections.Generic;
using System.Xml;
using UnityEngine;

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

// An OSM object that describes an arrangement of OsmNodes into a shape or road.
namespace 몰루
{
    public class OsmWay : BaseOsm
    {
        public enum WaySizeEnum
        {
            Primary,
            Secondary,
            Tertiary,
            Residential
        }
        // 도로 크기의 3가지 단계
        public WaySizeEnum WaySize { get; private set; }
        // Way ID.
        public ulong ID { get; private set; }
        // True if visible.
        public bool Visible { get; private set; }
        // Longitude position of the node.
        public List<ulong> NodeIDs { get; private set; }
        //True if the way is looping
        public bool IsLoop { get; private set; }
        //True if the way is a road
        public bool IsRoad { get; private set; }

        public OsmWay(XmlNode xmlNode, Dictionary<ulong, OsmNode> nodes)
        {
            NodeIDs = new List<ulong>();

            // Get the data from the attributes
            ID = GetAttribute<ulong>("id", xmlNode.Attributes);
            Visible = GetAttribute<bool>("visible", xmlNode.Attributes);

            // Get the nodes
            XmlNodeList xmlNodes = xmlNode.SelectNodes("nd");
            //Get the tags
            XmlNodeList tags = xmlNode.SelectNodes("tag");

            //Check if the way is road
            foreach (XmlNode t in tags)
            {
                string key = GetAttribute<string>("k", t.Attributes);
                string value = GetAttribute<string>("v", t.Attributes);

                if (key == "highway")
                {
                    if(value != "footway" && value != "path" && value != "steps" && value != "service" && value != "pedestrian" && value != "corridor")
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
            }


            if (IsRoad)
            {
                for (int i = 0; i < xmlNodes.Count; i++)
                {
                    //Reference ID to the node
                    ulong refNo = GetAttribute<ulong>("ref", xmlNodes[i].Attributes);
                    NodeIDs.Add(refNo);

                    if (i == 0 || i == xmlNodes.Count - 1)
                    {
                        // 길의 시작과 끝에 EndFlag를 True로 바꿈
                        if (nodes[refNo].NodeCount == 0)
                        {
                            nodes[refNo].EndFlag = true;
                        }
                        // 길의 시작과 끝이고 EndFlag가 True일 경우 만나는 지점이니까 NodeCount = -2로 바꾸고 끝이 아니니까 EndFlag = false로 바꾼다.
                        else if (nodes[refNo].NodeCount == 1 && nodes[refNo].EndFlag == true)
                        {
                            nodes[refNo].NodeCount = -2;
                            nodes[refNo].EndFlag = false;
                        }
                    }
                    // 시발 이거 어케 짯냐 벽느껴지네
                    // 길의 시작과 끝이 아닐 경우 NodeCount를 1 더한다
                    if (nodes[refNo].NodeCount == -1)
                    {
                        nodes[refNo].NodeCount = 3;
                    }
                    else
                    {
                        nodes[refNo].NodeCount += 1;
                    }
                }
                //Check if the way is looping
                if (NodeIDs.Count > 1)
                {
                    IsLoop = NodeIDs[0] == NodeIDs[NodeIDs.Count - 1];
                }
            }
        }
    }

}
