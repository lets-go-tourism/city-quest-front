using System.Collections;
using System.Collections.Generic;
using TriangleNet.Geometry;
using UnityEngine;

public class DistanceUpdate : MonoBehaviour
{
    void Start()
    {
        InvokeRepeating("YourFunction", 5f, 1f);
    }


}
