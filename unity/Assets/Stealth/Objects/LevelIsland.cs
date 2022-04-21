using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util.Algorithms.Triangulation;
using Util.Geometry.Polygon;

namespace Stealth.Objects
{
    /// <summary>
    /// Represents the level polygon.
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(EdgeCollider2D))]
    public class LevelIsland : MonoBehaviour
    {   
        [SerializeField]
        private Vector2[] outsideVertices = new Vector2[]
        {
            new Vector2(-0.5f, -0.5f),
            new Vector2( 0.5f, -0.5f),
            new Vector2( 0.5f,  0.5f),
            new Vector2(-0.5f,  0.5f),
        };

        public Vector2[] OutsideVertices
        {
            get { return outsideVertices; }
            set { outsideVertices = value; }
        }

        [SerializeField]
        private HoleBoundary[] holesBoundary = new HoleBoundary[] { };

        public HoleBoundary[] HolesBoundary
        {
            get { return holesBoundary; }
            set { holesBoundary = value; }
        }

        [System.Serializable]
        public class HoleBoundary
        {
            public Vector2[] Vertices;
        }

        [SerializeField]
        private LevelIslandHole holePrefab;

        [SerializeField]
        private DebugSettings debugSettings = new DebugSettings()
        {
            OutsideColor = Color.white,
            HolesColor = Color.gray,
            VertexGizmoRadius = 0.1f
        };

        [System.Serializable]
        private class DebugSettings
        {
            public Color OutsideColor;
            public Color HolesColor;
            public float VertexGizmoRadius;
        }

        public Polygon2DWithHoles Polygon { get; private set; }

        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;

        private Mesh outsideMesh;
        private Mesh[] holesMesh;

        private EdgeCollider2D edgeCollider;

        private void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();

            edgeCollider = GetComponent<EdgeCollider2D>();

            UpdateMesh();
            meshRenderer.enabled = true;
        }

        [ContextMenu("Update mesh")]
        public void UpdateMesh()
        {
            // We might be in edit mode, so retrieve any components we don't have a reference to
            if (meshFilter == null) meshFilter = GetComponent<MeshFilter>();
            if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();
            if (edgeCollider == null) edgeCollider = GetComponent<EdgeCollider2D>();

            // Create polygons for outside boundary and holes
            Polygon2D outsidePolygon = new Polygon2D(outsideVertices);
            Polygon2D[] holesPolygon = holesBoundary.Select(hole => new Polygon2D(hole.Vertices)).ToArray();

            // Create polygon for level
            Polygon = new Polygon2DWithHoles(outsidePolygon, holesPolygon);

            // Create meshes for outside and holes
            outsideMesh = Triangulator.Triangulate(Polygon.Outside, false).CreateMesh();
            outsideMesh.RecalculateNormals();

            holesMesh = holesPolygon.Select(hole => Triangulator.Triangulate(hole, false).CreateMesh()).ToArray();
            foreach (Mesh hole in holesMesh)
            {
                hole.RecalculateNormals();
            }

            List<Vector2> points = outsideVertices.ToList();
            points.Add(outsideVertices[0]);

            Vector2[] colliderPoints = new Vector2[outsideVertices.Length + 1];
            for (int i = 0; i < outsideVertices.Length; i++)
            {
                colliderPoints[i] = outsideVertices[i];
            }
            colliderPoints[colliderPoints.Length - 1] = outsideVertices[0];
            edgeCollider.points = colliderPoints;

            // Update mesh filters if we're in play mode
            if (Application.isPlaying)
            {
                // Assign mesh to mesh filter
                meshFilter.mesh = outsideMesh;

                // Destroy all current hole objects
                foreach (Transform child in transform)
                {
                    if (child.gameObject == holePrefab.gameObject) continue;
                    // Assume all children are hole objects
                    Destroy(child.gameObject);
                }
                // Create enough hole objects
                for (int i = 0; i < holesMesh.Length; i++)
                {
                    // Instantiate as child of self
                    LevelIslandHole instance = Instantiate(holePrefab, transform);
                    instance.gameObject.SetActive(true);
                    instance.UpdateMesh(holesPolygon[i], holesMesh[i]);
                }
            }
            else
            {
                // If we're in edit mode, disable the mesh renderer,
                // we'll draw the meshes using gizmos
                meshRenderer.enabled = false;
            }
        }

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                // Check that no two consecutive vertices have the same coordinates
                for (int i = 0; i + 1 < outsideVertices.Length; i++)
                {
                    if (Vector2.Distance(outsideVertices[i], outsideVertices[i + 1]) < Mathf.Epsilon)
                    {
                        // Move the second vertex inbetween the previous and next vertex
                        Vector2 prev = outsideVertices[i];
                        Vector2 next = (i + 2 < outsideVertices.Length) ? outsideVertices[i + 2] : outsideVertices[0];

                        outsideVertices[i + 1] = Vector2.Lerp(prev, next, 0.5f);
                    }
                }
                foreach (HoleBoundary hole in holesBoundary)
                {
                    if (hole.Vertices.Length == 0)
                    {
                        // Add some default vertices
                        hole.Vertices = new Vector2[]
                        {
                            new Vector2(-0.25f, -0.25f),
                            new Vector2( 0.25f, -0.25f),
                            new Vector2( 0.25f,  0.25f),
                            new Vector2(-0.25f,  0.25f),
                        };
                    }
                    else
                    {
                        // Check that no two consecutive vertices have the same coordinates
                        for (int i = 0; i + 1 < hole.Vertices.Length; i++)
                        {
                            if (Vector2.Distance(hole.Vertices[i], hole.Vertices[i + 1]) < Mathf.Epsilon)
                            {
                                // Move the second vertex inbetween the previous and next vertex
                                Vector2 prev = hole.Vertices[i];
                                Vector2 next = (i + 2 < hole.Vertices.Length) ? hole.Vertices[i + 2] : hole.Vertices[0];

                                hole.Vertices[i + 1] = Vector2.Lerp(prev, next, 0.5f);
                            }
                        }
                    }
                }
                UpdateMesh();
            }
        }

        private void OnDrawGizmos()
        {
            if (outsideMesh != null)
            {
                // Draw vertices of outside boundary
                Gizmos.color = debugSettings.OutsideColor;
                Gizmos.DrawWireMesh(outsideMesh, transform.position);
            }

            // Draw edges of holes
            Gizmos.color = debugSettings.HolesColor;
            foreach (Mesh hole in holesMesh)
            {
                if (hole.vertices.Length < 3) continue;
                Gizmos.DrawMesh(hole, transform.position);
            }
        }
    }
}