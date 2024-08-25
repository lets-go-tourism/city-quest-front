using System.Collections.Generic;
using UnityEngine;
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
/// Base infrastructure creator.
/// </summary>
[RequireComponent(typeof(MapReader))]
public abstract class InfrastructureBehaviour : MonoBehaviour
{
    /// <summary>
    /// The map reader object; contains all the data to build procedural geometry.
    /// </summary>
    protected MapReader map;

    /// <summary>
    /// Get the centre of an object or road.
    /// </summary>
    /// <param name="way">OsmWay object</param>
    /// <returns>The centre point of the object</returns>
    protected Vector3 GetCenter(OsmWay way)
    {
        Vector3 total = Vector3.zero;

        foreach (var id in way.NodeIDs)
        {
            total += map.nodes[id];
        }

        return total / way.NodeIDs.Count;
    }

    /// <summary>
    /// Procedurally generate an object from the data given in the OsmWay instance.
    /// </summary>
    /// <param name="way">OsmWay instance</param>
    /// <param name="mat">Material to apply to the instance</param>
    /// <param name="objectName">The name of the object (building name, road etc.)</param>
    protected void CreateObject(OsmWay way, Material mat, string objectName)
    {
        // Make sure we have some name to display
        objectName = string.IsNullOrEmpty(objectName) ? "OsmWay" : objectName;

        // Create an instance of the object and place it in the centre of its points
        GameObject go = new GameObject(objectName);
        Vector3 localOrigin = GetCenter(way);
        go.transform.position = localOrigin - map.bounds.Center;
        시발 s = go.AddComponent<시발>();
        s.start = way.StartRelationNode;
        s.end = way.EndRelationNode;

        // Add the mesh filter and renderer components to the object
        MeshFilter mf = go.AddComponent<MeshFilter>();
        MeshRenderer mr = go.AddComponent<MeshRenderer>();

        // Apply the material
        mr.material = mat;

        // Create the collections for the object's vertices, indices, UVs etc.
        List<Vector3> vectors = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> indices = new List<int>();

        // Call the child class' object creation code
        //OnObjectCreated(way, localOrigin, vectors, normals, uvs, indices);
        float roadWidth = 0;
        switch (way.WaySize)
        {
            case OsmWay.WaySizeEnum.Primary:
                roadWidth = primaryWidth;
                break;
            case OsmWay.WaySizeEnum.Secondary:
                roadWidth = secondaryWidth;
                break;
            case OsmWay.WaySizeEnum.Tertiary:
                roadWidth = tertiaryWidth;
                break;
            case OsmWay.WaySizeEnum.Residential:
                roadWidth = residentialWidth;
                break;
        }
        mf.mesh = CreateRoadMesh(way, roadWidth);

        // Apply the data to the mesh
        //mf.mesh.vertices = vectors.ToArray();
        //mf.mesh.normals = normals.ToArray();
        //mf.mesh.triangles = indices.ToArray();
        //mf.mesh.uv = uvs.ToArray();
    }
    public float primaryWidth = 5;
    public float secondaryWidth = 5;
    public float tertiaryWidth = 5;
    public float residentialWidth = 5;

