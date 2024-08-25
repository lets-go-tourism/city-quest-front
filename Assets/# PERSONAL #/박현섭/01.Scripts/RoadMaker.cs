using System.Collections;
using System.Collections.Generic;
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

/// <summary>
/// Road infrastructure maker.
/// </summary>
public class RoadMaker : MonoBehaviour
{
    public Material roadMaterial;
    private MapReader map;

    public float primaryWidth = 5;
    public float secondaryWidth = 5;
    public float tertiaryWidth = 5;
    public float residentialWidth = 5;

    private GameObject parentObject;

    public void MakeRoad()
    {
        if (map == null)
            map = GetComponent<MapReader>();

        parentObject = new GameObject("OSM Map");

        foreach (var way in map.ways.FindAll((w) => { return w.IsRoad; }))
        {
            CreateObject(way, roadMaterial, way.Name);
        }
    }

    private void CreateObject(OsmWay way, Material mat, string objectName)
    {
        // Make sure we have some name to display
        objectName = string.IsNullOrEmpty(objectName) ? "OsmWay" : objectName;

        // Create an instance of the object and place it in the centre of its points
        GameObject go = new GameObject(objectName);
        go.transform.parent = parentObject.transform;
        Vector3 localOrigin = GetCenter(way);
        go.transform.position = localOrigin - map.bounds.Center;
        WayData s = go.AddComponent<WayData>();

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
            else if (i == points.Length - 1 && way.EndRelationNode.Count > 0)
            {
                float angle = 0;
                float maxAngle = 0;
                for (int j = 0; j < way.EndRelationNode.Count; j++)
                {
                    first = ((map.nodes[way.EndRelationNode[j]] - GetCenter(way)) - points[i]).normalized;
                    angle = Vector3.Angle(first, -second);

                    if (angle > maxAngle)
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

    private Vector3 GetCenter(OsmWay way)
    {
        Vector3 total = Vector3.zero;

        foreach (var id in way.NodeIDs)
        {
            total += map.nodes[id];
        }

        return total / way.NodeIDs.Count;
    }
}

