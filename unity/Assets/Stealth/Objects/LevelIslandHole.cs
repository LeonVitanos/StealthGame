using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util.Algorithms.Triangulation;
using Util.Geometry.Polygon;

namespace Stealth.Objects
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(PolygonCollider2D))]
    public class LevelIslandHole : MonoBehaviour
    {
        private MeshFilter meshFilter;
        private PolygonCollider2D polyCollider;

        private void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            polyCollider = GetComponent<PolygonCollider2D>();
        }

        public void UpdateMesh(Polygon2D polygon, Mesh mesh)
        {
            if (meshFilter == null) meshFilter = GetComponent<MeshFilter>();
            if (polyCollider == null) polyCollider = GetComponent<PolygonCollider2D>();

            meshFilter.mesh = mesh;
            polyCollider.points = polygon.Vertices.ToArray();
        }
    }
}
