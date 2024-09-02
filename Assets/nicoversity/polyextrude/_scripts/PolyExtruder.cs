/*
 * PolyExtruder.cs
 *
 * Description: Class to render a custom polygon (2D) or prism (3D; simple extrusion of the polygon along the y-axis),
 * created through a generated (polygon) mesh using Triangulation.cs. Input vertices of the polygon are taken as a Vector2 array.
 * 
 * The class provides certain "quality-of-life" functions, such as for instance:
 * - calculating the area of the polygon surface
 * - calculating the centroid of the polygon
 * - automatic handling of correct surround mesh rendering, based on determination whether input vertices are ordered clockwise or counter-clockwise
 * 
 * Documentation:
 * - Prism 2D (is3D = false) = Polygon -> render created 2D mesh along x- and z-dimensions in the 3D space
 * - Prism 3D (is3D = true) = Extruded Polygon -> three components:
 *      - bottom mesh: = Prism 2D
 *      - top mesh: same as bottom mesh but at an increased position along the y-dimension
 *      - surround mesh: connecting bottom and top mesh along their outline vertices
 * - Outline Renderer = LineRenderer component based on the top most polygon, outlining the surface for better differentiation (for instance when placed next to other prisms)
 *
 * ATTENTION: No holes-support in polygon extrusion (Prism 3D) implemented!
 * Although Triangulation.cs supports "holes" in the 2D polygon, the support of holes as part of the PolyExtruder *has not* been implemented in this version.
 * 
 * 
 * === VERSION HISTORY | FEATURE CHANGE LOG ===
 * 2023-01-16:
 * - Minor bug fix: The updateColor() function in the PolyExtruder class considers now appropriately the coloring of the bottom mesh component
 *   depending on whether or not it exists in the 3D prism condition.
 * 2023-01-15:
 * - Minor bug fix: Appropriate type casting (double) of Vector2 values in calculateAreaAndCentroid() function.
 * - Modified the default material to utilize Unity's "Standard" shader instead of the legacy "Diffuse" shader.
 * 2023-01-13:
 * - Modified calculateAreaAndCentroid() function to internally utilize double (instead of float) type for area and centroid calculations.
 * - Added feature to conveniently indicate whether or not the bottom mesh component should be attached (only in Prism 3D, i.e., is3D = true).
 * - Added feature to conveniently indicate whether or not MeshCollider components should be attached.
 * 2023-01-06:
 * - Modified the mesh creation algorithm to use for each vertex the difference between original input vertex and calculated polygon centroid
     (instead of simply using the original input vertices). This way, the anchor of the generated mesh is correctly located at the coordinate
     system's origin (0,0), in turn enabling appropriate mesh manipulation at runtime (e.g., via Scale property in the GameObject's Transform).
 * - Unity version upgrade to support 2021.3.16f1 (from prior version 2019.2.17f1; no changes in code required).
 * 2021-02-17:
 * - Added feature to display a polygon's outline.
 * - Unity version upgrade to support 2019.2.17f1 (from prior version 2019.1.5f1; no changes in code required).
 * ============================================
 * 
 * 
 * Supported Unity version: 2021.3.16f1 Personal (tested)
 * 
 * Author: Nico Reski
 * Web: https://reski.nicoversity.com
 * Twitter: @nicoversity
 * GitHub: https://github.com/nicoversity
 * 
 */

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Class to create a (extruded) polygon based on a collection of vertices.
/// </summary>
public class PolyExtruder : MonoBehaviour
{
    #region Properties

    [Header("Prism Configuration")]
    public string prismName;                        // reference to name of the prism
    public Material mat;


    // private properties
    //

    // reference to extrusion height (Y axis)
    // Note: default y (height) of extruded polygon = 1.0f; default y (height) of bottom polygon = 0.0f;
    // -> scaling is applied using the GameObject transform's localScale y-value
    private static readonly float DEFAULT_BOTTOM_Y = 0.0f;
    private static readonly float DEFAULT_TOP_Y = 1.0f;

