using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Helpers;
using UnityEngine;

namespace Assets
{
    public enum RoadType
    {
        Highway,
        MajorRoad,
        MinorRoad,
        Rail,
        Path
    }

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    internal class RoadPolygon : MonoBehaviour
    {
        private Tile _tile;
        public string Id { get; set; }
        public RoadType Type { get; set; }
        private List<Vector3> _verts;

        public void Initialize(string id, Tile tile, List<Vector3> verts, string halfWidth)
        {
            Id = id;
            _tile = tile;
            Type = halfWidth.ToRoadType();
            _verts = verts;
        }

        private void Update()
        {
            for (int i = 1; i < _verts.Count; i++)
            {
                Debug.DrawLine(_tile.transform.position + _verts[i], _tile.transform.position + _verts[i - 1]);
            }

        }
    }
}
