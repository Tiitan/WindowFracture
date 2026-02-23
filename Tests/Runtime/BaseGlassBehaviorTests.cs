using NUnit.Framework;
using UnityEngine;
using WindowFracture.Runtime;

namespace Titan.Windowfracture.Tests
{
    public class BaseGlassBehaviorTests
    {
        [Test]
        public void GlassPanel_BreakWithoutMeshFilter_DoesNotThrow()
        {
            var go = new GameObject("panel_no_mesh");
            try
            {
                var panel = go.AddComponent<GlassPanel>();
                Assert.DoesNotThrow(() => panel.Break(Vector3.zero, Vector3.forward));
            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }

        [Test]
        public void Fall_AddsRigidbodyAndDownwardVelocity()
        {
            var shardGo = new GameObject("fall_shard");
            try
            {
                var shard = shardGo.AddComponent<Shard>();
                var polygon = TestMeshFactory.CreatePolygon(
                    new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1));
                shard.InitializeShard(null, polygon, null, 0.1f, Vector2.zero);

                shard.Fall();

                var rb = shardGo.GetComponent<Rigidbody>();
                Assert.NotNull(rb);
                Assert.That(rb.mass, Is.GreaterThan(0f));
                Assert.That(rb.linearVelocity.y, Is.LessThan(0f));
            }
            finally
            {
                Object.DestroyImmediate(shardGo);
            }
        }
    }
}