    // reference to original input vertices of Polygon in Vector2 Array format
    private Vector2[] originalPolygonVertices;

    // references to prism (polygon -> 2D; prism -> 3D) components
    private Mesh topMesh;
    private Mesh surroundMesh;
    private MeshRenderer bottomMeshRenderer;
    private MeshRenderer topMeshRenderer;
    private MeshRenderer surroundMeshRenderer;

    #endregion


    #region MeshCreator

    /// <summary>
    /// Create a prism based on the input parameters.
    /// </summary>
    /// <param name="prismName">Name of the prism within the Unity scene.</param>
    /// <param name="height">Height of the prism (= distance along the y-axis between bottom and top mesh).</param>
    /// <param name="vertices">Vector2 Array representing the input data of the polygon.</param>
    /// <param name="color">Color of the prism's material.</param>
    /// <param name="is3D">Set to<c>true</c> if polygon extrusion should be applied (= 3D prism), or <c>false</c> if it is only the (2D) polygon.</param>
    /// <param name="isUsingBottomMeshIn3D">Set to<c>true</c> if the bottom mesh component should be attached, or <c>false</c> if not.</param>
    /// <param name="isUsingColliders">Set to<c>true</c> if MeshCollider components should be attached, or <c>false</c> if not.</param>
    public void createPrism(string prismName, Vector2[] vertices, Material mat)
    {
        // set data
        this.prismName = name;
        this.originalPolygonVertices = vertices;
        this.mat = mat;

        // handle vertex order
        bool vertexOrderClockwise = areVerticesOrderedClockwise(this.originalPolygonVertices);
        if (!vertexOrderClockwise) System.Array.Reverse(this.originalPolygonVertices);

        // initialize meshes for prism
        initPrism();
    }

    /// <summary>
    /// Function to determine whether the input vertices are order clockwise or counter-clockwise.
    /// </summary>
    /// <returns>Returns <c>true</c> if vertices are ordered clockwise, and <c>false</c> if ordered counter-clockwise.</returns>
    /// <param name="vertices">Vector2 Array representing input vertices of the polygon.</param>
    private bool areVerticesOrderedClockwise(Vector2[] vertices)
    {
        // determine whether the order of vertices in the array is clockwise or counter-clockwise
        // this matters for the rendering of the 3D extruded surround mesh
        // implementation adapted via https://stackoverflow.com/a/1165943

        float edgesSum = 0.0f;
        for(int i = 0; i < vertices.Length; i++)
        {
            // handle last case
            if(i+1 == vertices.Length)
            {
                edgesSum = edgesSum + (vertices[0].x - vertices[i].x) * (vertices[0].y + vertices[i].y);
            }
            // handle normal case
            else
            {
                edgesSum = edgesSum + (vertices[i + 1].x - vertices[i].x) * (vertices[i + 1].y + vertices[i].y);
            }
        }

        // edges sum = positive -> clockwise
        // edges sum = negative -> counter-clockwise
        return (edgesSum >= 0.0f) ? true : false;
    }

