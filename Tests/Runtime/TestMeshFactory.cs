using System.Collections.Generic;
using MathNet.Spatial.Euclidean;
using UnityEngine;

namespace Titan.Windowfracture.Tests
{
    internal static class TestMeshFactory
    {
        public static Mesh CreateLineMesh(params Vector3[] points)
        {
            var mesh = new Mesh { name = "TestLineMesh" };
            var vertices = new List<Vector3>(points);
            var indices = new List<int>(points.Length);
            for (int i = 0; i < points.Length; i++)
                indices.Add(i);

            mesh.SetVertices(vertices);
            mesh.SetIndices(indices, MeshTopology.Lines, 0);
            return mesh;
        }

        public static Polygon2D CreatePolygon(params Vector2[] points)
        {
            var polyPoints = new List<Point2D>(points.Length);
            foreach (var point in points)
                polyPoints.Add(new Point2D(point.x, point.y));
            return new Polygon2D(polyPoints);
        }

        public static List<LineSegment2D> CreateSegments(params Vector2[] points)
        {
            var segments = new List<LineSegment2D>(points.Length / 2);
            for (int i = 0; i < points.Length; i += 2)
            {
                segments.Add(new LineSegment2D(
                    new Point2D(points[i].x, points[i].y),
                    new Point2D(points[i + 1].x, points[i + 1].y)));
            }

            return segments;
        }
    }
}
