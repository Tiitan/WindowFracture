using System.Linq;
using MathNet.Spatial.Euclidean;
using NUnit.Framework;
using UnityEngine;
using WindowFracture.Runtime;

namespace Titan.Windowfracture.Tests
{
    public class ShardTests
    {
        [Test]
        public void InitializeShard_SetsPolygonData()
        {
            var panelGo = new GameObject("panel");
            var shardGo = new GameObject("shard");

            try
            {
                var panel = panelGo.AddComponent<GlassPanel>();
                var shard = shardGo.AddComponent<Shard>();
                var polygon = TestMeshFactory.CreatePolygon(
                    new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1));

                shard.InitializeShard(panel, polygon, null, 0.1f, new Vector2(2, 3));

                Assert.NotNull(shard.Polygon);
                Assert.That(shard.Polygon.Vertices.Count(), Is.EqualTo(3));
            }
            finally
            {
                Object.DestroyImmediate(shardGo);
                Object.DestroyImmediate(panelGo);
            }
        }

        [Test]
        public void GetEdgesInPanelSpace_AppliesCenterOffset()
        {
            var panelGo = new GameObject("panel");
            var shardGo = new GameObject("shard");

            try
            {
                var panel = panelGo.AddComponent<GlassPanel>();
                var shard = shardGo.AddComponent<Shard>();
                var polygon = TestMeshFactory.CreatePolygon(
                    new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1));

                shard.InitializeShard(panel, polygon, null, 0.1f, new Vector2(2, 3));

                var first = default(LineSegment2D);
                foreach (var edge in shard.GetEdgesInPanelSpace())
                {
                    first = edge;
                    break;
                }

                Assert.That(first.StartPoint.Equals(new Point2D(2, 3), 1e-5), Is.True);
                Assert.That(first.EndPoint.Equals(new Point2D(3, 3), 1e-5), Is.True);
            }
            finally
            {
                Object.DestroyImmediate(shardGo);
                Object.DestroyImmediate(panelGo);
            }
        }
    }
}