    protected abstract void OnObjectCreated(OsmWay way, Vector3 origin, List<Vector3> vectors, List<Vector3> normals, List<Vector2> uvs, List<int> indices);
    private Mesh TestCreateRoadMesh(OsmWay way, float roadWidth)
    {
        Vector3[] points = new Vector3[way.NodeIDs.Count];
        Vector3[] verts = new Vector3[points.Length * 2];
        //Vector2[] uvs = new Vector2[verts.Length];

        for (int i = 0; i < way.NodeIDs.Count; i++)
        {
            points[i] = map.nodes[way.NodeIDs[i]] - GetCenter(way);
        }

        int[] tris = new int[2 * (points.Length - 1) * 3];
        int vertIndex = 0;
        int triIndex = 0;

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 forward = Vector3.zero;

            // 처음 점
            if(i == 0)
            {

            }
            // 마지막 점
            else if(i == points.Length - 1)
            {

            }
            // 마지막과 처음점 제외하고 전부
            else
            {
                forward += points[i + 1] - points[i];
                forward += points[i] - points[i - 1];

                forward.Normalize();
                Vector3 left = new Vector3(-forward.z, 0, forward.x);

                verts[vertIndex] = points[i] + left * roadWidth * .5f;
                verts[vertIndex + 1] = points[i] - left * roadWidth * .5f;
            }

            //float completionPercent = i / (float)(points.Length - 1);

            //uvs[vertIndex] = new Vector2(0, completionPercent);
            //uvs[vertIndex + 1] = new Vector2(1, completionPercent);

            if (i < points.Length - 1)
            {
                tris[triIndex] = vertIndex;
                tris[triIndex + 1] = vertIndex + 2;
                tris[triIndex + 2] = vertIndex + 1;

                tris[triIndex + 3] = vertIndex + 1;
                tris[triIndex + 4] = vertIndex + 2;
                tris[triIndex + 5] = vertIndex + 3;
            }

            vertIndex += 2;
            triIndex += 6;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        //mesh.uv = uvs;

        return mesh;
    }
    private Mesh CreateRoadMesh(OsmWay way, float roadWidth)
    {
        Vector3[] points = new Vector3[way.NodeIDs.Count];
        Vector3[] verts = new Vector3[points.Length * 2];
        //Vector2[] uvs = new Vector2[verts.Length];

        for (int i = 0; i < way.NodeIDs.Count; i++)
        {
            points[i] = map.nodes[way.NodeIDs[i]] - GetCenter(way);
        }

        int[] tris = new int[2 * (points.Length - 1) * 3];
        int vertIndex = 0;
        int triIndex = 0;

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 forward = Vector3.zero;
            Vector3 first = Vector3.zero;
            Vector3 second = Vector3.zero;
            // 마지막 점 제외 전부
            if (i < points.Length - 1)
            {
                first = (points[i + 1] - points[i]).normalized;
            }
            // 처음 제외 전부
            if (i > 0)
            {
                second = (points[i] - points[i - 1]).normalized;
            }

            int maxAngleIndex = 0;
            // 처음에만
            if (i == 0 && way.StartRelationNode.Count > 0)
            {
                float angle = 0;
                float maxAngle = 0;
                for (int j = 0; j < way.StartRelationNode.Count; j++)
                {
                    second = (points[i] - (map.nodes[way.StartRelationNode[j]] - GetCenter(way))).normalized;
                    angle = Vector3.Angle(first, -second);

                    if (angle > maxAngle)
                    {
                        maxAngle = angle;
                        maxAngleIndex = j;
                    }
                }
                second = (points[i] - (map.nodes[way.StartRelationNode[maxAngleIndex]] - GetCenter(way))).normalized;
            }
            //마지막만
            else if(i == points.Length - 1 && way.EndRelationNode.Count > 0)
            {
                float angle = 0;
                float maxAngle = 0;
                for (int j = 0; j < way.EndRelationNode.Count; j++)
                {
                    first = ((map.nodes[way.EndRelationNode[j]] - GetCenter(way)) - points[i]).normalized;
                    angle = Vector3.Angle(first, -second);

                    if(angle > maxAngle)
                    {
                        maxAngle = angle;
                        maxAngleIndex = j;
                    }
                }
                first = ((map.nodes[way.EndRelationNode[maxAngleIndex]] - GetCenter(way)) - points[i]).normalized;
            }

            forward += first + second;

            float result = roadWidth;
            if (i != 0 || i != points.Length - 1)
            {
                float angle = Vector3.Angle(first, -second) * .5f;

                if (angle != 0)
                    result = roadWidth / Mathf.Sin(angle * Mathf.Deg2Rad);
            }

            forward.Normalize();
            Vector3 left = new Vector3(-forward.z, 0, forward.x).normalized;

            verts[vertIndex] = points[i] + left * result * .5f;
            verts[vertIndex + 1] = points[i] - left * result * .5f;

            //float completionPercent = i / (float)(points.Length - 1);

            //uvs[vertIndex] = new Vector2(0, completionPercent);
            //uvs[vertIndex + 1] = new Vector2(1, completionPercent);

            if (i < points.Length - 1)
            {
                tris[triIndex] = vertIndex;
                tris[triIndex + 1] = vertIndex + 2;
                tris[triIndex + 2] = vertIndex + 1;

                tris[triIndex + 3] = vertIndex + 1;
                tris[triIndex + 4] = vertIndex + 2;
                tris[triIndex + 5] = vertIndex + 3;
            }

            vertIndex += 2;
            triIndex += 6;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        //mesh.uv = uvs;

        return mesh;
    }
}

public class IntersectionData
{
    public enum Type
    {
        One,
        Two,
    }

    public Type type;

    public ulong node1;
    public ulong node2;
    public ulong node3;

    public IntersectionData(Type type, ulong node1, ulong node2, ulong node3)
    {
        this.type = type;
        this.node1 = node1;
        this.node2 = node2;
        this.node3 = node3;
    }

    public IntersectionData(Type type, ulong node1, ulong node2)
    {
        this.type = type;
        this.node1 = node1;
        this.node2 = node2;
    }
}
