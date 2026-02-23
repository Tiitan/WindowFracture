using System.Collections.Generic;
using MathNet.Spatial.Euclidean;
using NUnit.Framework;
using UnityEngine;
using WindowFracture.Runtime;

namespace Titan.Windowfracture.Tests
{
    public class ClipPatternTests
    {
        [Test]
        public void Clip_KeepsSegmentInsideMask()
        {
            var mask = TestMeshFactory.CreatePolygon(
                new Vector2(0, 0),
                new Vector2(2, 0),
                new Vector2(2, 2),
                new Vector2(0, 2));
            var pattern = TestMeshFactory.CreateLineMesh(
                new Vector3(0.5f, 0.5f, 0f), new Vector3(1.5f, 0.5f, 0f));

            List<LineSegment2D> lines = ClipPattern.Clip(pattern, mask, Vector3.zero, 0f);

            Assert.That(ContainsSegment(lines, new Point2D(0.5, 0.5), new Point2D(1.5, 0.5), 1e-4), Is.True);
        }

        [Test]
        public void Clip_ClipsCrossingSegmentAtBoundary()
        {
            var mask = TestMeshFactory.CreatePolygon(
                new Vector2(0, 0),
                new Vector2(2, 0),
                new Vector2(2, 2),
                new Vector2(0, 2));
            var pattern = TestMeshFactory.CreateLineMesh(
                new Vector3(-1f, 1f, 0f), new Vector3(1f, 1f, 0f));

            List<LineSegment2D> lines = ClipPattern.Clip(pattern, mask, Vector3.zero, 0f);

            Assert.That(ContainsSegment(lines, new Point2D(0, 1), new Point2D(1, 1), 1e-4), Is.True);
        }

        [Test]
        public void Clip_DiscardsOutsideSegment()
        {
            var mask = TestMeshFactory.CreatePolygon(
                new Vector2(0, 0),
                new Vector2(2, 0),
                new Vector2(2, 2),
                new Vector2(0, 2));
            var pattern = TestMeshFactory.CreateLineMesh(
                new Vector3(-2f, -2f, 0f), new Vector3(-1f, -1f, 0f));

            List<LineSegment2D> lines = ClipPattern.Clip(pattern, mask, Vector3.zero, 0f);

            Assert.That(ContainsSegment(lines, new Point2D(-2, -2), new Point2D(-1, -1), 1e-4), Is.False);
        }

        [Test]
        public void Clip_AppliesRotationAndOffset()
        {
            var mask = TestMeshFactory.CreatePolygon(
                new Vector2(-3, -3),
                new Vector2(3, -3),
                new Vector2(3, 3),
                new Vector2(-3, 3));
            var pattern = TestMeshFactory.CreateLineMesh(
                new Vector3(0f, 0f, 0f), new Vector3(1f, 0f, 0f));

            List<LineSegment2D> lines = ClipPattern.Clip(pattern, mask, new Vector3(1f, 1f, 0f), 90f);

            Assert.That(ContainsSegment(lines, new Point2D(1, 1), new Point2D(1, 2), 1e-4), Is.True);
        }

        private static bool ContainsSegment(IEnumerable<LineSegment2D> lines, Point2D a, Point2D b, double tolerance)
        {
            foreach (var line in lines)
            {
                bool direct = line.StartPoint.Equals(a, tolerance) && line.EndPoint.Equals(b, tolerance);
                bool reverse = line.StartPoint.Equals(b, tolerance) && line.EndPoint.Equals(a, tolerance);
                if (direct || reverse)
                    return true;
            }

            return false;
        }
    }
}