    /// <summary>
    /// Function to calculate area and centroid of the polygon based on its input vertices.
    /// </summary>
    /// <param name="vertices">Vertices.</param>
    /// <returns>Returns <c>true</c> once the area and centroid of the polygon are set.</returns>
    private bool calculateAreaAndCentroid(Vector2[] vertices)
    {
        // calculate area and centroid of a polygon
        // implementation adapted via http://paulbourke.net/geometry/polygonmesh/

        // setup temporary variables for calculation
        double doubleArea = 0.0;
        double centroidX = 0.0;
        double centroidY = 0.0;

        // iterate through all vertices
        for (int i = 0; i < vertices.Length; i++)
        {
            // handle last case
            if (i + 1 == vertices.Length)
            {
                doubleArea = doubleArea + ((double)vertices[i].x * (double)vertices[0].y - (double)vertices[0].x * (double)vertices[i].y);
                centroidX = centroidX + (((double)vertices[i].x + (double)vertices[0].x) * ((double)vertices[i].x * (double)vertices[0].y - (double)vertices[0].x * (double)vertices[i].y));
                centroidY = centroidY + (((double)vertices[i].y + (double)vertices[0].y) * ((double)vertices[i].x * (double)vertices[0].y - (double)vertices[0].x * (double)vertices[i].y));
            }
            // handle normal case
            else
            {
                doubleArea = doubleArea + ((double)vertices[i].x * (double)vertices[i + 1].y - (double)vertices[i + 1].x * (double)vertices[i].y);
                centroidX = centroidX + (((double)vertices[i].x + (double)vertices[i + 1].x) * ((double)vertices[i].x * (double)vertices[i + 1].y - (double)vertices[i + 1].x * (double)vertices[i].y));
                centroidY = centroidY + (((double)vertices[i].y + (double)vertices[i + 1].y) * ((double)vertices[i].x * (double)vertices[i + 1].y - (double)vertices[i + 1].x * (double)vertices[i].y));
            }
        }

        // set area
        double polygonArea = (doubleArea < 0) ? doubleArea * -0.5 : doubleArea * 0.5;
        //this.polygonArea = (float)polygonArea;

        // set centroid
        double sixTimesArea = doubleArea * 3.0;
        //this.polygonCentroid = new Vector2((float)(centroidX / sixTimesArea), (float)(centroidY / sixTimesArea));

        // return statement (indicating that area and centroid have been set)
        return true;
    }


    private MeshFilter mfB;
    /// <summary>
    /// Function to initialize and setup the (meshes of the) prism based on the the PolyExtruder's set properties.
    /// </summary>
    private void initPrism()
    {
        // 1. BOTTOM MESH
        //

        // create bottom GameObject with required components
        GameObject goB = new GameObject();
        goB.transform.parent = this.transform;
        goB.transform.localPosition = Vector3.zero;
        goB.name = "bottom_" + this.prismName;
        mfB = goB.AddComponent<MeshFilter>();
        bottomMeshRenderer = goB.AddComponent<MeshRenderer>();
        bottomMeshRenderer.material = mat;

        // init helper values to create bottom mesh
        List<Vector2> pointsB = new List<Vector2>();
        List<List<Vector2>> holesB = new List<List<Vector2>>();
        List<int> indicesB = null;
        List<Vector3> verticesB = null;

        // convert original polygon data for bottom mesh
        //foreach (Vector2 v in originalPolygonVertices)
        //{
        //    pointsB.Add(v - polygonCentroid);   // consider calculated polygon centroid as anchor at the coordinate system's origin (0,0) for appropriate mesh manipulations at runtime (e.g., scaling)
        //    //pointsB.Add(v);                   // use original input vertices as is
        //}
        // perform TRIANGULATION
        Triangulation.triangulate(originalPolygonVertices.ToList(), holesB, DEFAULT_BOTTOM_Y, out indicesB, out verticesB);

        // assign indices and vertices and create mesh
        redrawMesh(verticesB, indicesB);
    }

    /// <summary>
    /// Function to redraw the mesh (for instance after it was updated).
    /// </summary>
    /// <param name="mesh">Reference to mesh component that needs to be redrawn.</param>
    /// <param name="vertices">Vector3 list of the the meshes vertices.</param>
    /// <param name="indices">Int list of the meshes vertex indices.</param>
    private void redrawMesh(List<Vector3> vertices, List<int> indices)
    {
        Mesh mesh = new Mesh();
        // clear prior mesh information
        mesh.Clear();

        // assign vertices and indices representing the mesh
        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();

        // sync mesh information
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        mfB.mesh = mesh;
    }

    #endregion
}
