using Assets.Helpers;
//using Dev.ComradeVanti.EarClip;                           
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnvironmentMaker : MonoBehaviour
{
    [SerializeField] private Material waterMaterial;
    [SerializeField] private Material forestMaterial;

    private MapReader map;
    private GameObject parentObject;

    public void MakeEnvironment()
    {
        if (map == null)
            map = GetComponent<MapReader>();

        parentObject = new GameObject("OSM Environment Map");

        foreach (var way in map.ways.FindAll((w) => { return w.IsWater; }))
        {
            CreateObject(way, waterMaterial, way.Name);
        }
        foreach (var way in map.ways.FindAll((w)=> { return w.IsForest; }))
        {
            CreateObject(way, forestMaterial, way.Name);
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
        go.transform.position += new Vector3(0, 1f, 0);

        // Add the mesh filter and renderer components to the object
       // MeshFilter mf = go.AddComponent<MeshFilter>();
        //MeshRenderer mr = go.AddComponent<MeshRenderer>();
        PolyExtruder polyExtruder = go.AddComponent<PolyExtruder>();

        Vector3[] points = new Vector3[way.NodeIDs.Count - 1];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = map.nodes[way.NodeIDs[i]] - GetCenter(way);
        }

        Vector2[] points2 = new Vector2[way.NodeIDs.Count - 1];
        for (int i = 0; i < points2.Length; i++)
        {
            points2[i] = new Vector2(points[i].x, points[i].z);
        }

        polyExtruder.createPrism(objectName, points2, mat);

        // Apply the material
       // mr.material = mat;

        // Create the collections for the object's vertices, indices, UVs etc.
        //mf.mesh = CreateMesh(way);

        // Apply the data to the mesh
        //mf.mesh.vertices = vectors.ToArray();
        //mf.mesh.normals = normals.ToArray();
        //mf.mesh.triangles = indices.ToArray();
        //mf.mesh.uv = uvs.ToArray();
    }
    private Mesh CreateMesh(OsmWay way)
    {
        // points need to be in clockwise order
        // sort if needed
        //points2 = points2.Clockwise().ToArray();

        //obj.transform.position = points2[0].ToVector3xz();
        //obj2.transform.position = points2[points2.Length - 1].ToVector3xz();

        // Triangulate
        //int[] triangles = Triangulate.ConcaveNoHoles(points2).ToArray();                  

        //int[] triangles = new int[(points.Length - 2) * 3];

        //for (int i = 0; i < points.Length - 2; i++)
        //{
        //    triangles[i * 3] = 0;
        //    triangles[i * 3 + 1] = i + 1;
        //    triangles[i * 3 + 2] = i + 2;
        //}
        Mesh mesh = new Mesh();
        //mesh.vertices = points;
        //mesh.triangles = triangles;                               
        //mesh.RecalculateNormals();
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
