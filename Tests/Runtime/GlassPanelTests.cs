using System.Collections.Generic;
using System.Reflection;
using MathNet.Spatial.Euclidean;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using WindowFracture.Runtime;

namespace Titan.Windowfracture.Tests
{
    public class GlassPanelTests
    {
        [Test]
        public void Break_SecondCallLogsAlreadyBroken()
        {
            var go = new GameObject("panel_no_mesh_twice");
            try
            {
                var panel = go.AddComponent<GlassPanel>();
                panel.Break(Vector3.zero, Vector3.forward);

                LogAssert.Expect(LogType.Error, "GlassPanel broken twice");
                panel.Break(Vector3.zero, Vector3.forward);
            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }

        [Test]
        public void OnNewShard_BuildsNeighborLinks_WhenEdgesMatch()
        {
            var panelGo = new GameObject("panel_neighbors");
            var shard1Go = new GameObject("shard_left");
            var shard2Go = new GameObject("shard_right");

            try
            {
                var panel = panelGo.AddComponent<GlassPanel>();
                SetGraphState(panel, new List<LineSegment2D>());

                var s1 = shard1Go.AddComponent<Shard>();
                s1.InitializeShard(panel,
                    TestMeshFactory.CreatePolygon(
                        new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1)),
                    null, 0.1f, Vector2.zero);

                var s2 = shard2Go.AddComponent<Shard>();
                s2.InitializeShard(panel,
                    TestMeshFactory.CreatePolygon(
                        new Vector2(1, 0), new Vector2(2, 0), new Vector2(2, 1), new Vector2(1, 1)),
                    null, 0.1f, Vector2.zero);

                panel.OnNewShard(s1);
                panel.OnNewShard(s2);

                var graph = GetPrivateField<Dictionary<Shard, HashSet<Shard>>>(panel, "_neighborGraph");
                Assert.That(graph[s1].Contains(s2), Is.True);
                Assert.That(graph[s2].Contains(s1), Is.True);
            }
            finally
            {
                Object.DestroyImmediate(shard1Go);
                Object.DestroyImmediate(shard2Go);
                Object.DestroyImmediate(panelGo);
            }
        }

        [Test]
        public void OnShardDestroyed_DropsDisconnectedShards()
        {
            var panelGo = new GameObject("panel_disconnect");
            var anchoredGo = new GameObject("anchored_shard");
            var freeGo = new GameObject("free_shard");

            try
            {
                var panel = panelGo.AddComponent<GlassPanel>();
                var frameEdges = new List<LineSegment2D>
                {
                    new LineSegment2D(new Point2D(0, 0), new Point2D(0, 1))
                };
                SetGraphState(panel, frameEdges);

                var anchored = anchoredGo.AddComponent<Shard>();
                anchored.InitializeShard(panel,
                    TestMeshFactory.CreatePolygon(
                        new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1)),
                    null, 0.1f, Vector2.zero);

                var free = freeGo.AddComponent<Shard>();
                free.InitializeShard(panel,
                    TestMeshFactory.CreatePolygon(
                        new Vector2(1, 0), new Vector2(2, 0), new Vector2(2, 1), new Vector2(1, 1)),
                    null, 0.1f, Vector2.zero);

                panel.OnNewShard(anchored);
                panel.OnNewShard(free);
                panel.OnShardDestroyed(anchored);

                var shards = GetPrivateField<List<Shard>>(panel, "_shards");
                Assert.That(shards.Count, Is.EqualTo(0));
                Assert.NotNull(free.GetComponent<Rigidbody>());
            }
            finally
            {
                Object.DestroyImmediate(anchoredGo);
                Object.DestroyImmediate(freeGo);
                Object.DestroyImmediate(panelGo);
            }
        }

        private static void SetGraphState(GlassPanel panel, List<LineSegment2D> frameEdges)
        {
            SetPrivateField(panel, "_shards", new List<Shard>());
            SetPrivateField(panel, "_neighborGraph", new Dictionary<Shard, HashSet<Shard>>());
            SetPrivateField(panel, "_anchoredShards", new HashSet<Shard>());
            SetPrivateField(panel, "_frameBoundaryEdges", frameEdges);
        }

        private static T GetPrivateField<T>(object target, string fieldName)
        {
            var field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(field, $"Missing field {fieldName}");
            return (T)field.GetValue(target);
        }

        private static void SetPrivateField(object target, string fieldName, object value)
        {
            var field = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(field, $"Missing field {fieldName}");
            field.SetValue(target, value);
        }
    }
}
