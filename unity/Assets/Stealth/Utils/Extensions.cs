using System;
using System.Linq;
using UnityEngine;
using Util.Geometry;
using Util.Geometry.Polygon;

namespace Stealth.Utils
{
    public static class Extensions
    {
        /// <summary>
        /// Shortens a line segment to <c>1 - <paramref name="amount"/></c> its length towards the middle.
        /// </summary>
        /// <param name="segment">The segment to shorten.</param>
        /// <param name="amount">The amount to shorten the line segment by.</param>
        /// <returns></returns>
        public static LineSegment Shorten(this LineSegment segment, float amount = 0.1f)
        {
            if (amount >= 1)
            {
                throw new ArgumentException($"{nameof(amount)} must be smaller than 1");
            }

            float magnitude = segment.Magnitude;
            Vector2 center = segment.Point1 + (segment.Point2 - segment.Point1) * 0.5f;
            Vector2 newP1 = segment.Point1 + (center - segment.Point1).normalized * 0.5f * amount * magnitude;
            Vector2 newP2 = segment.Point2 + (center - segment.Point2).normalized * 0.5f * amount * magnitude;

            return new LineSegment(newP1, newP2);
        }

        /// <summary>
        /// Transforms all vertices of a polygon to the local space of a <see cref="Transform"/>.
        /// </summary>
        /// <param name="polygon">The polygon to transform.</param>
        /// <param name="transform">The transform expressing the local space.</param>
        /// <returns></returns>
        public static Polygon2D ToLocalSpace(this Polygon2D polygon, Transform transform)
        {
            return new Polygon2D(polygon.Vertices.Select(vertex =>
            {
                Vector2 result = transform.InverseTransformPoint(vertex);
                return result;
            }));
        }
    }
}
