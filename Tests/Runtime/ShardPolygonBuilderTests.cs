using System.Collections.Generic;
using System.Linq;
using MathNet.Spatial.Euclidean;
using NUnit.Framework;
using UnityEngine;
using WindowFracture.Runtime;
using WindowFracture.Runtime;

namespace Titan.Windowfracture.Tests
{
    public class ShardPolygonBuilderTests
    {
        [Test]
        public void Build_WithClipPatternOutput_ReturnsValidPolygons()
        {
            var mask = TestMeshFactory.CreatePolygon(
                new Vector2(0f, 0f),
                new Vector2(2f, 0f),
                new Vector2(2f, 2f),
                new Vector2(0f, 2f));
            var pattern = TestMeshFactory.CreateLineMesh(
                new Vector3(-1f, 1f, 0f), new Vector3(3f, 1f, 0f));

            List<LineSegment2D> lines = ClipPattern.Clip(pattern, mask, Vector3.zero, 0f);
            List<Polygon2D> shards = ShardPolygonBuilder.Build(lines, 0.001f);

            Assert.That(shards.Count, Is.GreaterThan(0));
            Assert.That(shards.All(s => s.Vertices.Count() >= 3), Is.True);
        }

        [Test]
        public void Build_DuplicateAndMicroLines_DoNotChangeResult()
        {
            var mask = TestMeshFactory.CreatePolygon(
                new Vector2(0f, 0f),
                new Vector2(2f, 0f),
                new Vector2(2f, 2f),
                new Vector2(0f, 2f));
            var pattern = TestMeshFactory.CreateLineMesh(
                new Vector3(-1f, 1f, 0f), new Vector3(3f, 1f, 0f));

            List<LineSegment2D> clean = ClipPattern.Clip(pattern, mask, Vector3.zero, 0f);
            var noisy = new List<LineSegment2D>(clean);
            if (clean.Count > 0)
                noisy.Add(clean[0]);
            noisy.Add(new LineSegment2D(new Point2D(0.5, 0.5), new Point2D(0.5002, 0.5002)));

            List<Polygon2D> cleanShards = null;
            List<Polygon2D> noisyShards = null;

            Assert.DoesNotThrow(() => cleanShards = ShardPolygonBuilder.Build(clean, 0.001f));
            Assert.DoesNotThrow(() => noisyShards = ShardPolygonBuilder.Build(noisy, 0.001f));

            Assert.NotNull(cleanShards);
            Assert.NotNull(noisyShards);
            Assert.That(cleanShards.Count, Is.GreaterThan(0));
            Assert.That(noisyShards.Count, Is.EqualTo(cleanShards.Count));
            Assert.That(noisyShards.All(s => s.Vertices.Count() >= 3), Is.True);
        }

        private static void AddLoop(List<LineSegment2D> lines, IReadOnlyList<Vector2> points)
        {
            for (int i = 0; i < points.Count; i++)
            {
                Vector2 a = points[i];
                Vector2 b = points[(i + 1) % points.Count];
                lines.Add(new LineSegment2D(new Point2D(a.x, a.y), new Point2D(b.x, b.y)));
            }
        }
    }
}
