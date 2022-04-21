using System.Collections;
using System.Collections.Generic;
using Stealth.Objects;
using UnityEditor;
using UnityEngine;

namespace Stealth
{
    [CustomEditor(typeof(LevelIsland))]
    public class LevelIslandEditor : Editor
    {
        protected virtual void OnSceneGUI()
        {
            LevelIsland level = (LevelIsland)target;

            for (int i = 0; i < level.OutsideVertices.Length; i++)
            {
                Vector3 vertex = level.OutsideVertices[i];

                float size = HandleUtility.GetHandleSize(vertex) * 0.2f;
                Vector3 snap = Vector3.one * 0.5f;

                EditorGUI.BeginChangeCheck();
                Vector3 newPosition = Handles.FreeMoveHandle(vertex, Quaternion.identity, size, snap, Handles.CircleHandleCap);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(level, "Change vertex position");
                    level.OutsideVertices[i] = newPosition;
                    level.UpdateMesh();
                }
            }

            for (int i = 0; i < level.HolesBoundary.Length; i++)
            {
                LevelIsland.HoleBoundary hole = level.HolesBoundary[i];
                Vector2[] vertices = hole.Vertices;
                for (int j = 0; j < vertices.Length; j++)
                {
                    Vector3 vertex = vertices[j];

                    float size = HandleUtility.GetHandleSize(vertex) * 0.25f;
                    Vector3 snap = Vector3.one * 0.5f;

                    EditorGUI.BeginChangeCheck();
                    Vector3 newPosition = Handles.FreeMoveHandle(vertex, Quaternion.identity, size, snap, Handles.RectangleHandleCap);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(level, "Change vertex position");
                        hole.Vertices[j] = newPosition;
                        level.UpdateMesh();
                    }
                }
            }
        }
    }
}