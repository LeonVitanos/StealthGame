using System;
using UnityEngine;
using Util.Geometry;

namespace Stealth.Utils
{
    public class Endpoint : IComparable<Endpoint>, IEquatable<Endpoint>
    {
        public Vector2 Vertex;

        public bool IsBegin;

        public float AngleToViewpoint;

        public LineSegment Segment;

        public override string ToString()
        {
            return $"{{ {nameof(Vertex)}: {Vertex}, " +
                $"{nameof(IsBegin)}: {IsBegin}, " +
                $"{nameof(AngleToViewpoint)}: {AngleToViewpoint}, " +
                $"{nameof(Segment)}: {Segment} }}";
        }

        public int CompareTo(Endpoint other)
        {
            if (other == null) return 1;

            if (Equals(other)) return 0;

            if (AngleToViewpoint != other.AngleToViewpoint)
            {
                return (int)Mathf.Sign(AngleToViewpoint - other.AngleToViewpoint);
            }

            // Begin endpoints should come before end endpoints
            if (IsBegin && !other.IsBegin)
            {
                return -1;
            }
            if (!IsBegin && other.IsBegin)
            {
                return 1;
            }

            return 0;
        }

        public bool Equals(Endpoint other)
        {
            return Vertex.Equals(other.Vertex) &&
                IsBegin == other.IsBegin &&
                AngleToViewpoint == other.AngleToViewpoint &&
                Segment == other.Segment;
        }
    }
}
